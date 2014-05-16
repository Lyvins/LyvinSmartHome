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
//  File            :   OSManager.cs                                    //
//  Description     :   Creates and manages all OS functionality,       //
//                      including the Internal Event Manager,           //
//                      The Artificial Intelligence, and the Data Store.//
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   26-2-2014                                       //
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

using System.Threading;
using LyvinDataStoreLib;
using LyvinOS.DeviceAPI;
using LyvinOS.OS.ArtificialIntelligence;
using LyvinOS.OS.InternalEventManager;
using LyvinOS.OS.Security;
using LyvinOS.SystemAPI;
using LyvinObjectsLib.Actions;
using LyvinObjectsLib.Devices;

namespace LyvinOS.OS
{
    /// <summary>
    /// Creates and manages all OS functionality, including the Internal Event Manager,
    /// The Artificial Intelligence, and the Data Store.
    /// </summary>
    public class OSManager
    {
        private AIManager aiManager;
        public IEManager IEManager;
        private DSManager dsManager;
        private DeviceAPIManager deviceAPIManager;
        private SystemAPIManager systemAPI;
        private DeviceManager deviceManager;
        private SecurityManager securityManager;
        private DeviceRequestHandler deviceRequestHandler;

        private LogicalDeviceDriver logicalDeviceDriver;

        public OSManager()
        {
            deviceManager = new DeviceManager();
            securityManager = new SecurityManager();
            deviceRequestHandler = new DeviceRequestHandler(deviceManager);
            IEManager = new IEManager();
            aiManager = new AIManager();
            dsManager = new DSManager();
            deviceAPIManager = new DeviceAPIManager(deviceManager,IEManager, dsManager);
            systemAPI = new SystemAPIManager(securityManager, deviceRequestHandler);

            aiManager.Initialize(dsManager, IEManager);
            IEManager.Initialize(dsManager, systemAPI, deviceManager);

            logicalDeviceDriver = deviceAPIManager.LogicalDeviceDriver;
            //            appstub = new AppStub(this, deviceManager);
            deviceAPIManager.Initialize();
            
            //            SecurityManager = new SecurityManager(this, actionManager, applicationManager, userManager, deviceManager,
            //                                                  widgetDeviceManager, systemAPIManager);
            
                        DiscoverDevices();
                        Thread.Sleep(2500);
            //            appstub.Initialize();

        }

        public void DiscoverDevices()
        {
            DoAction(new LyvinAction("DISCOVER", "System", "System", "ALL", "ALL", ""));
        }

        public int DoAction(LyvinAction action)
        {
            /*if (action.SourceType != "System")
            {
                switch (SecurityManager.SecurityCheck(action))
                {
                    case 0:
                        return 0;
                    case 1:
                        return DoCheckedAction(action);
                    case 2:
                        actionManager.AddAction(action);
                        return 2;
                }
            }*/
            return DoCheckedAction(action);
        }

        private int DoCheckedAction(LyvinAction action)
        {
            switch (action.TargetType)
            {
                case "ALL":
                    logicalDeviceDriver.DoAction(action);
                    return 0;
                default:
                    logicalDeviceDriver.DoAction(action);
                    return 0;
            }
        }
    }
}
