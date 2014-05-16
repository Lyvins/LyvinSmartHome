//----------------------------------------------------------------------//
//  ___           ___    ___ ___      ___ ___  ________   ________      //
// |\  \         |\  \  /  /|\  \    /  /|\  \|\   ___  \|\   ____\     //
// \ \  \        \ \  \/  / | \  \  /  / | \  \ \  \\ \  \ \  \___|_    //
//  \ \  \        \ \    / / \ \  \/  / / \ \  \ \  \\ \  \ \_____  \   //
//   \ \  \____    \/  /  /   \ \    / /   \ \  \ \  \\ \  \|____|\  \  //
//    \ \_______\__/  / /      \ \__/ /     \ \__\ \__\\ \__\____\_\  \ //
//     \|_______|\___/ /        \|__|/       \|__|\|__| \|__|\_________\//
//              \|___|/                                     \|_________|//
//                                                                      //
//----------------------------------------------------------------------//
//  File            :   Cryptography.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinSystemLogicLib                                     //
//  Created on      :   17-5-2014                                        //
//----------------------------------------------------------------------//
//                                                                      //
//  Copyright (c) 2013, 2014 Lyvins                                     //
//                                                                      //
//  Licensed under the Apache License, Version 2.0 (the "License");     //
//  you may not use this file except in compliance with the License.    //
//  You may obtain a copy of the License at                             //
//                                                                      //
//      http://www.apache.org/licenses/LICENSE-2.0                      //
//                                                                      //
//  Unless required by applicable law or agreed to in writing, software //
//  distributed under the License is distributed on an "AS IS" BASIS,   //
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or     //
//  implied. See the License for the specific language governing        //
//  permissions and limitations under the License.                      //
//                                                                      //
//----------------------------------------------------------------------//
//                                                                      //
//  Prerequisites / Return Codes / Notes                                //
//                                                                      //
//----------------------------------------------------------------------//
//                                                                      //
//  Tasks / Features / Bugs                                             //
//                                                                      //
//----------------------------------------------------------------------//

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LyvinSystemLogicLib
{
    public static class Cryptography
    {

        private const int HashLoop = 100;
        private static readonly byte[] Salt = {0, 0, 0, 0, 0, 0, 0, 0}; // The key salt used to derive the key

        private static readonly byte[] VectorBytes = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                                       // The initialization vector to be used for the symmetric algorithm

        private static readonly byte[] keyBytes;

        private static readonly RijndaelManaged rijKey;

        /// <summary>
        /// The constructor for the Cryptography class
        /// </summary>
        static Cryptography()
        {
            // Read a proper password from WMI data
            string Password = GetDiskSerial();

            // The password for which to derive the key
            //Password = "a465apyis4h5g132sdf32a69fg96dxcmcvhf4oe5x0vb0b6s3zh1asd02f1asdg654as";

            // Initialize new RijndaelManaged class
            rijKey = new RijndaelManaged();
            rijKey.Mode = CipherMode.CBC;
            rijKey.Padding = PaddingMode.PKCS7;

            // Derive the key from the password
            PasswordDeriveBytes cdk = new PasswordDeriveBytes(Password, Salt);
            //string kex = Convert.ToBase64String(cdk.CryptDeriveKey("RC2", "SHA1", 128, salt));
            keyBytes = cdk.CryptDeriveKey("RC2", "SHA1", 128, Salt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedFile"></param>
        /// <returns></returns>
        public static CryptoStream DecryptFileToStream(FileStream encryptedFile)
        {
            // Initialize Cryptography and setting the keyBytes to be used
            //byte[] keyBytes = InitializeCryptography();

            // Decrypt the file and put the data in a memorystream
            ICryptoTransform decryptor = rijKey.CreateDecryptor(keyBytes, VectorBytes);
            CryptoStream cryptoStream = new CryptoStream(encryptedFile, decryptor, CryptoStreamMode.Read);

            return cryptoStream;
        }

        /// 
        /// <param name="TextStream"></param>
        /// <param name="encryptedFile"></param>
        public static void EncryptStreamToFile(MemoryStream TextStream, FileStream encryptedFile)
        {
            // convert the stream to bytes
            byte[] textBytes = TextStream.ToArray();

            // Encrypt the data and put the encrypted data into the file
            ICryptoTransform encryptor = rijKey.CreateEncryptor(keyBytes, VectorBytes);
            CryptoStream cryptoStream = new CryptoStream(encryptedFile, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(textBytes, 0, textBytes.Length);
            cryptoStream.FlushFinalBlock();

            // Flush the last data to the file and close the streams
            encryptedFile.Flush();
            encryptedFile.Close();
            cryptoStream.Close();

        }

        /// <summary>
        /// Hashes the password using a SHA512 encryption. It uses a predetermined salt for the encryption.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The hashed password</returns>
        public static string HashPassword(string password)
        {
            return HashPassword(Salt.ToString(), password);
        }

        /// <summary>
        /// Hashes the password and salt using a SHA512 encryption.
        /// </summary>
        /// <param name="salt">A salt used for extra variation and protection.</param>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The hashed password</returns>
        public static string HashPassword(string salt, string password)
        {
            string tmpPassword;

            for (int i = 0; i < HashLoop; i++)
            {
                tmpPassword = "";
                for (int j = 0; j < password.Length; j++)
                {
                    tmpPassword = tmpPassword + password[j];
                    if (j < salt.Length)
                    {
                        tmpPassword = tmpPassword + salt[salt.Length - j - 1];
                    }
                }

                //Convert the password string into an Array of bytes.

                UTF8Encoding textConverter = new UTF8Encoding();
                byte[] passBytes = textConverter.GetBytes(tmpPassword);

                //Return the encrypted bytes

                //MessageBox.Show(textConverter.GetString(new SHA384Managed().ComputeHash(passBytes)));
                password = textConverter.GetString(new SHA512Managed().ComputeHash(passBytes));
            }
            return password;
        }

        /// <summary>
        /// Gets the serial key of the first disk drive
        /// </summary>
        /// <returns>The serial key of the first disk drive</returns>
        private static string GetDiskSerial()
        {
            string ret = "";
#if !DEBUG
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
            "SELECT SerialNumber FROM Win32_DiskDrive WHERE DeviceID LIKE '%PHYSICALDRIVE0'");
            try
            {
                foreach (ManagementBaseObject m in searcher.Get())
                {
                    foreach (PropertyData data in m.Properties)
                    {
                        ret = data.Value.ToString();
                        break;
                    }
                }
            }
            catch (ManagementException) { }
#endif
            return ret;
        }

    }
}