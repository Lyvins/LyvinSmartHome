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
//  File            :   OpenCloseEventDataConnector.cs                  //
//  Description     :   The interface between the data store and the    //
//                      OpenCloseEventManager                           //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   27-2-2014                                       //
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
using LyvinAILib.InternalEventMessages;
using LyvinDataStoreLib.LyvinDeviceData;
using LyvinDataStoreLib.LyvinLogData;

namespace LyvinOS.OS.InternalEventManager
{
    /// <summary>
    /// The interface between the data store and the OpenCloseEventManager
    /// </summary>
    public class OpenCloseEventDataConnector
    {
        private DeviceData deviceData;
        private LogData logData;

        public OpenCloseEventDataConnector(DeviceData deviceData, LogData logData)
        {
            this.deviceData = deviceData;
            this.logData = logData;
        }

        public IE50DeviceEvent GetPreviousOpenCloseEvent()
        {

            return null;
        }

        public void StoreOpenCloseEvent(IE50DeviceEvent openCloseEvent)
        {

        }

        public bool GetDeviceTimeOut(string deviceID)
        {

            return false;
        }

        public OpenCloseSensor GetOpenCloseSensorDevice(String deviceID)
        {

            return null;
        }
    }
}
