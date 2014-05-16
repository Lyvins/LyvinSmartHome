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
//  File            :   LyvinUserGroup.cs                                //
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
using System.Linq;

namespace LyvinObjectsLib.Users
{
    public class LyvinUserGroup
    {
        private List<LyvinUser> users;

        public LyvinUserGroup()
        {
            users = new List<LyvinUser>();
        }

        /// <summary>
        /// A constructor for the user group
        /// </summary>
        /// <param name="userGroupID">The unique id of the usergroup</param>
        /// <param name="userGroupName">The name of the usergroup</param>
        public LyvinUserGroup(string userGroupID, string userGroupName)
        {
            UserGroupID = userGroupID;
            UserGroupName = userGroupName;
            users = new List<LyvinUser>();
        }

        /// <summary>
        /// Adds a user to the usergroup
        /// </summary>
        /// <param name="user">The user to be added</param>
        public void AddUser(LyvinUser user)
        {
            if (!users.Contains(user))
            {
                users.Add(user);
            }
        }

        /// <summary>
        /// Checks whether the user group contains a specific user
        /// </summary>
        /// <param name="userID">The unique id of the user</param>
        /// <returns>True if the usergroup contains the user, otherwise false</returns>
        public bool Contains(string userID)
        {
            return users.Any(u => u.UserID == userID);
        }

        /// <summary>
        /// Lists all users in the user group
        /// </summary>
        /// <returns>A list of all users in the user group</returns>
        public List<LyvinUser> ListUsers()
        {
            return users;
        }

        /// <summary>
        /// Removes a specific user from the user group
        /// </summary>
        /// <param name="userID">The id of the user to be removed</param>
        public void RemoveUser(string userID)
        {
            if (users.Any(u => u.UserID == userID))
            {
                users.Remove(users.Single(u => u.UserID == userID));
            }
        }

        /// <summary>
        /// The unique id of the usergroup
        /// </summary>
        public string UserGroupID { get; set; }

        /// <summary>
        /// The name of the usergroup
        /// </summary>
        public string UserGroupName { get; set; }

    }
}