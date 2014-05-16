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
//  File            :   ISCDeviceReplyAPI.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinDeviceAPIContracts                                     //
//  Created on      :   16-5-2014                                        //
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

using System.ServiceModel;
using LyvinDeviceAPIContracts.DeviceAPIMessages;

namespace LyvinDeviceAPIContracts
{
    /// <summary>
    /// This contract should be implemented by the Event Manager, applications, and the
    /// DeviceAPIApplication manager (maybe also the widget manager and widgets) in order to
    /// receive replies on the device requests from the system.
    /// </summary>
    [ServiceContract]
    public interface ISCDeviceReplyAPI
    {

        /// 
        /// <param name="reply"></param>
        [OperationContract(IsOneWay = true)]
        void DeviceValueReply(DeviceValueReply reply);

        /// 
        /// <param name="reply"></param>
        [OperationContract(IsOneWay = true)]
        void DeviceListReply(DeviceListReply reply);

        /// 
        /// <param name="request"></param>
        [OperationContract(IsOneWay = true)]
        void DeviceZoneListReply(DeviceZoneListReply reply);

        /// 
        /// <param name="request"></param>
        [OperationContract(IsOneWay = true)]
        void DeviceGroupListReply(DeviceGroupListReply reply);

        /// 
        /// <param name="request"></param>
        [OperationContract(IsOneWay = true)]
        void DeviceTypeListReply(DeviceTypeListReply reply);

    }
}