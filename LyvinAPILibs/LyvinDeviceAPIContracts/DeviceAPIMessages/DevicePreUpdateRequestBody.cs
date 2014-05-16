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
//  File            :   DevicePreUpdateRequestBody.cs                                //
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
    public class DevicePreUpdateRequestBody
    {

        [DataMember]
        public DeviceAPIApplication DeviceAPIApplication { get; set; }

        [DataMember]
        public DeviceAPIDevice Device { get; set; }

        [DataMember]
        public DeviceAPIDeviceGroup DeviceGroup { get; set; }

        [DataMember]
        public string DeviceType { get; set; }

        [DataMember]
        public List<DeviceAPIAttribute> DeviceTypeAttributes { get; set; }

        [DataMember]
        public bool Direct_Control { get; set; }

        [DataMember]
        public string EM_ID { get; set; }

        [DataMember]
        public int Priority { get; set; }

        public DevicePreUpdateRequestBody()
        {
            DeviceAPIApplication = new DeviceAPIApplication();
            Device = new DeviceAPIDevice();
            DeviceGroup = new DeviceAPIDeviceGroup();
            DeviceTypeAttributes = new List<DeviceAPIAttribute>();
        }

        public DevicePreUpdateRequestBody(DeviceAPIApplication deviceAPIApplication, DeviceAPIDevice device,
                                          DeviceAPIDeviceGroup deviceGroup, string deviceType,
                                          List<DeviceAPIAttribute> deviceTypeAttributes, bool directControl, string emID,
                                          int priority)
        {
            DeviceAPIApplication = deviceAPIApplication;
            Device = device;
            DeviceGroup = deviceGroup;
            DeviceType = deviceType;
            DeviceTypeAttributes = deviceTypeAttributes;
            Direct_Control = directControl;
            EM_ID = emID;
            Priority = priority;
        }
    }
}