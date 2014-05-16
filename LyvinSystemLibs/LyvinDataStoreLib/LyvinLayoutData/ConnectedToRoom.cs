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
//  File            :   ConnectedToRoom.cs                              //
//  Description     :   Represents the data of a room connected         //
//                      to a certain room                               //
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
//  ToDo:   Commenting                                                  //
//                                                                      //
//----------------------------------------------------------------------//

using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Represents the data of a room connected to a certain room
    /// </summary>
    [TableName("connectedtoroom")]
    [PrimaryKey("ConnectedToRoomID")]
    public class ConnectedToRoom
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong ConnectedToRoomID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong SourceRoomID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong TargetRoomID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float Probability { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ConnectedToRoom()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetroomid"></param>
        /// <param name="probability"></param>
        /// <param name="sourceroomid"></param>
        public ConnectedToRoom(ulong targetroomid, float probability, ulong sourceroomid)
        {
            TargetRoomID = targetroomid;
            Probability = probability;
            SourceRoomID = sourceroomid;
        }
    }
}