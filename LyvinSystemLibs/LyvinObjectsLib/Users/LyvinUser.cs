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
//  File            :   LyvinUser.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinObjectsLib                                     //
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

using System.Collections.Generic;

namespace LyvinObjectsLib.Users
{
    public class LyvinUser
    {
        public LyvinUser()
        {

        }

        /// <summary>
        /// A constructor for the user object
        /// </summary>
        /// <param name="userID">The unique ID of a user</param>
        /// <param name="firstName">The first name of the user</param>
        /// <param name="middleName">The middle name of the user</param>
        /// <param name="lastName">The last name of the user</param>
        /// <param name="passwordHash">The password hash of the user</param>
        public LyvinUser(string userID, string firstName, string middleName, string lastName, string passwordHash)
        {
            UserID = userID;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PasswordHash = passwordHash;
        }

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The middle name of the user
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// The passowrd hash of the user
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// The current permissions of the user
        /// </summary>
        public List<string> Permissions { get; set; }

        /// <summary>
        /// The primary usergroup of the user
        /// </summary>
        public LyvinUserGroup PrimaryUserGroup { get; set; }

        /// <summary>
        /// The unnique ID of the user
        /// </summary>
        public string UserID { get; set; }

    }
}