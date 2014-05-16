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
//  File            :   DimmableDevice.cs                                //
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
    public class DimmableDevice : LyvinDevice
    {
        /// <summary>
        /// The constructor of a dimmable device
        /// </summary>
        /// <param name="id">The unique id of the device</param>
        /// <param name="name">The name of the device</param>
        /// <param name="description">A description of the device</param>
        /// <param name="deviceList">An optional list of sub devices if the device acts as a hub</param>
        /// <param name="driver">The PDD of the device</param>
        /// <param name="driverFile">The filename of the PDD</param>
        /// <param name="type">The type of the device</param>
        /// <param name="wattage">The wattage of the device</param>
        /// <param name="deviceSettingsFile">The filename of the settings file of the device</param>
        /// <param name="deviceSettingsType">The object type of the device settings used in (de)serialization</param>
        /// <param name="deviceSettings">The object used by the pdd to store device settings</param>
        /// <param name="dimmable">A boolean indicating whether the device is dimmable</param>
        /// <param name="brightness">The brightness/intensity of a dimmable device (0-100)</param>
        /// <param name="status">The latest status of the device (ON/OFF/etc)</param>
        /// <param name="reachable"> </param>
        public DimmableDevice(string id, string name, string description, List<LyvinDevice> deviceList,
                              IPhysicalDeviceDriver driver, string driverFile, string type, int wattage,
                              string deviceSettingsFile, Type deviceSettingsType, object deviceSettings, bool dimmable,
                              int brightness, string status, bool reachable) :
                                  base(
                                  id, name, description, deviceList, driver, driverFile, type, wattage,
                                  deviceSettingsFile, deviceSettingsType, deviceSettings, reachable, status)
        {
            Dimmable = dimmable;
            Intensity = brightness;
        }

        /// <summary>
        /// A constructor of a dimmable device
        /// </summary>
        /// <param name="d">The dimmable device</param>
        public DimmableDevice(LyvinDevice d)
            : base(
                d.ID, d.Name, d.Description, d.DeviceList, d.Driver, d.DriverFile, d.Type, d.Wattage,
                d.DeviceSettingsFile, d.DeviceSettingsType, d.DeviceSettings, d.Reachable, d.Status)
        {
            Dimmable = true;
            Intensity = 0;
        }

        public DimmableDevice()
            : base()
        {

        }

        /// <summary>
        /// A boolean indicating whether the device is dimmable
        /// </summary>
        public bool Dimmable { get; set; }

        /// <summary>
        /// The brigtness/intensity of the dimmable device (0-100)
        /// </summary>
        public int Intensity { get; set; }

        /// <summary>
        /// The status of the device (ON/OFF/etc)
        /// </summary>
        public string Status { get; set; }

        public void ReceiveDeviceEvent(LyvinEvent deviceEvent)
        {
            if (deviceEvent.SourceID == ID)
            {
                switch (deviceEvent.Code)
                {
                    case "DEVICE_STATUS":
                        base.ReceiveDeviceEvent(deviceEvent);
                        break;
                    case "DEVICE_BRIGHTNESS":
                        try
                        {
                            Intensity = Int32.Parse(deviceEvent.Value);
                        }
                        catch (Exception)
                        {
                            //ToDo: Error handling
                        }
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
                case "INTENSITY":
                    return Intensity.ToString();
                case "REACHABLE":
                    return Reachable.ToString();
                default:
                    return "";
            }
        }

    }
}