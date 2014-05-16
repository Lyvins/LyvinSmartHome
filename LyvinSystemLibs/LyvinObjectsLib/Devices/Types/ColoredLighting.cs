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
//  File            :   ColoredLighting.cs                                //
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

    public class ColoredLighting : Lighting
    {
        /// <summary>
        /// A constructor for a Colored Lighting Device.
        /// </summary>
        /// <param name="id">The unique id of the device</param>
        /// <param name="name">The name of the device</param>
        /// <param name="description">A description of the device</param>
        /// <param name="deviceList">An optional container containing all sub devices if this device acts as a hub</param>
        /// <param name="driver">The PDD of the device</param>
        /// <param name="driverFile">The file name of the PDD</param>
        /// <param name="type">The type of the device</param>
        /// <param name="wattage">The wattage of the device</param>
        /// <param name="deviceSettingsFile">The filename of the device settings file</param>
        /// <param name="deviceSettingsType">The object type of the device settings, used to (de)serialize</param>
        /// <param name="deviceSettings">The object used by the pdd to store device settings</param>
        /// <param name="dimmable">A boolean indicating whether the device is dimmable</param>
        /// <param name="brightness">The brightness of the light</param>
        /// <param name="status">The current status of the device (ON, OFF, etc)</param>
        /// <param name="reachable"> </param>
        public ColoredLighting(string id, string name, string description, List<LyvinDevice> deviceList,
                               IPhysicalDeviceDriver driver, string driverFile, string type, int wattage,
                               string deviceSettingsFile, Type deviceSettingsType, object deviceSettings, bool dimmable,
                               int brightness, string status, bool reachable) :
                                   base(
                                   id, name, description, deviceList, driver, driverFile, type, wattage,
                                   deviceSettingsFile, deviceSettingsType, deviceSettings, dimmable, brightness, status,
                                   reachable)
        {
            RGB = "0,0,0";
            Hue = 0;
            Sat = 0;
        }

        /// <summary>
        /// A constructor for a Colored Lighting Device.
        /// </summary>
        /// <param name="device">The device which is a colored lighting device</param>
        public ColoredLighting(LyvinDevice device) :
            base(device)
        {
            RGB = "0,0,0";
            Hue = 0;
            Sat = 0;
        }

        public ColoredLighting()
            : base()
        {

        }

        /// <summary>
        /// The RGB value for the colored light. Using the format 255:255:255 (0-255)
        /// </summary>
        public string RGB { get; set; }

        /// <summary>
        /// The Hue value for the colored light (0-3600)
        /// </summary>
        public int Hue { get; set; }

        /// <summary>
        /// The Saturation value for the colored light (0-100)
        /// </summary>
        public int Sat { get; set; }

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
                        base.ReceiveDeviceEvent(deviceEvent);
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
                    case "DEVICE_RGB":
                        RGB = string.Copy(deviceEvent.Value);
                        break;
                    case "DEVICE_HUE":
                        try
                        {
                            Hue = Int32.Parse(deviceEvent.Value);
                        }
                        catch (Exception)
                        {
                            //ToDo: Error handling
                        }
                        break;
                    case "DEVICE_SATURATION":
                        try
                        {
                            Sat = Int32.Parse(deviceEvent.Value);
                        }
                        catch (Exception)
                        {
                            //ToDo: Error Handling
                        }
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
                case "RGB":
                    return RGB;
                case "HUE":
                    return Hue.ToString();
                case "SATURATION":
                    return Sat.ToString();
                default:
                    return "";
            }
        }

    }
}