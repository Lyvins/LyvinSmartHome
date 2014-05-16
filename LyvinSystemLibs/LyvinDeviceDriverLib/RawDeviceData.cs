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
//  File            :   RawDeviceData.cs                                //
//  Description     :   Represents a device's raw data                  //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinDeviceDriverLib                            //
//  Created on      :   12-5-2014                                       //
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

namespace LyvinDeviceDriverLib
{
    /// <summary>
    /// Represents a device's raw data
    /// </summary>
    public class RawDeviceData
    {
        /// <summary>
        /// 
        /// </summary>
        public RawDeviceData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driverID"></param>
        /// <param name="deviceData"></param>
        public RawDeviceData(string driverID, string deviceData)
        {
            DriverID = driverID;
            DeviceData = deviceData;
        }

        /// <summary>
        /// Unique Identifier used by the driver to recognize the device
        /// </summary>
        public string DriverID { get; set; }

        /// <summary>
        /// The value of the raw device device data
        /// </summary>
        public string DeviceData { get; set; }
    }
}
