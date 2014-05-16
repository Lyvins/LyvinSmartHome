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
//  File            :   LyvinDevice.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinObjectsLib                                     //
//  Created on      :   17-5-2014                                        //
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
using LyvinDeviceDriverLib;
using LyvinObjectsLib.Events;

namespace LyvinObjectsLib.Devices
{
    // ToDo: Use protected, virtual and override to improve the code (see stackoverflow post)
    // ToDo: Make it so this class can be serialized and deserialized, either by implementing OnSerializing and OnDeserializing method. Or by creating a dummy class with strings representing the zones, groups, etc...

    public class LyvinDevice
    {
        public LyvinDevice()
        {
            DeviceList = new List<LyvinDevice>();
            DeviceGroups = new List<DeviceGroup>();
            DeviceTypes = new List<DeviceType>();
            DeviceZones = new List<DeviceZone>();
            Reachable = true;
            Status = "ON";
        }

        /// <summary>
        /// A constructor for a standard device.
        /// </summary>
        /// <param name="id">The unique id of the device</param>
        /// <param name="name">The name of the device</param>
        /// <param name="description">A description of the device</param>
        /// <param name="deviceList">An optional list of sub devices if the device acts as a hub</param>
        /// <param name="driver">The PDD of the device</param>
        /// <param name="driverFile">The filename of the PDD</param>
        /// <param name="type">The type of the device</param>
        /// <param name="wattage">The wattage of the device</param>
        /// <param name="deviceSettingsFile">The file name of the device settings file</param>
        /// <param name="deviceSettingsType">The object type of the device settings used in (de)serialization</param>
        /// <param name="deviceSettings">The object used by the pdd to store device settings</param>
        /// <param name="reachable">a status indicator indicating whether the device is reachable</param>
        /// <param name="status">The status of a device (ie ON/OFF/AUTO/MANUAL/etc.</param>
        public LyvinDevice(string id, string name, string description, List<LyvinDevice> deviceList,
                           IPhysicalDeviceDriver driver, string driverFile, string type, int wattage,
                           string deviceSettingsFile, Type deviceSettingsType, object deviceSettings, bool reachable,
                           string status)
        {
            ID = id;
            Name = name;
            Description = description;
            DeviceList = deviceList;
            Driver = driver;
            DriverFile = driverFile;
            Type = type;
            Wattage = wattage;
            DeviceSettingsType = deviceSettingsType;
            DeviceSettingsFile = deviceSettingsFile;
            DeviceGroups = new List<DeviceGroup>();
            DeviceTypes = new List<DeviceType>();
            DeviceZones = new List<DeviceZone>();
            DeviceSettings = deviceSettings;
            Reachable = reachable;
            Status = status;
        }

        /// <summary>
        /// A description of the device
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional list of sub devices if the device acts as a hub
        /// </summary>
        public List<LyvinDevice> DeviceList { get; set; }

        /// <summary>
        /// The PDD of the device
        /// </summary>
        public IPhysicalDeviceDriver Driver { get; set; }

        /// <summary>
        /// The filename of the PDD
        /// </summary>
        public string DriverFile { get; set; }

        /// <summary>
        /// The object used by the pdd to store device settings
        /// </summary>
        public object DeviceSettings { get; set; }

        /// <summary>
        /// The filename of the device settings file to which the device settings object will be parsed
        /// </summary>
        public string DeviceSettingsFile { get; set; }

        /// <summary>
        /// The unique ID of the device
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The name of the device
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The wattage of the device
        /// </summary>
        public int Wattage { get; set; }

        /// <summary>
        /// The type of the device
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The object type of the device settings used in (de)serialization
        /// </summary>
        public Type DeviceSettingsType { get; set; }

        /// <summary>
        /// The Device Zones a device is part of.
        /// </summary>
        public List<DeviceZone> DeviceZones { get; set; }

        /// <summary>
        /// The Device groups a device is part of.
        /// </summary>
        public List<DeviceGroup> DeviceGroups { get; set; }

        /// <summary>
        /// The device types a device is part of.
        /// </summary>
        public List<DeviceType> DeviceTypes { get; set; }

        /// <summary>
        /// Whether a device is reachable or not.
        /// </summary>
        public bool Reachable { get; set; }

        /// <summary>
        /// The status of a device (ie ON/OFF/AUTO/MANUAL/etc.
        /// </summary>
        public string Status { get; set; }

        public virtual void ReceiveDeviceEvent(LyvinEvent deviceEvent)
        {
            if (deviceEvent.SourceID == ID)
            {
                switch (deviceEvent.Code)
                {
                    case "DEVICE_STATUS":
                        Status = string.Copy(deviceEvent.Value);
                        break;
                    case "DEVICE_REACHABLE":
                        Reachable = deviceEvent.Value == "True";
                        break;
                    default:
                        break;
                }
            }
        }

        public virtual string GetDeviceValue(string attribute)
        {
            switch (attribute)
            {
                case "STATUS":
                    return Status;
                case "REACHABLE":
                    return Reachable.ToString();
                default:
                    return "";
            }
        }

    }
}