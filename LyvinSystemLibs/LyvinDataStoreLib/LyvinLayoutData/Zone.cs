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
//  File            :   Zone.cs                                         //
//  Description     :   Represents all zone data and serves as a        //
//                      serializable object for the database and xml    //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   27-3-2014                                       //
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

using System.Collections.Generic;
using LyvinDataStoreLib.LyvinDeviceData.DatabaseHelperObjects;
using LyvinDataStoreLib.Models;
using LyvinSystemLogicLib;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Represents all zone data and serves as a serializable object for the database and xml
    /// </summary>
    [TableName("zone")]
    [PrimaryKey("ZoneID")]
    public class Zone
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong ZoneID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Zone()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneID"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        public Zone(ulong zoneID, string name, string status)
        {
            ZoneID = zoneID;
            Name = name;
            Status = status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceid"></param>
        /// <param name="probability"></param>
        public void AddDevice(ulong deviceid, float probability)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Zone>(ZoneID)) && (lyvinDB.Exists<DatabaseHelperDevice>(deviceid)))
                {
                    var deviceinzone =
                        lyvinDB.SingleOrDefault<DeviceInZone>(
                            "SELECT * FROM deviceinzone WHERE ZoneID=@0 AND DeviceID=@1",
                            ZoneID, deviceid);

                    if (deviceinzone == null)
                        deviceinzone = new DeviceInZone(ZoneID, probability, deviceid);
                    else
                        deviceinzone.Probability = probability;

                    lyvinDB.Save(deviceinzone);
                }
                else
                {
                    ErrorManager.InvokeError("Database Error", "Trying to add deviceinzone item with invalid foreign key");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DeviceInZone> GetDevices()
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                return lyvinDB.Fetch<DeviceInZone>("SELECT * FROM deviceinzone WHERE ZoneID=@0", ZoneID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceid"></param>
        public void RemoveDevice(ulong deviceid)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Zone>(ZoneID)) && (lyvinDB.Exists<DatabaseHelperDevice>(deviceid)))
                {
                    var deviceinzone =
                        lyvinDB.SingleOrDefault<DeviceInZone>(
                            "SELECT * FROM deviceinzone WHERE ZoneID=@0 AND DeviceID=@1",
                            ZoneID, deviceid);

                    if (deviceinzone == null)
                        return;

                    lyvinDB.Delete(deviceinzone);
                }
            }
        }
    }
}
