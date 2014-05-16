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
//  File            :   UserManager.cs                                //
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace LyvinObjectsLib.Users
{
    public class UserManager
    {
        private List<LyvinUserGroup> userGroups;

        private List<LyvinUser> users;

        public UserManager()
        {
            userGroups = new List<LyvinUserGroup>();
            users = new List<LyvinUser>();
        }

        /// <summary>
        /// Adds a user to the list of users, if the user does not already exist
        /// </summary>
        /// <param name="user">the user to be added</param>
        public void AddUser(LyvinUser user)
        {
            if (users.All(u => u.UserID != user.UserID))
            {
                users.Add(user);
            }
        }

        /// <summary>
        /// Adds a usergroup to the list of usergroups, if the usergroup does not already exist
        /// </summary>
        /// <param name="userGroup">The usergroup to be added</param>
        public void AddUserGroup(LyvinUserGroup userGroup)
        {
            if (userGroups.All(u => u.UserGroupID != userGroup.UserGroupID))
            {
                userGroups.Add(userGroup);
            }
        }

        /// <summary>
        /// Checks if the user has a specific permission
        /// </summary>
        /// <param name="userID">The unique id of the user</param>
        /// <param name="permission">The permission to be checked</param>
        /// <returns>True if the user has the permission, otherwise false</returns>
        public bool CheckUserPermission(string userID, string permission)
        {
            try
            {
                return users.Single(u => u.UserID == userID).Permissions.Contains(permission);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a specific user
        /// </summary>
        /// <param name="userID">The unique user ID of the user to be gotten</param>
        /// <returns>The user containing the user id, or null if none exist</returns>
        public LyvinUser GetUser(string userID)
        {
            try
            {
                return users.Single(u => u.UserID == userID);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specific user group
        /// </summary>
        /// <param name="userGroupID">The unique id of the user group to be gotten</param>
        /// <returns>The usergroup containing the user group id</returns>
        public LyvinUserGroup GetUserGroup(string userGroupID)
        {
            try
            {
                return userGroups.Single(u => u.UserGroupID == userGroupID);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Lists all usergroups
        /// </summary>
        /// <returns>Returns a list of all usergroups</returns>
        public List<LyvinUserGroup> ListUserGroups()
        {
            return userGroups;
        }

        /// <summary>
        /// Lists all Users
        /// </summary>
        /// <returns>Returns a list of all users</returns>
        public List<LyvinUser> ListUsers()
        {
            return users;
        }

        /// <summary>
        /// Removes a specific user from the user list
        /// </summary>
        /// <param name="userID">The unique user id of the user to be removed</param>
        public void RemoveUser(string userID)
        {
            try
            {
                users.Remove(users.Single(u => u.UserID == userID));
            }
            catch (ArgumentNullException)
            {
                //Users does not exist
            }
            catch (InvalidOperationException)
            {
                //User does not exist
            }
        }

        /// <summary>
        /// Removes a specific usergroup from the lists of usergroups
        /// </summary>
        /// <param name="userGroupID">The unique id of the usergroup to be removed</param>
        public void RemoveUserGroup(string userGroupID)
        {
            try
            {
                userGroups.Remove(userGroups.Single(u => u.UserGroupID == userGroupID));
            }
            catch (ArgumentNullException)
            {
                //UserGroups does not exist
            }
            catch (InvalidOperationException)
            {
                //UserGroup does not exist
            }
        }

    }
}