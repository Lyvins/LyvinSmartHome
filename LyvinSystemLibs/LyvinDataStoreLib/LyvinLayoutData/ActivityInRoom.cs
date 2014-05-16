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
//  File            :   ActivityInRoom.cs                               //
//  Description     :   Represents the data for an activity             //
//                      in a certain room                               //
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
    /// Represents the data for an activity in a certain room
    /// </summary>
    [TableName("activityinroom")]
    [PrimaryKey("ActivityInRoomID")]
    public class ActivityInRoom
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong ActivityInRoomID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong ActivityID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Manual { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float Probability { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong RoomID { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public ActivityInRoom()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityid"></param>
        /// <param name="roomid"></param>
        /// <param name="manual"></param>
        /// <param name="probability"></param>
        public ActivityInRoom(ulong activityid, ulong roomid, bool manual, float probability)
        {
            ActivityID = activityid;
            RoomID = roomid;
            Manual = manual;
            Probability = probability;
        }
    }
}