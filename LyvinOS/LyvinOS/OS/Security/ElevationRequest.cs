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
//  File            :   ElevationRequest.cs                             //
//  Description     :   Contains elevation request data                 //
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
//  ToDo: Move to OS with the Elevation Manager                         //
//                                                                      //
//----------------------------------------------------------------------//

using System.Collections.Generic;
using LyvinObjectsLib.Actions;
using LyvinObjectsLib.Users;

namespace LyvinOS.OS.Security
{
    /// <summary>
    /// Contains elevation request data
    /// </summary>
    public class ElevationRequest
    {
        public ElevationRequest(string id, LyvinAction action, List<LyvinUser> usersCanGrant, List<Policy> actionPolicies,
                                bool canGrantSelf)
        {
            ElevationID = id;
            Action = action;
            UsersCanGrant = usersCanGrant;
            ActionPolicices = actionPolicies;
            CanGrantSelf = canGrantSelf;
        }

        public string ElevationID { get; set; }

        public LyvinAction Action { get; set; }

        public List<LyvinUser> UsersCanGrant { get; set; }

        public List<Policy> ActionPolicices { get; set; }

        public bool CanGrantSelf { get; set; }
    }
}
