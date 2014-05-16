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
//  File            :   LyvinOSInputHost.cs                             //
//  Description     :   Receives all API calls from the Event Manager   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   05-07-2013                                       //
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
using LyvinElevationAPIContracts.ElevationAPIMessages;
using LyvinOS.OS;
using LyvinOSAPIContracts;
using LyvinSystemLogicLib;

namespace LyvinOS.SystemAPI
{
    /// <summary>
    /// Receives all API calls from the Event Manager
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class LyvinOSInputHost : ISCLyvinOSInputContract
    {
        private readonly DeviceRequestHandler deviceRequestHandler;
        public LyvinOSOutputProxy OutputProxy { get; set; }

        public LyvinOSInputHost()
        {
            
        }

        public LyvinOSInputHost(DeviceRequestHandler devicerequesthandler)
        {
            deviceRequestHandler = devicerequesthandler;
        }

        public bool HandShake()
        {
            Logger.LogItem(string.Format("Received handshake from Event Manager"), LogType.SYSTEMAPI);
            return true; // outputProxy.HandShake();// true;
        }

        public bool KeepAlive()
        {
            Logger.LogItem(string.Format("Received Keep Alive Ping from Event Manager"), LogType.SYSTEMAPI);
            return true;
        }

        public void DevicePreUpdateRequest(DevicePreUpdateRequest request)
        {
            Logger.LogItem(string.Format("Received Device_Pre_Update_Request from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", request.Header.Request_ID, request.Body.EM_ID), LogType.SYSTEMAPI);
            if (OutputProxy != null)
                OutputProxy.DevicePreUpdateReply(deviceRequestHandler.DevicePreUpdateRequest(request.Body));
            else
            {
                Logger.LogItem("Could not reach LyvinEM.", LogType.SYSTEMAPI);
            }
        }

        public void DeviceUpdateRequest(DeviceUpdateRequest request)
        {
            Logger.LogItem(string.Format("Received Device_Update_Request from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", request.Header.Request_ID, request.Body.EM_ID), LogType.SYSTEMAPI);
            if (OutputProxy != null)
                OutputProxy.DeviceUpdateReply(deviceRequestHandler.DeviceUpdateRequest(request.Body));
        }

        public void ElevationCleanupReply(ElevationCleanupReply reply)
        {
            Logger.LogItem(string.Format("Received Elevation_Cleanup_Reply from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", reply.Header.Request_ID, reply.Body.Elevation_ID), LogType.SYSTEMAPI);
            
            Logger.LogItem(string.Format("Warning: support for {0} has not yet been implemented.", reply.Header.Request_Type), LogType.WARNING);
        }

        public void ElevationReachableReply(ElevationReachableReply reply)
        {
            Logger.LogItem(string.Format("Received Elevation_Reachable_Reply from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", reply.Header.Request_ID, reply.Body.Elevation_ID), LogType.SYSTEMAPI);
            
            Logger.LogItem(string.Format("Warning: support for {0} has not yet been implemented.", reply.Header.Request_Type), LogType.WARNING);
        }

        public void ElevationRequestReply(ElevationRequestReply reply)
        {
            Logger.LogItem(string.Format("Received Elevation_Request_Reply from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", reply.Header.Request_ID, reply.Body.Elevation_ID), LogType.SYSTEMAPI);
            
            Logger.LogItem(string.Format("Warning: support for {0} has not yet been implemented.", reply.Header.Request_Type), LogType.WARNING);
        }

        public void ElevationRequiredReply(ElevationRequiredReply reply)
        {
            Logger.LogItem(string.Format("Received Elevation_Required_Reply from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", reply.Header.Request_ID, reply.Body.Elevation_ID), LogType.SYSTEMAPI);

            Logger.LogItem(string.Format("Warning: support for {0} has not yet been implemented.", reply.Header.Request_Type), LogType.WARNING);
        }

        public void SystemPassRequiredReply(SystemPassRequiredReply reply)
        {
            Logger.LogItem(string.Format("Received System_Pass_Required_Reply from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", reply.Header.Request_ID, reply.Body.Elevation_ID), LogType.SYSTEMAPI);

            Logger.LogItem(string.Format("Warning: support for {0} has not yet been implemented.", reply.Header.Request_Type), LogType.WARNING);
        }

        public void DeviceValueRequest(DeviceValueRequest request)
        {
            Logger.LogItem(string.Format("Received Device_Value_Request from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", request.Header.Request_ID, request.Body.EM_ID), LogType.SYSTEMAPI);
            if (OutputProxy!=null)
            OutputProxy.DeviceValueReply(deviceRequestHandler.DeviceValueRequest(request.Body));
            else
            {
                Logger.LogItem("Could not reach LyvinEM.", LogType.SYSTEMAPI);
            }
        }

        public void DeviceListRequest(DeviceListRequest request)
        {
            Logger.LogItem(string.Format("Received Device_List_Request from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", request.Header.Request_ID, request.Body.EM_ID), LogType.SYSTEMAPI);
            if (OutputProxy != null)
            OutputProxy.DeviceListReply(deviceRequestHandler.DeviceListRequest(request.Body));
            else
            {
                Logger.LogItem("Could not reach LyvinEM.", LogType.SYSTEMAPI);
            }
        }

        public void DeviceZoneListRequest(DeviceZoneListRequest request)
        {
            Logger.LogItem(string.Format("Received Device_Zone_List_Request from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", request.Header.Request_ID, request.Body.EM_ID), LogType.SYSTEMAPI);
            if (OutputProxy != null)
            OutputProxy.DeviceZoneListReply(deviceRequestHandler.DeviceZoneListRequest(request.Body));
            else
            {
                Logger.LogItem("Could not reach LyvinEM.", LogType.SYSTEMAPI);
            }
        }

        public void DeviceGroupListRequest(DeviceGroupListRequest request)
        {
            Logger.LogItem(string.Format("Received Device_Group_List_Request from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", request.Header.Request_ID, request.Body.EM_ID), LogType.SYSTEMAPI);
            if (OutputProxy != null)
            OutputProxy.DeviceGroupListReply(deviceRequestHandler.DeviceGroupListRequest(request.Body));
            else
            {
                Logger.LogItem("Could not reach LyvinEM.", LogType.SYSTEMAPI);
            }
        }

        public void DeviceTypeListRequest(DeviceTypeListRequest request)
        {
            Logger.LogItem(string.Format("Received Device_Type_List_Request from Lyvin OS with Unique SensorID: {0} and EM SensorID: {1}", request.Header.Request_ID, request.Body.EM_ID), LogType.SYSTEMAPI);
            if (OutputProxy != null)
            OutputProxy.DeviceTypeListReply(deviceRequestHandler.DeviceTypeListRequest(request.Body));
            else
            {
                Logger.LogItem("Could not reach LyvinEM.", LogType.SYSTEMAPI);
            }
        }
    }
}