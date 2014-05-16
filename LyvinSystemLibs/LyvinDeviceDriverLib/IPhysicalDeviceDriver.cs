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
//  File            :   IPhysicalDeviceDriver.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinDeviceDriverLib                                     //
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

using System.Collections.Generic;

namespace LyvinDeviceDriverLib
{
    public interface IPhysicalDeviceDriver
    {
        
        /// <summary>
        /// This function always has to be called after creating a new physical device driver, to initialize the required references to the communication manager and the logical device driver.
        /// </summary>
        /// <param name="comManager">A reference to the Communication Manager.</param>
        /// <param name="logicalDeviceDriver">A reference to the Logical Device Driver.</param>
        void Initialize(IComManager comManager, ILogicalDeviceDriver logicalDeviceDriver);

        /// 
        /// <param name="action"></param>
        /// <param name="deviceSettings"></param>
        int DoAction(DeviceAction action, object deviceSettings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        int ReceiveData(Dictionary<string, string> settings, string data);

        /// <summary>
        /// 
        /// </summary>
        List<DeviceAction> Actions { get; set; }

        IComManager ComManager { get; set; }

        ILogicalDeviceDriver LogicalDeviceDriver { get; set; }

        string CommunicationProtocol { get; set; }

        List<DeviceEvent> Events { get; set; }

        List<DevicePolicy> CustomDevicePolicies { get; set; }

        string Type { get; set; }

        string FileName { get; set; }
    }
}