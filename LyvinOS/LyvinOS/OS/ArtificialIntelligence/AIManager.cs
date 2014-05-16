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
//  File            :   AIManager.cs                                    //
//  Description     :   Creates and manages all AI managers             //
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LyvinAILib;
using LyvinDataStoreLib;
using LyvinOS.OS.InternalEventManager;
using LyvinSystemLogicLib;

namespace LyvinOS.OS.ArtificialIntelligence
{
    /// <summary>
    /// Creates and manages all AI managers
    /// </summary>
    public class AIManager
    {
        private List<IAIModule> AIModules;
        private DSManager dsManager;
        private IEManager ieManager;

        private const string AIDir = "..\\AI";         //ToDo: Retreive the directory and extension from the configuration
        private const string AIExt = ".dll";

        /// <summary>
        /// 
        /// </summary>
        public AIManager()
        {
            AIModules = new List<IAIModule>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsmanager"></param>
        /// <param name="iemanager"></param>
        public void Initialize(DSManager dsmanager, IEManager iemanager)
        {
            Logger.LogItem("Initializing the AI manager.", LogType.SYSTEM);
            dsManager = dsmanager;
            ieManager = iemanager;
            LoadAIModules();
        }

        /// <summary>
        /// Load all AI Modules from dll.
        /// </summary>
        public int LoadAIModules()
        {
            AIModules.Clear();

            var di = new DirectoryInfo(AIDir);
            if (!di.Exists)
            {
                di.Create();
            }
            var machineFiles = di.GetFiles("*" + AIExt);
            foreach (var fi in machineFiles)
            {
                try
                {
                    Logger.LogItem("Found a possible AI Module: " + fi.Name, LogType.DEBUG);
                    var asm = Assembly.LoadFrom(fi.FullName);
                    foreach (var typeAsm in asm.GetTypes().Where(typeAsm => (typeAsm.GetInterface(typeof(IAIModule).FullName) != null)))
                    {
                        Logger.LogItem(
                            "Found device driver: " + fi.Name + " (" +
                            typeAsm.GetInterface(typeof(IAIModule).FullName) + ")", LogType.SYSTEM);

                        var plugObject = Activator.CreateInstance(typeAsm);

                        if (!(plugObject is IAIModule)) continue;
                        Logger.LogItem("This library is a valid AI Module.", LogType.SYSTEM);

                        //Cast this to an IPhysicalDeviceDriver interface and add to the collection
                        var plugin = plugObject as IAIModule;
                        plugin.Initialize(dsManager, ieManager);
                        AIModules.Add(plugin);
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