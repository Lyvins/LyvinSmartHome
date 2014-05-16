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
//  File            :   Sensor.cs                                //
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

namespace LyvinObjectsLib.Devices.Types
{
    public class Sensor : LyvinDevice
    {
        /// <summary>
        /// A constructor for a sensor device
        /// </summary>
        /// <param name="id">The unique id of the device</param>
        /// <param name="name">The name of the device</param>
        /// <param name="description">A description of the device</param>
        /// <param name="deviceList">An optional list of sub devices</param>
        /// <param name="driver">The PDD of the device</param>
        /// <param name="driverFile">The filename of the PDD of the device</param>
        /// <param name="type">The type of the device</param>
        /// <param name="wattage">The wattage of the device</param>
        /// <param name="deviceSettingsFile">The filename of the settings file of the device</param>
        /// <param name="deviceSettingsType">The object type of the device settings used in (de)serialization</param>
        /// <param name="deviceSettings">The object used by the pdd to store device settings</param>
        /// <param name="status">The latest status of the device (ON/OFF/etc)</param>
        /// <param name="sensorvalue">The sensors current value</param>
        /// <param name="reachable"> </param>
        public Sensor(string id, string name, string description, List<LyvinDevice> deviceList, IPhysicalDeviceDriver driver, string driverFile, string type, int wattage, string deviceSettingsFile, Type deviceSettingsType, object deviceSettings, string status, string sensorvalue, bool reachable) : 
            base(id, name, description, deviceList, driver, driverFile, type, wattage, deviceSettingsFile, deviceSettingsType, deviceSettings, reachable, status)
	    {
	        Value = sensorvalue;
	    }

        /// <summary>
        /// A constructor for a lighting device
        /// </summary>
        /// <param name="d">The lighting device</param>
        public Sensor(LyvinDevice d)
            : base(d.ID, d.Name, d.Description, d.DeviceList, d.Driver, d.DriverFile, d.Type, d.Wattage, d.DeviceSettingsFile, d.DeviceSettingsType, d.DeviceSettings, d.Reachable, d.Status)
        {
            Value = "0";
        }
        
        public Sensor()
            : base()
	    {
	        
	    }

        /// <summary>
        /// The value of the sensor
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceEvent"></param>
        public void ReceiveDeviceEvent(LyvinEvent deviceEvent)
        {
            if (deviceEvent.SourceID == ID)
            {
                switch (deviceEvent.Code)
                {
                    case "DEVICE_STATUS":
                        base.ReceiveDeviceEvent(deviceEvent);
                        break;
                    case "SENSOR_TEMP":
                        Value = deviceEvent.Value;
                        break;
                    case "SENSOR_INTENSITY":
                        Value = deviceEvent.Value;
                        break;
                    case "SENSOR_MOTION":
                        Value = deviceEvent.Value;
                        break;
                    case "SENSOR_REACHABLE":
                        base.ReceiveDeviceEvent(deviceEvent);
                        break;
                    default:
                        break;
                }
            }
        }

        public string GetDeviceValue(string attribute)
        {
            switch (attribute)
            {
                case "STATUS":
                    return Status;
                case "TEMP":
                    return Value;
                case "INTENSITY":
                    return Value;
                case "MOTION":
                    return Value;
                case "REACHABLE":
                    return Reachable.ToString();
                default:
                    return "";
            }
        }
    }
}
