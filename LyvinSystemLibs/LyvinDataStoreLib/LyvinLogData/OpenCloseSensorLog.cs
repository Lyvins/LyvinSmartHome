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
//  File            :   OpenCloseSensorDataItem.cs                      //
//  Description     :   Represents an open close sensor data item       //
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
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLogData
{
    /// <summary>
    /// Represents an open close sensor data item
    /// </summary>
    [TableName("openclosesensorlog")]
    [PrimaryKey("OpenCloseSensorLogID")]
    public class OpenCloseSensorLog
    {
        public ulong OpenCloseSensorLogID { get; set; }
        public ulong DeviceID { get; set; }
        public bool State { get; set; }
        public DateTime Triggered { get; set; }

        public OpenCloseSensorLog()
        {
            Triggered = new DateTime();
        }

        public OpenCloseSensorLog(ulong deviceid, DateTime triggered)
        {
            Init(deviceid, triggered, false);
        }

        public OpenCloseSensorLog(ulong deviceid, DateTime triggered, bool state)
        {
            Init(deviceid, triggered, state);
        }
        
        private void Init(ulong deviceid, DateTime triggered, bool state)
        {
            DeviceID = deviceid;
            Triggered = triggered;
            State = state;
        }
    }
}