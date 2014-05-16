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
//  File            :   IComManager.cs                                //
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
    public interface IComManager
    {
        /// <summary>
        /// Calls the write method in the corresponding communication protocol.
        /// </summary>
        /// <param name="settings">The dictionary containing all relevant settings which the communication protocol needs to write the data.</param>
        /// <param name="data">The data to be written.</param>
        /// <param name="comProtocol">The communication protocol that should do the write action.</param>
        /// <param name="pdd">The physical device driver to which the replied data should be sent.</param>
        /// <returns>Returns a 0 for success.</returns>
        int Write(Dictionary<string, string> settings, string data, string comProtocol, IPhysicalDeviceDriver pdd);

        /// <summary>
        /// Calls the corresponding physical device driver with the received data.
        /// </summary>
        /// <param name="settings">The settings used by the physical device driver when the write/listen method was called.</param>
        /// <param name="data">The data which was received.</param>
        /// <param name="pdd">The id of the physical device driver, sent when the write/listen method was called.</param>
        /// <returns>Returns 0 for success.</returns>
        int ReceiveData(Dictionary<string, string> settings, string data, string pdd);

        /// <summary>
        /// Calls the listening method in the corresponding communication protocol.
        /// </summary>
        /// <param name="settings">The dictionary containing all relevant settings which the communication protocol needs to start listening.</param>
        /// <param name="comProtocol">The communication protocol that should start listening.</param>
        /// <param name="pdd">The physical device driver to which the received data should be sent.</param>
        /// <returns>Returns a 0 for success.</returns>
        int Listen(Dictionary<string, string> settings, string comProtocol, IPhysicalDeviceDriver pdd);

        /// <summary>
        /// Calls the stop listening method in the corresponding communication protocol.
        /// </summary>
        /// <param name="settings">The dictionary containing all relevant settings which the communication protocol needs to stop listening.</param>
        /// <param name="comProtocol">The communication protocol that should stop listening.</param>
        /// <param name="pdd">The physical device driver that started the listening process.</param>
        /// <returns>Returns a 0 for success.</returns>
        int StopListening(Dictionary<string, string> settings, string comProtocol, IPhysicalDeviceDriver pdd);
    }
}