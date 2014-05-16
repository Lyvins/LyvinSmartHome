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
//  File            :   ElevationManager.cs                             //
//  Description     :   Contains all elevation data                     //
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
//  ToDo: Move to OS and move all elevations into the Data Store        //
//                                                                      //
//----------------------------------------------------------------------//

using System.Collections.Generic;
using System.Linq;

namespace LyvinOS.OS.Security
{
    /// <summary>
    /// Contains all elevation data
    /// </summary>
    public class ElevationManager
    {

        private readonly List<Elevation> elevations;

        public ElevationManager()
        {
            elevations = new List<Elevation>();
        }

        /// 
        /// <param name="elevation"></param>
        public void AddElevation(Elevation elevation)
        {
            if (!elevations.Contains(elevation))
            {
                elevations.Add(elevation);
            }
        }

        public List<Elevation> ListElevations()
        {
            return elevations;
        }

        /// 
        /// <param name="userID"></param>
        public List<Elevation> ListUserElevations(string userID)
        {
            return elevations.Where(e => e.User.UserID == userID).ToList();
        }

        /// 
        /// <param name="elevationID"></param>
        public void RemoveElevation(string elevationID)
        {
            if (elevations.Exists(e => e.ElevationID == elevationID))
            {
                elevations.Remove(elevations.Single(e => e.ElevationID == elevationID));
            }
        }

        public bool CheckCurrentElevation(string userID, Policy policy)
        {
            return
                elevations.Where(e => e.User.UserID == userID)
                          .SelectMany(el => el.Policies)
                          .Any(p => p.PolicyID == policy.PolicyID);
        }
    }
}