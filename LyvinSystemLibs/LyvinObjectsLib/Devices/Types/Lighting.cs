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
//  File            :   Lighting.cs                                //
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
    // ToDo: Use protected, virtual and override to improve the code (see stackoverflow post)

    public class Lighting : LyvinDevice
    {
        /// <summary>
        /// A constructor for a lighting device
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
        /// <param name="dimmable">A boolean indicating whether the device is dimmable</param>
        /// <param name="brightness">The brightness/intensity of the dimmable device</param>
        /// <param name="status">The latest status of the device (ON/OFF/etc)</param>
        /// <param name="reachable"> </param>
        public Lighting(string id, string name, string description, List<LyvinDevice> deviceList,
                        IPhysicalDeviceDriver driver, string driverFile, string type, int wattage,
                        string deviceSettingsFile, Type deviceSettingsType, object deviceSettings, bool dimmable,
                        int brightness, string status, bool reachable) :
                            base(
                            id, name, description, deviceList, driver, driverFile, type, wattage, deviceSettingsFile,
                            deviceSettingsType, deviceSettings, reachable, status)
        {
            Dimmable = dimmable;
            Brightness = brightness;
        }

        /// <summary>
        /// A constructor for a lighting device
        /// </summary>
        /// <param name="d">The lighting device</param>
        public Lighting(LyvinDevice d)
            : base(
                d.ID, d.Name, d.Description, d.DeviceList, d.Driver, d.DriverFile, d.Type, d.Wattage,
                d.DeviceSettingsFile, d.DeviceSettingsType, d.DeviceSettings, d.Reachable, d.Status)
        {
            Dimmable = true;
            Brightness = 0;
        }

        public Lighting()
            : base()
        {

        }

        /// <summary>
        /// A boolean indicating whether the device is dimmable
        /// </summary>
        public bool Dimmable { get; set; }

        /// <summary>
        /// The brightness/intensity of the dimmable device (0-100)
        /// </summary>
        public int Brightness { get; set; }

        public override void ReceiveDeviceEvent(LyvinEvent deviceEvent)
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
                            Brightness = Int32.Parse(deviceEvent.Value);
                        }
                        catch (Exception)
                        {
                            //ToDo: Some Error handling
                        }
                        break;
                    case "DEVICE_EFFECT":
                        //ToDo: Implement effect if necessary
                        break;
                    case "DEVICE_TRANSITION":
                        //ToDo: Implement transition if necessary
                        break;
                    case "DEVICE_REACHABLE":
                        base.ReceiveDeviceEvent(deviceEvent);
                        break;
                    default:
                        break;
                }
            }
        }

        public override string GetDeviceValue(string attribute)
        {
            switch (attribute)
            {
                case "STATUS":
                    return Status;
                case "BRIGHTNESS":
                    return Brightness.ToString();
                case "EFFECT":
                    return "";
                case "TRANSITION":
                    return "";
                case "REACHABLE":
                    return Reachable.ToString();
                default:
                    return "";
            }
        }

    }
}