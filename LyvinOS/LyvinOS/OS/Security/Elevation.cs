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
//  File            :   Elevation.cs                                    //
//  Description     :   Contains elevation item data                    //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   05-07-2013                                       //
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
//  ToDo: Move to Data Store with an appropriate Connector              //
//                                                                      //
//----------------------------------------------------------------------//

using System.Collections.Generic;
using LyvinObjectsLib.Users;

namespace LyvinOS.OS.Security
{
    /// <summary>
    /// Contains elevation item data
    /// </summary>
    public class Elevation
    {

        public Elevation()
        {

        }

        /// 
        /// <param name="elevationID"></param>
        /// <param name="user"></param>
        /// <param name="policies"></param>
        /// <param name="permanent"></param>
        /// <param name="time"></param>
        public Elevation(string elevationID, LyvinUser user, List<Policy> policies, bool permanent, int time)
        {

        }

        public string ElevationID { get; set; }

        public bool Permanent { get; set; }

        public List<Policy> Policies { get; set; }

        public int Time { get; set; }

        public LyvinUser User { get; set; }

    }
}