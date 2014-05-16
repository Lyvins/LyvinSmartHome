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
//  File            :   PolicyManager.cs                                //
//  Description     :   Contains all policy data                        //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   05-07-2013                                      //
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
//  ToDo: Move to OS and move all policies to Data Store                //
//                                                                      //
//----------------------------------------------------------------------//

using System.Collections.Generic;
using System.Linq;
using LyvinObjectsLib.Users;

namespace LyvinOS.OS.Security
{
    /// <summary>
    /// Contains all policy data
    /// </summary>
    public class PolicyManager
    {
        private readonly List<Policy> globalPolicies;

        private readonly List<UserGroupPolicy> userGroupPolicies;

        private readonly List<UserPolicy> userPolicies;

        private readonly UserManager userManager;

        public PolicyManager(UserManager usermanager)
        {
            globalPolicies = new List<Policy>();
            userGroupPolicies = new List<UserGroupPolicy>();
            userPolicies = new List<UserPolicy>();
            userManager = usermanager;
        }

        /// 
        /// <param name="policy"></param>
        public void AddGlobalPolicy(Policy policy)
        {

        }

        /// 
        /// <param name="policy"></param>
        /// <param name="userGroup"></param>
        public void AddUserGroupPolicy(UserGroupPolicy policy, LyvinUserGroup userGroup)
        {

        }

        /// 
        /// <param name="policy"></param>
        /// <param name="user"></param>
        public void AddUserPolicy(UserPolicy policy, LyvinUser user)
        {

        }

        public List<Policy> GetGlobalPolicies()
        {

            return null;
        }

        /// 
        /// <param name="userGroupID"></param>
        public List<Policy> GetUserGroupPolicies(string userGroupID)
        {

            return null;
        }

        /// 
        /// <param name="userID"></param>
        public List<Policy> GetUserPolicies(string userID)
        {

            return null;
        }

        /// 
        /// <param name="policyID"></param>
        public void RemovePolicy(string policyID)
        {

        }

        /// 
        /// <param name="userList"></param>
        /// <param name="policy"></param>
        public List<LyvinUser> UsersCanGrantPolicy(List<LyvinUser> userList, Policy policy)
        {

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policies"></param>
        /// <returns></returns>
        public List<LyvinUser> UsersCanGrantPolicies(List<Policy> policies)
        {
            var usersCanGrantPolicies = new List<LyvinUser>();
            var grantAll = new Policy("CAN_GRANT_ALL_POLICIES", "Grant All", "This user can grant all policies",
                                         "All_Policies", "Policy", "", "");
            var grantOwn = new Policy("CAN_GRANT_OWN_POLICIES", "Grant Own", "This user can grant his own policies",
                                         "Own_Policies", "Policy", "", "");
            var denyOnAll = false;
            var denyOnOwn = false;
            var userGroupsDenyOnAll = new List<LyvinUserGroup>();
            var userGroupsDenyOnOwn = new List<LyvinUserGroup>();

            if (CheckGlobalPolicy(grantAll)) denyOnAll = true;
            if (CheckGlobalPolicy(grantOwn)) denyOnOwn = true;

            if (denyOnAll && denyOnOwn)
            {
                return usersCanGrantPolicies;
            }

            foreach (var userGroup in userManager.ListUserGroups())
            {
                if (CheckUserGroupPolicy(grantAll, userGroup.UserGroupID)) userGroupsDenyOnAll.Add(userGroup);
                if (CheckUserGroupPolicy(grantOwn, userGroup.UserGroupID)) userGroupsDenyOnOwn.Add(userGroup);
            }

            foreach (var user in userManager.ListUsers())
            {
                if (!denyOnAll)
                {
                    if (!userGroupsDenyOnAll.Any(ug => ug.ListUsers().Contains(user)))
                    {
                        if (!CheckUserPolicy(grantAll, user.UserID))
                        {
                            usersCanGrantPolicies.Add(user);
                        }
                    }
                }
                if (!usersCanGrantPolicies.Contains(user))
                {
                    if (!denyOnOwn)
                    {
                        if (!userGroupsDenyOnOwn.Any(ug => ug.ListUsers().Contains(user)))
                        {
                            if (!CheckUserPolicy(grantOwn, user.UserID))
                            {
                                bool canGrant = true;
                                foreach (var policy in policies)
                                {
                                    if (CheckUserPolicy(policy, user.UserID)) canGrant = false;
                                }
                                if (canGrant) usersCanGrantPolicies.Add(user);
                            }
                        }
                    }
                }
            }

            return usersCanGrantPolicies;
        }

        public bool CheckGlobalPolicy(Policy policy)
        {
            return globalPolicies.Any(gp => gp.PolicyID == policy.PolicyID);
        }

        public bool CheckUserPolicy(Policy policy, string userID)
        {
            return userPolicies.Any(up => ((up.Policy.PolicyID == policy.PolicyID) && (up.User.UserID == userID)));
        }

        public bool CheckUserGroupPolicy(Policy policy, string userGroupID)
        {
            return
                userGroupPolicies.Any(
                    ugp => ((ugp.Policy.PolicyID == policy.PolicyID) && (ugp.UserGroup.UserGroupID == userGroupID)));
        }
    }
}