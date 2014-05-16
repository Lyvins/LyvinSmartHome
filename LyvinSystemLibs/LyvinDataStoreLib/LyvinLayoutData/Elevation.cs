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
//  File            :   ElevationData.cs                                //
//  Description     :   Represents elevation data                       //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   7-3-2014                                        //
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
//  ToDo:   Commenting                                                  //
//                                                                      //
//----------------------------------------------------------------------//


using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Represents elevation data
    /// </summary>
    [TableName("elevation")]
    [PrimaryKey("ElevationID")]
    public class Elevation
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong ElevationID { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong RoomID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Elevation()
        {
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="roomid"></param>
        public Elevation(int floor, ulong roomid)
        {
            Floor = floor;
            RoomID = roomid;
        }
    }
}
