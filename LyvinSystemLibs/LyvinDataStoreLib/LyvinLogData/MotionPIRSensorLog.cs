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
//  File            :   MotionPIR.cs                                    //
//  Description     :   Represents a data item of a motion PIR sensor   //
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
    /// Represents a data item of a motion PIR sensor
    /// </summary>
    [TableName("motionpirsensorlog")]
    [PrimaryKey("MotionPIRSensorLogID")]
    public class MotionPIRSensorLog
    {
        public ulong MotionPIRSensorLogID { get; set; }
        public ulong DeviceID{ get; set; }
        public int People { get; set; }
        public int RepeatPattern { get; set; }
        public DateTime Triggered { get; set; }

        public MotionPIRSensorLog()
        {
            Triggered = new DateTime();
        }

        public MotionPIRSensorLog(ulong deviceid, DateTime triggered, int people, int repeatpattern)
        {
            Init(deviceid, triggered, people, repeatpattern);
        }

        public MotionPIRSensorLog(ulong deviceid, DateTime triggered)
        {
            Init(deviceid, triggered, 0, 0);
        }

        private void Init(ulong deviceid, DateTime triggered, int people, int repeatpattern)
        {
            DeviceID = deviceid;
            People = people;
            RepeatPattern = repeatpattern;
            Triggered = triggered;
        }
    }
}