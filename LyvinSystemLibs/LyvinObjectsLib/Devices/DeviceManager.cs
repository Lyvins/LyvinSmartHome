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
//  File            :   DeviceManager.cs                                //
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
using System.IO;
using System.Linq;
using LyvinObjectsLib.Devices.Types;
using LyvinObjectsLib.Events;
using LyvinSystemLogicLib;

namespace LyvinObjectsLib.Devices
{
    public class DeviceManager
    {
        private const string deviceTypeFile = "System\\DeviceTypes.xml";

        /// <summary>
        /// The standard constructor for the device manager
        /// </summary>
        public DeviceManager()
        {
            DeviceGroups = new List<DeviceGroup>();
            Devices = new List<LyvinDevice>();
            DeviceTypes = new List<DeviceType>();
            DeviceZones = new List<DeviceZone>();
            Initialize();
        }

        /// <summary>
        /// Returns a certain device from a list
        /// </summary>
        /// <param name="list">The list containing all the devices</param>
        /// <param name="id">The ID of the device to be returned</param>
        /// <returns>Returns the device with the unique device ID. Returns null if the list does not contain the device.</returns>
        private LyvinDevice FindDevice(List<LyvinDevice> list, string id)
        {
            LyvinDevice temp = null;
            foreach (var device in list)
            {
                if (device.ID == id)
                {
                    temp = device;
                }
                else if (device.DeviceList != null)
                {
                    temp = FindDevice(device.DeviceList, id);
                }
                if (temp != null)
                {
                    return temp;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a certain device
        /// </summary>
        /// <param name="id">The unique ID of the device to be returned</param>
        /// <returns>The device with the unique device ID. Returns null if the device does not exist.</returns>
        public LyvinDevice GetDevice(string id)
        {
            return FindDevice(Devices, id);
        }

        /// <summary>
        /// Returns a certain device group
        /// </summary>
        /// <param name="id">The unique id of the device group to be returned</param>
        /// <returns>Returns the device group containing the unique id</returns>
        public DeviceGroup GetDeviceGroup(string id)
        {
            return DeviceGroups.Single(dg => dg.ID == id);
        }

        /// <summary>
        /// Returns a certain device zone
        /// </summary>
        /// <param name="id">The unique id of the device zone to be returned</param>
        /// <returns>Returns the device zone containing the unique id</returns>
        public DeviceZone GetDeviceZone(string id)
        {
            try
            {
                return DeviceZones.Single(dz => dz.ID == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a certain device type
        /// </summary>
        /// <param name="id">The unique ID of the device type to be returned</param>
        /// <returns>Returns the device type containing the unique ID</returns>
        public DeviceType GetDeviceType(string id)
        {
            return DeviceTypes.Single(dt => dt.ID == id);
        }

        /// <summary>
        /// List of all device groups
        /// </summary>
        public List<DeviceGroup> DeviceGroups { get; set; }

        /// <summary>
        /// List of all devices
        /// </summary>
        public List<LyvinDevice> Devices { get; set; }

        /// <summary>
        /// List of all device types
        /// </summary>
        public List<DeviceType> DeviceTypes { get; set; }

        /// <summary>
        /// List of all device zones
        /// </summary>
        public List<DeviceZone> DeviceZones { get; set; }

        /// <summary>
        /// Returns a list of all devices contained in a device, including the device itself
        /// </summary>
        /// <param name="device">The device containing the sub devices</param>
        /// <returns>Returns a list of all devices contained in a device, including the device itself</returns>
        public List<LyvinDevice> GetAllSubDevices(LyvinDevice device)
        {
            List<LyvinDevice> temp = new List<LyvinDevice>();
            temp.Add(device);
            if (device.DeviceList != null)
            {
                foreach (var d in device.DeviceList)
                {
                    temp.AddRange(GetAllSubDevices(d));
                }
            }
            return temp;
        }

        /// <summary>
        /// Initializes a device, analyzing its type and converting it to the proper devicetype. Also adds it to the proper device type list.
        /// </summary>
        /// <param name="device">The device to be initialized</param>
        /// <returns>The initialized device</returns>
        private LyvinDevice InitializeDevice(LyvinDevice device)
        {
            LyvinDevice temp;
            switch (device.Type)
            {
                case "HUB":
                    temp = new LyvinDevice(device.ID, device.Name, device.Description, new List<LyvinDevice>(),
                                           device.Driver, device.DriverFile, device.Type, device.Wattage,
                                           device.DeviceSettingsFile, device.DeviceSettingsType, device.DeviceSettings,
                                           true, "ON");
                    GetDeviceType(device.Type).Devices.Add(temp);
                    break;
                case "LIGHT":
                    temp = new Lighting(device);
                    GetDeviceType(device.Type).Devices.Add(temp);
                    break;
                case "COLORED_LIGHT":
                    temp = new ColoredLighting(device);
                    GetDeviceType(device.Type).Devices.Add(temp);
                    break;
                case "DIMMABLE_DEVICE":
                    temp = new DimmableDevice(device);
                    GetDeviceType(device.Type).Devices.Add(temp);
                    break;
                case "MOTION_SENSOR":
                    temp = new Sensor(device);
                    GetDeviceType(device.Type).Devices.Add(temp);
                    break;
                case "TEMPERATURE_SENSOR":
                case "LIGHT_SENSOR":
                    temp = new Sensor(device);
                    GetDeviceType(device.Type).Devices.Add(temp);
                    break;
                default:
                    temp = new LyvinDevice(device.ID, device.Name, device.Description, new List<LyvinDevice>(),
                                           device.Driver, device.DriverFile, device.Type, device.Wattage,
                                           device.DeviceSettingsFile, device.DeviceSettingsType, device.DeviceSettings,
                                           true, "ON");
                    break;
            }
            temp.DeviceList = new List<LyvinDevice>();
            if (device.DeviceList != null)
            {
                foreach (var d in device.DeviceList)
                {
                    temp.DeviceList.Add(InitializeDevice(d));
                }
            }
            return temp;
        }

        /// <summary>
        /// Initializes and adds a device
        /// </summary>
        /// <param name="device">The device to be added</param>
        /// <returns>Returns 1 if successful, otherwise 0</returns>
        public int AddDevice(LyvinDevice device)
        {
            if (device != null)
            {
                Devices.Add(InitializeDevice(device));
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Initializes the device types and loads all Devices from xml and/or database (saving and loading not yet implemented)
        /// </summary>
        /// <returns>Returns 1 if successful, otherwise 0</returns>
        public int Initialize()
        {
            int ret = 1;
            if (InitializeDeviceTypes() == 0)
            {
                ret = 0;
            }
            //ToDo: Add code for saving and loading devices, groups, etc to and from xml and or databases.
            return ret;
        }

        /// <summary>
        /// Initializes the device types from xml/database. Or creates a list of standard device types if the file does not exist.
        /// </summary>
        /// <returns>Returns 1 if successful, otherwise 0.</returns>
        private int InitializeDeviceTypes()
        {
            DeviceTypes.Clear();

            if (File.Exists(deviceTypeFile))
            {
                DeviceTypes = XMLParser.GetObjectsFromFile<DeviceType>(deviceTypeFile, "", false, true);
                /*foreach (var o in l)
                {
                    DeviceTypes.Add((DeviceType)o);
                }*/
                return 1;
            }
            else
            {
                return CreateDeviceTypes();
            }
        }

        /// <summary>
        /// Creates a list of standard device types and writes them to xml/database.
        /// </summary>
        /// <returns>Returns 1 if successful, otherwise 0.</returns>
        private int CreateDeviceTypes()
        {
            DeviceTypes.Add(new DeviceType("HUB", "Hub", "A hub connected to several devices."));
            DeviceTypes.Add(new DeviceType("LIGHT", "Standard Lighting", "Standard Lighting."));
            DeviceTypes.Add(new DeviceType("COLORED_LIGHT", "Colored Lighting", "Colored Lighting."));
            DeviceTypes.Add(new DeviceType("DIMMABLE_DEVICE", "Dimmable Device", "Dimmable Device."));
            DeviceTypes.Add(new DeviceType("MOTION_SENSOR", "Motion Sensor", "Motion Sensor."));
            DeviceTypes.Add(new DeviceType("TEMPERATURE_SENSOR", "Temperature Sensor", "Temperature Sensor."));
            DeviceTypes.Add(new DeviceType("LIGHT_SENSOR", "Light Sensor", "Light Sensor."));

            XMLParser.WriteXMLFromObjects<DeviceType>(deviceTypeFile, "", DeviceTypes, false, true);
            return 1;
        }

        /// <summary>
        /// Sends the device event to the specific device class, which, if necessary, will update the values in that device class.
        /// </summary>
        /// <param name="deviceEvent">the device event to be handled.</param>
        public void ReceiveDeviceEvent(LyvinEvent deviceEvent)
        {
            try
            {
                var device = GetDevice(deviceEvent.SourceID);
                if (device != null)
                {
                    switch (device.Type)
                    {
                        case "LIGHT":
                            var lightdevice = device as Lighting;
                            if (lightdevice != null)
                            {
                                lightdevice.ReceiveDeviceEvent(deviceEvent);
                            }
                            break;
                        case "COLORED_LIGHT":
                            var coloreddevice = device as ColoredLighting;
                            if (coloreddevice != null)
                            {
                                coloreddevice.ReceiveDeviceEvent(deviceEvent);
                            }
                            break;
                        case "DIMMABLE_DEVICE":
                            var dimmabledevice = device as DimmableDevice;
                            if (dimmabledevice != null)
                            {
                                dimmabledevice.ReceiveDeviceEvent(deviceEvent);
                            }
                            break;
                        case "MOTION_SENSOR":
                            var motionsensor = device as Sensor;
                            if (motionsensor != null)
                            {
                                motionsensor.ReceiveDeviceEvent(deviceEvent);
                            }
                            break;
                        case "TEMPERATURE_SENSOR":
                            var temperaturesensor = device as Sensor;
                            if (temperaturesensor != null)
                            {
                                temperaturesensor.ReceiveDeviceEvent(deviceEvent);
                            }
                            break;
                        case "LIGHT_SENSOR":
                            var lightsensor = device as Sensor;
                            if (lightsensor != null)
                            {
                                lightsensor.ReceiveDeviceEvent(deviceEvent);
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
                //ToDo: Error handling
            }
        }

        public string GetDeviceValue(string deviceID, string attribute)
        {
            try
            {
                var device = GetDevice(deviceID);
                if (device != null)
                {
                    switch (device.Type)
                    {
                        case "LIGHT":
                            var lightdevice = device as Lighting;
                            if (lightdevice != null)
                            {
                                return lightdevice.GetDeviceValue(attribute);
                            }
                            break;
                        case "COLORED_LIGHT":
                            var coloreddevice = device as ColoredLighting;
                            if (coloreddevice != null)
                            {
                                return coloreddevice.GetDeviceValue(attribute);
                            }
                            break;
                        case "DIMMABLE_DEVICE":
                            var dimmabledevice = device as DimmableDevice;
                            if (dimmabledevice != null)
                            {
                                return dimmabledevice.GetDeviceValue(attribute);
                            }
                            break;
                        case "MOTION_SENSOR":
                            var motionsensor = device as Sensor;
                            if (motionsensor != null)
                            {
                                return motionsensor.GetDeviceValue(attribute);
                            }
                            break;
                        case "TEMPERATURE_SENSOR":
                            var temperaturesensor = device as Sensor;
                            if (temperaturesensor != null)
                            {
                                return temperaturesensor.GetDeviceValue(attribute);
                            }
                            break;
                        case "LIGHT_SENSOR":
                            var lightsensor = device as Sensor;
                            if (lightsensor != null)
                            {
                                return lightsensor.GetDeviceValue(attribute);
                            }
                            break;
                        default:
                            return device.GetDeviceValue(attribute);
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                //ToDo: Error handling
            }
            return "";
        }

    }
}