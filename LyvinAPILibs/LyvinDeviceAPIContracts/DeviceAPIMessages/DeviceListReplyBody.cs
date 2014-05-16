﻿//----------------------------------------------------------------------//
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
//  File            :   DeviceListReplyBody.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinDeviceAPIContracts                                     //
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

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LyvinDeviceAPIContracts.DeviceAPIMessages
{
    [DataContract]
    public class DeviceListReplyBody
    {
        [DataMember]
        public DeviceAPIApplication DeviceAPIApplication { get; set; }

        [DataMember]
        public List<DeviceAPIDevice> Devices { get; set; }

        [DataMember]
        public List<DeviceAPIDeviceGroup> DeviceGroups { get; set; }

        [DataMember]
        public List<DeviceAPIDeviceType> DeviceTypes { get; set; }

        [DataMember]
        public List<DeviceAPIDeviceZone> DeviceZones { get; set; }

        [DataMember]
        public string EM_ID { get; set; }

        [DataMember]
        public int Importance { get; set; }

		public DeviceListReplyBody()
        {
            DeviceAPIApplication = new DeviceAPIApplication();
            Devices = new List<DeviceAPIDevice>();
            DeviceGroups = new List<DeviceAPIDeviceGroup>();
            DeviceTypes = new List<DeviceAPIDeviceType>();
            DeviceZones = new List<DeviceAPIDeviceZone>();
		}

        public DeviceListReplyBody(DeviceAPIApplication deviceAPIApplication, List<DeviceAPIDevice> devices, List<DeviceAPIDeviceGroup> devicegroups, List<DeviceAPIDeviceType> devicetypes, List<DeviceAPIDeviceZone> devicezones, string emid, int importance)
        {
            DeviceAPIApplication = deviceAPIApplication;
            Devices = devices;
            DeviceGroups = devicegroups;
            DeviceTypes = devicetypes;
            DeviceZones = devicezones;
            EM_ID = emid;
            Importance = importance;
        }
    }
}
