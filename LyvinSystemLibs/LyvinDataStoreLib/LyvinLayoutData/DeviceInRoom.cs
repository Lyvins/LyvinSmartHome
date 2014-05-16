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
//  File            :   Room.cs                                         //
//  Description     :   Represents the room data of a device            //
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
//  ToDo: Commenting                                                    //
//                                                                      //
//----------------------------------------------------------------------//

using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Represents the room data of a device
    /// </summary>
    [TableName("deviceinroom")]
    [PrimaryKey("DeviceInRoomID")]
    public class DeviceInRoom
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong DeviceInRoomID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong RoomID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float Probability { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong DeviceID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DeviceInRoom()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="probability"></param>
        /// <param name="deviceid"></param>
        public DeviceInRoom(ulong roomid, float probability, ulong deviceid)
        {
            RoomID = roomid;
            Probability = probability;
            DeviceID = deviceid;
        }
    }
}