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
using LyvinObjectsLib.Users;

namespace LyvinObjectsLib.Policies
{
    public class PolicyManager
    {

        private List<LyvinPolicy> globalPolicies { get; set; }

        private List<LyvinUserGroupPolicy> userGroupPolicies { get; set; }

        private List<LyvinUserPolicy> userPolicies { get; set; }

        private UserManager userManager;

        public PolicyManager(UserManager userManager)
        {

        }

        /// 
        /// <param name="policy"></param>
        public void AddGlobalPolicy(LyvinPolicy policy)
        {

        }

        /// 
        /// <param name="policy"></param>
        /// <param name="userGroup"></param>
        public void AddUserGroupPolicy(LyvinUserGroupPolicy policy, LyvinUserGroup userGroup)
        {

        }

        /// 
        /// <param name="policy"></param>
        /// <param name="user"></param>
        public void AddUserPolicy(LyvinUserPolicy policy, LyvinUser user)
        {

        }

        public List<LyvinPolicy> GetGlobalPolicies()
        {

            return null;
        }

        /// 
        /// <param name="UserGroupID"></param>
        public List<LyvinPolicy> GetUserGroupPolicies(string UserGroupID)
        {

            return null;
        }

        /// 
        /// <param name="UserID"></param>
        public List<LyvinPolicy> GetUserPolicies(string UserID)
        {

            return null;
        }

        /// 
        /// <param name="PolicyID"></param>
        public void RemovePolicy(string PolicyID)
        {

        }

        /// 
        /// <param name="userList"></param>
        /// <param name="policy"></param>
        public List<LyvinUser> UsersCanGrantPolicy(List<LyvinUser> userList, LyvinPolicy policy)
        {

            return null;
        }

        /// 
        /// <param name="userList"></param>
        /// <param name="policy"></param>
        public List<LyvinUser> UsersCanGrantPolicies(List<LyvinPolicy> policies)
        {
            List<LyvinUser> usersCanGrantPolicies = new List<LyvinUser>();
            LyvinPolicy grantAll = new LyvinPolicy("CAN_GRANT_ALL_POLICIES", "Grant All",
                                                   "This user can grant all policies", "All_Policies", "Policy", "", "");
            LyvinPolicy grantOwn = new LyvinPolicy("CAN_GRANT_OWN_POLICIES", "Grant Own",
                                                   "This user can grant his own policies", "Own_Policies", "Policy", "",
                                                   "");
            bool denyOnAll = false;
            bool denyOnOwn = false;
            List<LyvinUserGroup> userGroupsDenyOnAll = new List<LyvinUserGroup>();
            List<LyvinUserGroup> userGroupsDenyOnOwn = new List<LyvinUserGroup>();

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

        public bool CheckGlobalPolicy(LyvinPolicy policy)
        {
            return globalPolicies.Any(gp => gp.PolicyID == policy.PolicyID);
        }

        public bool CheckUserPolicy(LyvinPolicy policy, string userID)
        {
            return userPolicies.Any(up => ((up.Policy.PolicyID == policy.PolicyID) && (up.User.UserID == userID)));
        }

        public bool CheckUserGroupPolicy(LyvinPolicy policy, string userGroupID)
        {
            return
                userGroupPolicies.Any(
                    ugp => ((ugp.Policy.PolicyID == policy.PolicyID) && (ugp.UserGroup.UserGroupID == userGroupID)));
        }

    }
}