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
//  File            :   OccupancyData.cs                                //
//  Description     :   Represents all Occupancy data                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   26-2-2014                                       //
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
//  -Implemented for 1.5 [AI] Presence                                  //
//  -ToDo: Test                                                         //
//                                                                      //
//----------------------------------------------------------------------//

using System.Collections.Generic;
using System.Linq;
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLogData
{
    /// <summary>
    /// Represents all Occupancy data
    /// </summary>
    public class OccupancyLogData
    {
        private readonly List<OccupancyLog> occupancy;

        public OccupancyLogData()
        {
            occupancy = new List<OccupancyLog>();
        }

        /// <summary>
        /// Adds a new occupancy log item to the data store
        /// </summary>
        /// <param name="item">The occupancy log item to be added.</param>
        public void LogOccupancy(OccupancyLog item)
        {
            DBLogOccupancy(item);
            MemLogOccupancy(item);
        }

        /// <summary>
        /// Adds a new occupancy log item to the database
        /// </summary>
        /// <param name="item">The occupancy log item to be added.</param>
        private void DBLogOccupancy(OccupancyLog item)
        {
            using (var lyvinsDB = new Database("lyvinsdb"))
            {
                lyvinsDB.Insert(item);
            }
        }

        /// <summary>
        /// Adds a new occupancy log item to the internal data store
        /// </summary>
        /// <param name="item">The occupancy log item to be added.</param>
        private void MemLogOccupancy(OccupancyLog item)
        {
            lock (occupancy)
            {
                if (occupancy.Exists(o => o.RoomID == item.RoomID))
                {
                    var items = occupancy.Where(o => o.RoomID == item.RoomID);
                    foreach (var occupancyLogItem in items)
                    {
                        occupancy.Remove(occupancyLogItem);
                    }
                }
                occupancy.Add(item);
            }
        }

        /// <summary>
        /// Gets the last occupancy log item of a certain room from the data store
        /// </summary>
        /// <param name="roomid">The id of the room</param>
        /// <returns>The last occupancy log item of the room</returns>
        public OccupancyLog GetLastOccupancy(ulong roomid)
        {
            return occupancy.Where(o => o.RoomID == roomid).OrderByDescending(o => o.TimeStamp).SingleOrDefault();
        }
    }
}