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
//  File            :   DeviceAPIManager.cs                             //
//  Description     :   Creates and manages the device API              //
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


using LyvinDataStoreLib;
using LyvinOS.OS.InternalEventManager;
using LyvinObjectsLib.Devices;

namespace LyvinOS.DeviceAPI
{
    /// <summary>
    /// Creates and manages the device API
    /// </summary>
    public class DeviceAPIManager
    {
        private readonly DeviceManager deviceManager;
        public LogicalDeviceDriver LogicalDeviceDriver;
        private readonly CommunicationManager communicationManager;
        private readonly IEManager ieManager;
        private readonly DeviceDataConnector deviceDataConnector;

        public DeviceAPIManager()
        {

        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="devicemanager"></param>
        /// <param name="iemanager"></param>
        /// <param name="dsManager"></param>
        public DeviceAPIManager(DeviceManager devicemanager, IEManager iemanager, DSManager dsManager)
        {
            ieManager = iemanager;
            communicationManager = new CommunicationManager();
            deviceManager = devicemanager;
            deviceDataConnector = new DeviceDataConnector(dsManager.LogData,dsManager.DeviceData);
            LogicalDeviceDriver = new LogicalDeviceDriver(deviceManager, communicationManager, ieManager, deviceDataConnector);
        }

        /// <summary>
        /// Gets all ComProtocols and Drivers from files and puts them in the memory.
        /// Retrieves all current devices from xml and/or database and checks if they are
        /// still online. Then tries to discover all other physical devices by using the
        /// Drivers and creates a Device object based on their type. These devices will
        /// also be put in xml and/or the database.
        /// </summary>
        public int Initialize()
        {
            LogicalDeviceDriver.Initialize();
            communicationManager.LoadComProtocols();

            return 0;
        }
    }
}