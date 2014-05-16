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
//  File            :   DeviceChangedEventBody.cs                                //
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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LyvinDeviceAPIContracts.DeviceAPIMessages
{
    [DataContract]
    public class DeviceChangedEventBody
    {

        [DataMember]
        public string ChangeType { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public DeviceAPIDevice Device { get; set; }

        [DataMember]
        public DeviceAPIDeviceGroup DeviceGroup { get; set; }

        [DataMember]
        public string DeviceType { get; set; }

        [DataMember]
        public List<DeviceAPIAttribute> DeviceTypeAttributes { get; set; }

        [DataMember]
        public string EM_ID { get; set; }

        public DeviceChangedEventBody()
        {
            DateTime = DateTime.Now;
            Device = new DeviceAPIDevice();
            DeviceGroup = new DeviceAPIDeviceGroup();
            DeviceTypeAttributes = new List<DeviceAPIAttribute>();
        }

        public DeviceChangedEventBody(string changetype, DateTime datetime, DeviceAPIDevice device,
                                      DeviceAPIDeviceGroup devicegroup, string devicetype,
                                      List<DeviceAPIAttribute> devicetypeattributes, string emid)
        {
            ChangeType = changetype;
            DateTime = datetime;
            Device = device;
            DeviceGroup = devicegroup;
            DeviceType = devicetype;
            DeviceTypeAttributes = devicetypeattributes;
            EM_ID = emid;
        }

    }
}