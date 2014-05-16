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
//  File            :   OpenCloseSensor.cs                              //
//  Description     :   Represents the data of an open close sensor.    //
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
//----------------------------------------------------------------------//

using LyvinDataStoreLib.LyvinDeviceData.DatabaseHelperObjects;
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinDeviceData
{
    /// <summary>
    /// Represents the data of an open close sensor.
    /// </summary>
    [TableName("openclosesensor")]
    [PrimaryKey("OpenCloseSensorID")]
    public class OpenCloseSensor
    {
        public ulong OpenCloseSensorID { get; set; }
        public ulong DeviceID { get; set; }
        public string Type { get; set; }

        [Ignore] public DatabaseHelperDevice DatabaseHelperDevice { get; set; }
        
        public OpenCloseSensor()
        {
            
        }

        public OpenCloseSensor(string type, DatabaseHelperDevice databaseHelperDevice)
        {
            DatabaseHelperDevice = databaseHelperDevice;
            DeviceID = databaseHelperDevice.DeviceID;
            Type = type;
        }
    }
}