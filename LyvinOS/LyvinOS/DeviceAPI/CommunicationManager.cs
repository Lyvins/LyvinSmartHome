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
//  File            :   CommunicationManager.cs                         //
//  Description     :   Creates and manages all communication libraries //
//                      Also forwards all sent and received data.       //
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
using System.Linq;
using System.Reflection;
using LyvinDeviceDriverLib;
using LyvinDeviceDriverLib.SystemComInterfaces;
using LyvinSystemLogicLib;

namespace LyvinOS.DeviceAPI
{
    /// <summary>
    /// Creates and manages all communication manager libraries. Also forwards all sent and received data.
    /// </summary>
    public class CommunicationManager : IComManager
    {
        private List<IComProtocol> comProtocols;

        private Dictionary<string, IPhysicalDeviceDriver> physicalDeviceDrivers
        {
            get;
            set;
        }

        private readonly string comDir = "..\\ComProtocols";
        private readonly string comExt = ".dll";

        public CommunicationManager()
        {
            comProtocols = new List<IComProtocol>();
            physicalDeviceDrivers = new Dictionary<string, IPhysicalDeviceDriver>();
        }

        /// <summary>
        /// Gets all ComProtocols from files and puts them in the memory.
        /// </summary>
        public int LoadComProtocols()
        {
            Logger.LogItem("Initializing the communication manager.", LogType.SYSTEM);
            comProtocols.Clear();

            DirectoryInfo di = new DirectoryInfo(comDir);
            if (!di.Exists)
            {
                di.Create();
            }
            FileInfo[] machineFiles = di.GetFiles("*" + comExt);
            foreach (FileInfo fi in machineFiles)
            {
                Assembly asm = default(Assembly);
                try
                {
                    Logger.LogItem("Found a possible communication protocol: " + fi.Name, LogType.DEBUG);
                    asm = Assembly.LoadFrom(fi.FullName);
                    foreach (Type typeAsm in asm.GetTypes())
                    {
                        if ((typeAsm.GetInterface(typeof (IComProtocol).FullName) != null))
                        {
                            Logger.LogItem(
                                "Found communication protocol: " + fi.Name + " (" +
                                typeAsm.GetInterface(typeof (IComProtocol).FullName) + ")", LogType.SYSTEM);

                            object plugObject = Activator.CreateInstance(typeAsm);

                            if (plugObject is IComProtocol)
                            {
                                Logger.LogItem("This library is a valid communication protocol.", LogType.SYSTEM);

                                //Cast this to an IMachineManager interface and add to the collection
                                var plugin = plugObject as IComProtocol;
                                plugin.FileName = fi.Name;
                                plugin.ComManager = this;
                                comProtocols.Add(plugin);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Logger.LogItem(fi.Name + " is not an assembly file.", LogType.DEBUG);
                    return 99;
                }

            }
            return 0;
        }

        /// <summary>
        /// Calls the write method in the corresponding communication protocol.
        /// </summary>
        /// <param name="settings">The dictionary containing all relevant settings which the communication protocol needs to write the data.</param>
        /// <param name="data">The data to be written.</param>
        /// <param name="comProtocol">The communication protocol that should do the write action.</param>
        /// <param name="pdd">The physical device driver to which the replied data should be sent.</param>
        /// <returns>Returns a 0 for success.</returns>
        public int Write(Dictionary<string, string> settings, string data, string comProtocol, IPhysicalDeviceDriver pdd)
        {
            if (!physicalDeviceDrivers.Keys.Contains(pdd.FileName))
            {
                physicalDeviceDrivers.Add(pdd.FileName, pdd);
            }
            else
            {
                physicalDeviceDrivers[pdd.FileName] = pdd;
            }
            try
            {
                Logger.LogItem("Writing data from the pdd \"" + pdd.Type + "\" to the protocol \"" + comProtocol + "\".", LogType.COMPROTOCOL);
                return comProtocols.Single(cp => cp.ComType == comProtocol).Write(settings, data, pdd.FileName);
            }
            catch (Exception)
            {
                Logger.LogItem("The requested communication protocol \""+ comProtocol + "\" does not exist.", LogType.ERROR);
                return 99;
            }
        }

        /// <summary>
        /// Calls the corresponding physical device driver with the received data.
        /// </summary>
        /// <param name="settings">The settings used by the physical device driver when the write/listen method was called.</param>
        /// <param name="data">The data which was received.</param>
        /// <param name="pdd">The id of the physical device driver, sent when the write/listen method was called.</param>
        /// <returns>Returns 0 for success.</returns>
        public int ReceiveData(Dictionary<string ,string> settings, string data, string pddID)
        {
            if (physicalDeviceDrivers.Keys.Contains(pddID))
            {
                Logger.LogItem("Received data for the pdd \"" + pddID + "\".", LogType.COMPROTOCOL);
                return physicalDeviceDrivers[pddID].ReceiveData(settings, data);
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Calls the listening method in the corresponding communication protocol.
        /// </summary>
        /// <param name="settings">The dictionary containing all relevant settings which the communication protocol needs to start listening.</param>
        /// <param name="comProtocol">The communication protocol that should start listening.</param>
        /// <param name="pdd">The physical device driver to which the received data should be sent.</param>
        /// <returns>Returns a 0 for success.</returns>
        public int Listen(Dictionary<string, string> settings, string comProtocol, IPhysicalDeviceDriver pdd)
        {
            if (!physicalDeviceDrivers.Keys.Contains(pdd.FileName))
            {
                physicalDeviceDrivers.Add(pdd.FileName, pdd);
            }
            else
            {
                physicalDeviceDrivers[pdd.FileName] = pdd;
            }
            try
            {
                Logger.LogItem("Starting to listen for the pdd \"" + pdd.Type + "\" on the protocol \"" + comProtocol + "\".", LogType.COMPROTOCOL);
                return comProtocols.Single(cp => cp.ComType == comProtocol).Listen(settings, pdd.FileName);
            }
            catch (Exception)
            {
                Logger.LogItem("The requested communication protocol \"" + comProtocol + "\" does not exist.", LogType.ERROR);
                return 99;
            }
        }

        /// <summary>
        /// Calls the stop listening method in the corresponding communication protocol.
        /// </summary>
        /// <param name="settings">The dictionary containing all relevant settings which the communication protocol needs to stop listening.</param>
        /// <param name="comProtocol">The communication protocol that should stop listening.</param>
        /// <param name="pdd">The physical device driver that started the listening process.</param>
        /// <returns>Returns a 0 for success.</returns>
        public int StopListening(Dictionary<string, string> settings, string comProtocol, IPhysicalDeviceDriver pdd)
        {
            if (!physicalDeviceDrivers.Keys.Contains(pdd.FileName))
            {
                physicalDeviceDrivers.Add(pdd.FileName, pdd);
            }
            else
            {
                physicalDeviceDrivers[pdd.FileName] = pdd;
            }
            try
            {
                Logger.LogItem("Stopping listening for the pdd \"" + pdd.Type + "\" on the protocol \"" + comProtocol + "\".", LogType.COMPROTOCOL);
                return comProtocols.Single(cp => cp.ComType == comProtocol).StopListening(settings, pdd.FileName);
            }
            catch (Exception)
            {
                Logger.LogItem("The requested communication protocol \"" + comProtocol + "\" does not exist.", LogType.ERROR);
                return 99;
            }
        }
    }
}
