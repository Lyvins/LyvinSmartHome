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
//  File            :   XMLParser.cs                                //
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LyvinSystemLogicLib
{
    /// <summary>
    /// XMLParser is mainly a static class which allows to read and write XML in a very Generic way. 
    /// It will handle all possible XML parse errors.
    /// Hence when this class is used it's not necessary to catch errors in you own logic
    /// </summary>
    public static class XMLParser
    {
        static XMLParser()
        {
            
        }

        /// <summary>
        /// Returns a List of Objects read from 'file' specified by 'type'
        /// </summary>
        /// <param name="type">The class type for which to parse XML</param>
        /// <param name="file">The file to read the XML from</param>
        /// <param name="encrypted"> </param>
        /// <param name="fqfn"> </param>
        /// <returns>A List containing all the gathered object from XML</returns>
        public static List<Object> GetObjectsFromFile(Type type, string file, bool encrypted, bool fqfn)
        {
            return GetObjectsFromFile(type, file, "", encrypted, fqfn);
        }

        /// <summary>
        /// Read XML objects from file with a Fully Qualified File Name, with optional encryption
        /// </summary>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public static List<Object> GetObjectsFromFile(Type type, string file, bool encrypt)
        {
            return GetObjectsFromFile(type, file, "", encrypt, true);
        }

        /// <summary>
        /// Read XML objects from file with a Fully Qualified File Name, without encryption
        /// </summary>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<Object> GetObjectsFromFile(Type type, string file)
        {
            return GetObjectsFromFile(type, file, "", false, true);
        }

        /// <summary>
        /// Returns a List of Objects read from 'file' specified by 'type', 'fileAddition' is appended to 'file' to create the full filename
        /// </summary>
        /// <param name="type">The class type for which to parse XML</param>
        /// <param name="file">The file to read the XML from</param>
        /// <param name="fileAddition">An extra FileName addition, e.g. ProductionList1, Produciontlist2 etc.</param>
        /// <param name="encrypted">Whether or not the file to read is encrypted</param>
        /// <param name="fqfn">Whether or not the filename has been provided AS IS, no additions will be added like .xml or .enc</param>
        /// <returns>A List containing all the gathered object from XML</returns>
        public static List<Object> GetObjectsFromFile(Type type, string file, string fileAddition, bool encrypted, bool fqfn)
        {
            return GetObjectsFromFile<Object>(file, fileAddition, encrypted, fqfn);
        }

        /// <summary>
        /// Returns a List of Objects read from 'file' specified by 'type', 'fileAddition' is appended to 'file' to create the full filename
        /// </summary>
        /// <param name="type">The class type for which to parse XML</param>
        /// <param name="file">The file to read the XML from</param>
        /// <param name="fileAddition">An extra FileName addition, e.g. ProductionList1, Produciontlist2 etc.</param>
        /// <param name="encrypted">Whether or not the file to read is encrypted</param>
        /// <param name="fqfn">Whether or not the filename has been provided AS IS, no additions will be added like .xml or .enc</param>
        /// <returns>A List containing all the gathered object from XML</returns>
        public static List<T> GetObjectsFromFile<T>(string file, string fileAddition, bool encrypted, bool fqfn)
        {
            Type type = typeof (T);
            string fileName = file;
            if (!fileAddition.Equals(""))
            {
                fileName = fileName + fileAddition;
            }

            List<T> objects = new List<T>();

            bool xmlNullElement = false;

            XmlTextReader xmlTextReader = null;
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            if (encrypted)
            {
                if (!fqfn)
                {
                    fileName = fileName + ".enc";
                }
                if (File.Exists(fileName))
                {
                    FileStream encryptedFile = File.OpenRead(fileName);

                    CryptoStream cryptoStream = Cryptography.DecryptFileToStream(encryptedFile);
                    try
                    {
                        xmlTextReader = new XmlTextReader(cryptoStream);
                    }
                    catch (CryptographicException)
                    {
                        ReportXMLError(fileName);
                    }
                }
                else
                {
                    Logger.LogItem("XML file to be parsed does not exist: " + fileName, LogType.ERROR);
                }
            }
            else
            {
                if (!fqfn)
                {
                    fileName = fileName + ".xml";
                }
                if (File.Exists(fileName))
                {
                    xmlTextReader = new XmlTextReader(fileName);
                }
                else
                {
                    Logger.LogItem("XML file to be parsed does not exist: " + fileName, LogType.ERROR);
                }
            }

            if (xmlTextReader != null)
            {
                try
                {
                    Logger.LogItem("Parsing xml file " + fileName + " to objects: " + type.ToString(), LogType.SYSTEM);
                    xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
                    xmlTextReader.Read(); // XML header
                    xmlTextReader.Read(); // Root element
                    xmlTextReader.Read(); // First element

                    var addtempitem = xmlSerializer.Deserialize(xmlTextReader);
                    while (!xmlTextReader.EOF)
                    {
                        if (addtempitem != null)
                        {
                            if (!xmlNullElement)
                            {
                                objects.Add((T)addtempitem);
                                addtempitem = xmlSerializer.Deserialize(xmlTextReader);
                            }
                            else
                            {
                                throw (new XmlException("XML file is corrupt."));
                            }
                        }
                        else
                        {
                            xmlNullElement = true;
                            xmlTextReader.Read();
                            if (!xmlTextReader.EOF)
                            {
                                addtempitem = xmlSerializer.Deserialize(xmlTextReader);
                            }
                        }
                    }

                    xmlTextReader.Close();
                }
                catch (ArgumentException)
                {
                    ReportXMLError(fileName);
                }
                catch (InvalidOperationException)
                {
                    ReportXMLError(fileName);
                }
                catch (XmlException)
                {
                    ReportXMLError(fileName);
                }
                xmlTextReader.Close();
            }
            return objects;
        }

        /// <summary>
        /// Returns an object read from 'file' specified by 'type', 'fileAddition' is appended to 'file' to create the full filename
        /// </summary>
        /// <param name="type">The class type for which to parse XML</param>
        /// <param name="file">The file to read the XML from</param>
        /// <param name="fileAddition">An extra FileName addition, e.g. ProductionList1, Produciontlist2 etc.</param>
        /// <param name="encrypted">Whether or not the file to read is encrypted</param>
        /// <param name="fqfn">Whether or not the filename has been provided AS IS, no additions will be added like .xml or .enc</param>
        /// <returns>A List containing all the gathered object from XML</returns>
        public static Object GetObjectFromFile(Type type, string file, string fileAddition, bool encrypted, bool fqfn)
        {
            return GetObjectFromFile<Object>(file, fileAddition, encrypted, fqfn);
        }

        /// <summary>
        /// Returns an object read from 'file' specified by 'type', 'fileAddition' is appended to 'file' to create the full filename
        /// </summary>
        /// <param name="file">The file to read the XML from</param>
        /// <param name="fileAddition">An extra FileName addition, e.g. ProductionList1, Produciontlist2 etc.</param>
        /// <param name="encrypted">Whether or not the file to read is encrypted</param>
        /// <param name="fqfn">Whether or not the filename has been provided AS IS, no additions will be added like .xml or .enc</param>
        /// <returns>A List containing all the gathered object from XML</returns>
        public static T GetObjectFromFile<T>(string file, string fileAddition, bool encrypted, bool fqfn)
        {
            Type type = typeof (T);
            string fileName = file;
            if (!fileAddition.Equals(""))
            {
                fileName = fileName + fileAddition;
            }

            Object o = new Object();

            bool xmlNullElement = false;

            XmlTextReader xmlTextReader = null;
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            if (encrypted)
            {
                if (!fqfn)
                {
                    fileName = fileName + ".enc";
                }
                if (File.Exists(fileName))
                {
                    FileStream encryptedFile = File.OpenRead(fileName);

                    CryptoStream cryptoStream = Cryptography.DecryptFileToStream(encryptedFile);
                    try
                    {
                        xmlTextReader = new XmlTextReader(cryptoStream);
                    }
                    catch (CryptographicException)
                    {
                        ReportXMLError(fileName);
                    }
                }
                else
                {
                    Logger.LogItem("XML file to be parsed does not exist: " + fileName, LogType.ERROR);
                }
            }
            else
            {
                if (!fqfn)
                {
                    fileName = fileName + ".xml";
                }
                if (File.Exists(fileName))
                {
                    xmlTextReader = new XmlTextReader(fileName);
                }
                else
                {
                    Logger.LogItem("XML file to be parsed does not exist: " + fileName, LogType.ERROR);
                }
            }

            if (xmlTextReader != null)
            {
                try
                {
                    Logger.LogItem("Parsing xml file " + fileName + " to objects: " + type.ToString(), LogType.SYSTEM);
                    xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
                    xmlTextReader.Read(); // XML header
                    xmlTextReader.Read(); // Root element

                    o = xmlSerializer.Deserialize(xmlTextReader);
                    
                    xmlTextReader.Close();
                }
                catch (ArgumentException)
                {
                    ReportXMLError(fileName);
                }
                catch (InvalidOperationException)
                {
                    ReportXMLError(fileName);
                }
                catch (XmlException)
                {
                    ReportXMLError(fileName);
                }
                xmlTextReader.Close();
            }
            return (T)o;
        }

        /// <summary>
        /// Helper function to report Errors while parsing XML
        /// </summary>
        /// <param name="file">The file with errors</param>
        private static void ReportXMLError(string file)
        {
            Logger.LogItem("The XML file is corrupt: " + file + ". The application will now close.", LogType.ERROR);
            
            throw (new Exception("XML corrupt " + file + ". The application will close."));
        }

        /// <summary>
        /// Write XML objects to file with a Fully Qualified File Name, without encryption
        /// </summary>
        /// <param name="file">Fully Qualified File Name</param>
        /// <param name="objects">The List of objects to write to the file</param>
        /// <param name="type">The type of objects which will be written</param>
        public static void WriteXMLFromObjects(string file, List<Object> objects, Type type)
        {
            WriteXMLFromObjects(file, "", objects, type, false, true);
        }

        /// <summary>
        /// Write XML objects to file with a Fully Qualified File Name, with encryption
        /// </summary>
        /// <param name="file">Fully Qualified File Name</param>
        /// <param name="objects">The List of objects to write to the file</param>
        /// <param name="type">The type of objects which will be written</param>
        /// <param name="encrypt">Should the data be written encrypted</param>
        public static void WriteXMLFromObjects(string file, List<Object> objects, Type type, bool encrypt)
        {
            WriteXMLFromObjects(file, "", objects, type, encrypt, true);
        }

        /// <summary>
        /// Write XML objects to file with a Fully Qualified File Name, with encryption
        /// </summary>
        /// <param name="file">Fully Qualified File Name</param>
        /// <param name="objects">The List of objects to write to the file</param>
        /// <param name="type">The type of objects which will be written</param>
        /// <param name="encrypt">Should the data be written encrypted</param>
        /// <param name="fqfn">Whether or not the filename has been provided AS IS, no additions will be added like .xml or .enc</param>
        public static void WriteXMLFromObjects(string file, List<Object> objects, Type type, bool encrypt, bool fqfn)
        {
            WriteXMLFromObjects(file, "", objects, type, encrypt, fqfn);
        }

        /// <summary>
        /// Writes the 'List' of 'Objects' in XML dialect to 'file' in form of the specified 'type'
        /// </summary>
        /// <param name="file">The file to write to</param>
        /// <param name="fileAddition">filename addition</param>
        /// <param name="objects">The List of objects to write to the file</param>
        /// <param name="type">The type of objects which will be written</param>
        /// <param name="encrypt">Should the data be written encrypted</param>
        /// <param name="fqfn">Whether or not the filename has been provided AS IS, no additions will be added like .xml or .enc</param>
        public static void WriteXMLFromObjects(string file, string fileAddition, List<Object> objects, Type type, bool encrypt, bool fqfn)
        {
            WriteXMLFromObjects<Object>(file, fileAddition, objects, encrypt, fqfn);
        }

        /// <summary>
        /// Writes the 'List' of 'Objects' in XML dialect to 'file' in form of the specified 'type'
        /// </summary>
        /// <param name="file">The file to write to</param>
        /// <param name="fileAddition">filename addition</param>
        /// <param name="objects">The List of objects to write to the file</param>
        /// <param name="encrypt">Should the data be written encrypted</param>
        /// <param name="fqfn">Whether or not the filename has been provided AS IS, no additions will be added like .xml or .enc</param>
        public static void WriteXMLFromObjects<T>(string file, string fileAddition, List<T> objects, bool encrypt, bool fqfn)
        {
            XmlTextWriter xmlTextWriter;
            MemoryStream memoryStream = null;
            Type type = typeof (T);

            string fileName = file;
            if (!fileAddition.Equals(""))
            {
                fileName = fileName + fileAddition;
            }

            if (fileName.Contains("\\"))
            {
                var dir = fileName.Substring(0, fileName.LastIndexOf('\\'));
                var di = new DirectoryInfo(dir);
                if (!di.Exists)
                {
                    di.Create();
                }
            }

            if (encrypt)
            {
                if (!fqfn)
                {
                    fileName = fileName + ".enc";
                }
                memoryStream = new MemoryStream();
                xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            }
            else
            {
                if (!fqfn)
                {
                    fileName = fileName + ".xml";
                }
                xmlTextWriter = new XmlTextWriter(fileName, Encoding.UTF8);
            }

            Logger.LogItem("Writing xml file " + fileName + " from objects: " + type.ToString(), LogType.SYSTEM);

            XmlSerializer xmlSerializer = new XmlSerializer(type);
            XmlSerializerNamespaces EmptyNamespace = new XmlSerializerNamespaces();

            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement(type.ToString().EndsWith("s") ? type + "es" : type + "s");
            EmptyNamespace.Add("", "");

            foreach (var item in objects)
            {
                xmlSerializer.Serialize(xmlTextWriter, item, EmptyNamespace);
            }

            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();

            if (encrypt)
            {
                FileStream encryptedFile = File.Create(fileName);
                Cryptography.EncryptStreamToFile(memoryStream, encryptedFile);
                memoryStream.Close();
            }
        }

        /// <summary>
        /// Writes the object in XML dialect to 'file' in form of the specified 'type'
        /// </summary>
        /// <param name="file">The file to write to</param>
        /// <param name="fileAddition">filename addition</param>
        /// <param name="o">The object to write to the file</param>
        /// <param name="type">The type of object which will be written</param>
        /// <param name="encrypt">Should the data be written encrypted</param>
        /// <param name="fqfn">Whether or not the filename has been provided AS IS, no additions will be added like .xml or .enc</param>
        public static void WriteXMLFromObject(string file, string fileAddition, Object o, Type type, bool encrypt, bool fqfn)
        {
            WriteXMLFromObject<Object>(file, fileAddition, o, encrypt, fqfn);
        }

        /// <summary>
        /// Writes the object in XML dialect to 'file' in form of the specified 'type'
        /// </summary>
        /// <param name="file">The file to write to</param>
        /// <param name="fileAddition">filename addition</param>
        /// <param name="o">The object to write to the file</param>
        /// <param name="encrypt">Should the data be written encrypted</param>
        /// <param name="fqfn">Whether or not the filename has been provided AS IS, no additions will be added like .xml or .enc</param>
        public static void WriteXMLFromObject<T>(string file, string fileAddition, T o, bool encrypt, bool fqfn)
        {
            Type type = typeof (T);
            XmlTextWriter xmlTextWriter;
            MemoryStream memoryStream = null;

            string fileName = file;
            if (!fileAddition.Equals(""))
            {
                fileName = fileName + fileAddition;
            }

            if (fileName.Contains("\\"))
            {
                var dir = fileName.Substring(0, fileName.LastIndexOf('\\'));
                var di = new DirectoryInfo(dir);
                if (!di.Exists)
                {
                    di.Create();
                }
            }

            if (encrypt)
            {
                if (!fqfn)
                {
                    fileName = fileName + ".enc";
                }
                memoryStream = new MemoryStream();
                xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            }
            else
            {
                if (!fqfn)
                {
                    fileName = fileName + ".xml";
                }
                xmlTextWriter = new XmlTextWriter(fileName, Encoding.UTF8);
            }

            Logger.LogItem("Writing xml file " + fileName + " from objects: " + type, LogType.SYSTEM);

            XmlSerializer xmlSerializer = new XmlSerializer(type);
            XmlSerializerNamespaces EmptyNamespace = new XmlSerializerNamespaces();

            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.WriteStartDocument();

            xmlSerializer.Serialize(xmlTextWriter, o, EmptyNamespace);

            xmlTextWriter.WriteEndDocument();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();

            if (encrypt)
            {
                var encryptedFile = File.Create(fileName);
                Cryptography.EncryptStreamToFile(memoryStream, encryptedFile);
                memoryStream.Close();
            }
        }
    }
}
