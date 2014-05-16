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
//  File            :   Occupancy.cs                                    //
//  Description     :   Represents an occupancy data item               //
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
//                                                                      //
//----------------------------------------------------------------------//

using System;
using LyvinDataStoreLib.LyvinLayoutData;
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLogData
{
    /// <summary>
    /// Represents an occupancy data item 
    /// </summary>
    [TableName("occupancylog")]
    [PrimaryKey("OccupancyLogID")]
    public class OccupancyLog
    {
        public ulong OccupancyLogID { get; set; }
        public int People{ get; set; }
        public float ProbabilityAbsolute { get; set; }
        public string ProbabilityRelative { get; set; }
        public ulong RoomID { get; set; }
        public DateTime TimeStamp { get; set; }

        [Ignore] public Room Room { get; set; }

        public OccupancyLog()
        {
            Room = new Room();
            TimeStamp = new DateTime();
        }

        public OccupancyLog(Room room, string probabilityrelative, float probabilityabsolute, int people,
                                DateTime timestamp)
        {
            Room = room;
            RoomID = room.RoomID;
            ProbabilityRelative = probabilityrelative;
            ProbabilityAbsolute = probabilityabsolute;
            People = people;
            TimeStamp = timestamp;
        }
    }
}