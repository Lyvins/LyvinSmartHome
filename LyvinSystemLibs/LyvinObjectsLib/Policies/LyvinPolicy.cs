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
//  File            :   LyvinPolicy.cs                                //
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

namespace LyvinObjectsLib.Policies
{
    public class LyvinPolicy
    {

        public LyvinPolicy()
        {

        }

        public LyvinPolicy(string id, string name, string description, string objectID, string objectType,
                           string policyType, string actionType)
        {

        }

        public string Description { get; set; }

        public string PolicyID { get; set; }

        public string PolicyName { get; set; }

        public string PolicyObjectID { get; set; }

        /// <summary>
        /// <ul>
        /// 	<li>Individual devices</li>
        /// 	<li>Zones</li>
        /// 	<li>Groups</li>
        /// 	<li>Device Types</li>
        /// 	<li>Interfaces</li>
        /// 	<li>Apps</li>
        /// 	<li>Widgets</li>
        /// </ul>
        /// </summary>
        public string PolicyObjectType { get; set; }

        /// <summary>
        /// <ul>
        /// 	<li>Global</li>
        /// 	<li>UserGroup</li>
        /// 	<li>User</li>
        /// 	<li>WidgetDevice</li>
        /// </ul>
        /// </summary>
        public string PolicyType { get; set; }

        /// <summary>
        /// Can be "Read" or "Read/Change" depending on the denied action.
        /// </summary>
        public string ActionType { get; set; }

    }
}