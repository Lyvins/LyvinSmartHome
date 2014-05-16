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
//  File            :   Converter.cs                                    //
//  Description     :   Internal object converter                       //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   18-04-2013                                      //
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
using System.Linq;
using LyvinDeviceDriverLib;
using LyvinObjectsLib.Actions;
using LyvinObjectsLib.Devices;

namespace LyvinObjectsLib
{
    /// <summary>
    /// Internal object Converter
    /// </summary>
    public static class Converter
    {
        public static LyvinDevice ConvertPhysicalDevice(PhysicalDevice source, IPhysicalDeviceDriver pdd, string deviceSettingsFile)
        {
            List<LyvinDevice> devices = null;
            if (source.DeviceList != null)
            {
                devices = source.DeviceList.Select(d => ConvertPhysicalDevice(d, pdd, "Devices\\Settings\\" + pdd.Type + "\\" + CleanFileName(d.ID) + ".xml")).ToList();
            }

            return new LyvinDevice(source.ID, source.Name, source.Description, devices, pdd, pdd.FileName, source.Type, source.Wattage, deviceSettingsFile, source.DeviceSettingsType, source.DeviceSettings, source.Reachable, "ON");
        }

        public static DeviceAction ConvertToDeviceAction(LyvinAction source)
        {
            return new DeviceAction(source.Code, source.TargetType, source.Value, source.TargetID);
        }

        public static string CleanFileName(string filename)
        {
            string file = filename;
            file = string.Concat(file.Split(System.IO.Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));

            if (file.Length > 250)
            {
                file = file.Substring(0, 250);
            }
            return file;
        }
    }
}
