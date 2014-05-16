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
//  File            :   DeviceRawDataLog.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinDataStoreLib                                     //
//  Created on      :   16-5-2014                                        //
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


using System;
using System.Collections.Generic;
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLogData
{
    public class DeviceRawDataLog
    {
        private readonly List<DeviceRawDataItem> deviceRawData;

        public DeviceRawDataLog()
        {
            deviceRawData = new List<DeviceRawDataItem>();
        }

        /// <summary>
        /// Adds a device's raw data item to the data store
        /// </summary>
        /// <param name="deviceRawDataItem">The raw data item to be added</param>
        public void LogRawDeviceData(DeviceRawDataItem deviceRawDataItem)
        {
            if (!DBLogRawDeviceData(deviceRawDataItem))
                MemLogRawDeviceData(deviceRawDataItem);
        }

        /// <summary>
        /// Adds a device's raw data item to the database
        /// </summary>
        /// <param name="deviceRawDataItem">The raw data item to be added</param>
        private bool DBLogRawDeviceData(DeviceRawDataItem deviceRawDataItem)
        {
            try
            {
                using (var lyvinsDb = new Database("lyvinsdb"))
                {
                    if (deviceRawData.Count > 0)
                    {
                        foreach (var rawDataItem in deviceRawData)
                        {
                            lyvinsDb.Insert(rawDataItem);
                        }
                        deviceRawData.Clear();
                    }

                    lyvinsDb.Insert(deviceRawDataItem);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a device's raw data item to the internal data store
        /// </summary>
        /// <param name="deviceRawDataItem">The raw data item to be added</param>
        private void MemLogRawDeviceData(DeviceRawDataItem deviceRawDataItem)
        {
            lock (deviceRawData)
            {
                deviceRawData.Add(deviceRawDataItem);
            }
        }
    }
}
