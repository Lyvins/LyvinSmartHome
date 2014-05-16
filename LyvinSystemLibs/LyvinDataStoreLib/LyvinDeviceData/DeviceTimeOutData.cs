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
//  File            :   DeviceTimeOutData.cs                            //
//  Description     :   Contains all device time out data               //
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
//                                                                      //
//----------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.Linq;
using LyvinDataStoreLib.LyvinDeviceData.DatabaseHelperObjects;
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinDeviceData
{
    /// <summary>
    /// Contains all device time out data
    /// </summary>
    public class DeviceTimeOutData
    {
        public List<DeviceTimeOut> DeviceTimeOuts { get; set; }

        public DeviceTimeOutData()
        {
            DeviceTimeOuts = new List<DeviceTimeOut>();
        }

        public bool GetDeviceTimeOut(ulong deviceid)
        {
            if (DeviceTimeOuts.Exists(dt => dt.DeviceID == deviceid))
            {
                var devicetimeout = DeviceTimeOuts.Single(dt => dt.DeviceID == deviceid);
                if (devicetimeout.TimeStamp.AddSeconds(devicetimeout.SecondsTimeOut) < DateTime.Now)
                {
                    return true;
                }
                DeviceTimeOuts.Remove(devicetimeout);
                return false;
            }
            return false;
        }

        public void AddDeviceTimeOut(ulong deviceid, int seconds)
        {
            DeviceTimeOuts.RemoveAll(dt => dt.DeviceID == deviceid);
            var timeout = new DeviceTimeOut(deviceid, DateTime.Now, seconds);
            using (var lyvinsdb = new Database("lyvinsdb"))
            {
                lyvinsdb.Insert(timeout);
            }
            DeviceTimeOuts.Add(timeout);
        }

        public void RemoveDeviceTimeOut(ulong deviceid)
        {
            DeviceTimeOuts.RemoveAll(dt => dt.DeviceID == deviceid);
        }

        /// <summary>
        /// Used to initialize device timeouts for that particular device
        /// </summary>
        /// <param name="device">The discovered device</param>
        public void DeviceDiscovered(DatabaseHelperDevice device)
        {
            // For now device time outs are considered elapsed when the device is newly discovered or the system restarts.
        }
    }
}
