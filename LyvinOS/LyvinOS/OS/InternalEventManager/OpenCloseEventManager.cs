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
//  File            :   OpenCloseEventManager.cs                        //
//  Description     :   Handles all Internal Device Events              //
//                      from Open Close Sensors                         //
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

using LyvinAILib;
using LyvinAILib.InternalEventMessages;
using LyvinDataStoreLib;

namespace LyvinOS.OS.InternalEventManager
{
    /// <summary>
    /// Handles all Internal Device Events from Open Close Sensors
    /// </summary>
    public class OpenCloseEventManager
    {

        private OpenCloseEventDataConnector dataConnector;
        private IIEManager ieManager;

        /// <summary>
        /// 
        /// </summary>
        public OpenCloseEventManager()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iemanager"></param>
        /// <param name="dsmanager"></param>
        public void Initialize(IIEManager iemanager, DSManager dsmanager)
        {
            dataConnector = new OpenCloseEventDataConnector(dsmanager.DeviceData, dsmanager.LogData);
            iemanager.IE50DeviceEvent +=new System.EventHandler<InternalEventArgs<IE50DeviceEvent>>(SensorTriggered);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SensorTriggered(object sender, InternalEventArgs<IE50DeviceEvent> e)
        {
            var deviceEvent = e.InternalEvent;

            if (deviceEvent.DeviceType.DeviceTypeID == "OPEN_CLOSE_SENSOR")
            {
                // ToDo: handle the event and create AI event and create EM event
            }
        }
    }
}