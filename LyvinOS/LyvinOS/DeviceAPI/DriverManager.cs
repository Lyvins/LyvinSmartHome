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
//  File            :   DriverManager.cs                                //
//  Description     :   Loads and manages all driver libraries.         //
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
using System.IO;
using System.Reflection;
using LyvinDeviceDriverLib;
using LyvinSystemLogicLib;

namespace LyvinOS.DeviceAPI
{
    /// <summary>
    /// Loads and manages all driver libraries.
    /// </summary>
    public class DriverManager
    {
        private readonly string pddDir = "..\\PDD";
        private readonly string pddExt = ".dll";

        ~DriverManager()
        {

        }

        public virtual void Dispose()
        {

        }

        public DriverManager()
        {
            DeviceDrivers = new List<IPhysicalDeviceDriver>();
        }

        public List<IPhysicalDeviceDriver> DeviceDrivers { get; set; }

        /// <summary>
        /// Load all drivers from dll.
        /// </summary>
        public int LoadDrivers(LogicalDeviceDriver ldd, CommunicationManager communicationManager)
        {
            //AppDomain test = AppDomain.CreateDomain("test");

            Logger.LogItem("Initializing the driver manager.", LogType.SYSTEM);
            DeviceDrivers.Clear();

            DirectoryInfo di = new DirectoryInfo(pddDir);
            if (!di.Exists)
            {
                di.Create();
            }
            FileInfo[] machineFiles = di.GetFiles("*" + pddExt);
            foreach (FileInfo fi in machineFiles)
            {
                Assembly asm = default(Assembly);
                try
                {
                    Logger.LogItem("Found a possible device driver: " + fi.Name, LogType.DEBUG);
                    asm = Assembly.LoadFrom(fi.FullName);
                    foreach (Type typeAsm in asm.GetTypes())
                    {
                        if ((typeAsm.GetInterface(typeof (IPhysicalDeviceDriver).FullName) != null))
                        {
                            Logger.LogItem(
                                "Found device driver: " + fi.Name + " (" +
                                typeAsm.GetInterface(typeof (IPhysicalDeviceDriver).FullName) + ")", LogType.SYSTEM);

                            object plugObject = Activator.CreateInstance(typeAsm);

                            if (plugObject is IPhysicalDeviceDriver)
                            {
                                Logger.LogItem("This library is a valid device driver.", LogType.SYSTEM);

                                //Cast this to an IPhysicalDeviceDriver interface and add to the collection
                                var plugin = plugObject as IPhysicalDeviceDriver;
                                plugin.FileName = fi.Name;
                                plugin.ComManager = communicationManager;
                                plugin.LogicalDeviceDriver = ldd;
                                DeviceDrivers.Add(plugin);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Logger.LogItem(fi.Name + " is not an assembly file.", LogType.DEBUG);
                }
            }
            return 0;
        }
    }
}