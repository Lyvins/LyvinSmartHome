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
//  File            :   LyvinOSOutputProxy.cs                           //
//  Description     :   Handles all outgoing API communication to the   //
//                      Event Manager                                   //
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

using System;
using System.Collections.Generic;
using System.ServiceModel;
using LyvinDeviceAPIContracts.DeviceAPIMessages;
using LyvinElevationAPIContracts.ElevationAPIMessages;
using LyvinOSAPIContracts;
using LyvinSystemLogicLib;

namespace LyvinOS.SystemAPI
{
    /// <summary>
    /// Handles all outgoing API communication to the Event Manager
    /// </summary>
    public class LyvinOSOutputProxy
    {
        private ISCLyvinOSOutputContract outputChannel;
        private readonly ChannelFactory<ISCLyvinOSOutputContract> channelFactory;

        private const string CurrentRequestVersion = "0.1";

        private readonly Queue<DevicePreUpdateReplyBody> devicePreUpdateReplyQueue;
        private readonly Queue<DeviceUpdateReplyBody> deviceUpdateReplyQueue;
        private readonly Queue<ElevationCleanupRequestBody> elevationCleanupRequestQueue;
        private readonly Queue<ElevationReachableRequestBody> elevationReachableRequestQueue;
        private readonly Queue<ElevationRequestRequestBody> elevationRequestRequestQueue;
        private readonly Queue<ElevationRequiredRequestBody> elevationRequiredRequestQueue;
        private readonly Queue<SystemPassRequiredRequestBody> systemPassRequiredRequestQueue;
        private readonly Queue<DeviceValueReplyBody> deviceValueReplyQueue;
        private readonly Queue<DeviceListReplyBody> deviceListReplyQueue;
        private readonly Queue<DeviceZoneListReplyBody> deviceZoneListReplyQueue;
        private readonly Queue<DeviceGroupListReplyBody> deviceGroupListReplyQueue;
        private readonly Queue<DeviceTypeListReplyBody> deviceTypeListReplyQueue;
        private readonly Queue<DeviceChangedEventBody> deviceChangedEventQueue;
        private readonly Queue<DevicePreUpdateEventBody> devicePreUpdateEventQueue;

        public LyvinOSOutputProxy()
        {
            channelFactory = new ChannelFactory<ISCLyvinOSOutputContract>("outputChannel");

            devicePreUpdateReplyQueue = new Queue<DevicePreUpdateReplyBody>();
            deviceUpdateReplyQueue = new Queue<DeviceUpdateReplyBody>();
            elevationCleanupRequestQueue = new Queue<ElevationCleanupRequestBody>();
            elevationReachableRequestQueue = new Queue<ElevationReachableRequestBody>();
            elevationRequestRequestQueue = new Queue<ElevationRequestRequestBody>();
            elevationRequiredRequestQueue = new Queue<ElevationRequiredRequestBody>();
            systemPassRequiredRequestQueue = new Queue<SystemPassRequiredRequestBody>();
            deviceValueReplyQueue = new Queue<DeviceValueReplyBody>();
            deviceListReplyQueue = new Queue<DeviceListReplyBody>();
            deviceZoneListReplyQueue = new Queue<DeviceZoneListReplyBody>();
            deviceGroupListReplyQueue = new Queue<DeviceGroupListReplyBody>();
            deviceTypeListReplyQueue = new Queue<DeviceTypeListReplyBody>();
            deviceChangedEventQueue = new Queue<DeviceChangedEventBody>();
            devicePreUpdateEventQueue = new Queue<DevicePreUpdateEventBody>();
        }

        public string GetClientAddress()
        {
            return channelFactory.Endpoint.Address.ToString();
        }

        public bool CLoseClient()
        {
            if (channelFactory.State == CommunicationState.Opened)
            {
                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();
                outputChannel = null;
                return true;
            }
            return false;
        }

        public void SendQueuedRequests()
        {
            lock (devicePreUpdateReplyQueue)
            {
                while (devicePreUpdateReplyQueue.Count > 0)
                {
                    DevicePreUpdateReply(devicePreUpdateReplyQueue.Dequeue());
                }
            }

            lock (deviceUpdateReplyQueue)
            {
                while (deviceUpdateReplyQueue.Count > 0)
                {
                    DeviceUpdateReply(deviceUpdateReplyQueue.Dequeue());
                }
            }

            lock (elevationCleanupRequestQueue)
            {
                while (elevationCleanupRequestQueue.Count > 0)
                {
                    ElevationCleanupRequest(elevationCleanupRequestQueue.Dequeue());
                }
            }

            lock (elevationReachableRequestQueue)
            {
                while (elevationReachableRequestQueue.Count > 0)
                {
                    ElevationReachableRequest(elevationReachableRequestQueue.Dequeue());
                }
            }

            lock (elevationRequestRequestQueue)
            {
                while (elevationRequestRequestQueue.Count > 0)
                {
                    ElevationRequestRequest(elevationRequestRequestQueue.Dequeue());
                }
            }

            lock (elevationRequiredRequestQueue)
            {
                while (elevationRequiredRequestQueue.Count > 0)
                {
                    ElevationRequiredRequest(elevationRequiredRequestQueue.Dequeue());
                }
            }

            lock (systemPassRequiredRequestQueue)
            {
                while (systemPassRequiredRequestQueue.Count > 0)
                {
                    SystemPassRequiredRequest(systemPassRequiredRequestQueue.Dequeue());
                }
            }

            lock (deviceValueReplyQueue)
            {
                while (deviceValueReplyQueue.Count > 0)
                {
                    DeviceValueReply(deviceValueReplyQueue.Dequeue());
                }
            }

            lock (deviceListReplyQueue)
            {
                while (deviceListReplyQueue.Count > 0)
                {
                    DeviceListReply(deviceListReplyQueue.Dequeue());
                }
            }

            lock (deviceZoneListReplyQueue)
            {
                while (deviceZoneListReplyQueue.Count > 0)
                {
                    DeviceZoneListReply(deviceZoneListReplyQueue.Dequeue());
                }
            }

            lock (deviceGroupListReplyQueue)
            {
                while (deviceGroupListReplyQueue.Count > 0)
                {
                    DeviceGroupListReply(deviceGroupListReplyQueue.Dequeue());
                }
            }

            lock (deviceTypeListReplyQueue)
            {
                while (deviceTypeListReplyQueue.Count > 0)
                {
                    DeviceTypeListReply(deviceTypeListReplyQueue.Dequeue());
                }
            }

            lock (deviceChangedEventQueue)
            {
                while (deviceChangedEventQueue.Count > 0)
                {
                    DeviceChangedEvent(deviceChangedEventQueue.Dequeue());
                }
            }

            lock (devicePreUpdateEventQueue)
            {
                while (devicePreUpdateEventQueue.Count > 0)
                {
                    DevicePreUpdateEvent(devicePreUpdateEventQueue.Dequeue());
                }
            }
        }

        public void ClearQueuedRequests()
        {
            lock (devicePreUpdateReplyQueue)
            {
                devicePreUpdateReplyQueue.Clear();
            }

            lock (deviceUpdateReplyQueue)
            {
                deviceUpdateReplyQueue.Clear();
            }

            lock (elevationCleanupRequestQueue)
            {
                elevationCleanupRequestQueue.Clear();
            }

            lock (elevationReachableRequestQueue)
            {
                elevationReachableRequestQueue.Clear();
            }

            lock (elevationRequestRequestQueue)
            {
                elevationRequestRequestQueue.Clear();
            }

            lock (elevationRequiredRequestQueue)
            {
                elevationRequiredRequestQueue.Clear();
            }

            lock (systemPassRequiredRequestQueue)
            {
                systemPassRequiredRequestQueue.Clear();
            }

            lock (deviceValueReplyQueue)
            {
                deviceValueReplyQueue.Clear();
            }

            lock (deviceListReplyQueue)
            {
                deviceListReplyQueue.Clear();
            }

            lock (deviceZoneListReplyQueue)
            {
                deviceZoneListReplyQueue.Clear();
            }

            lock (deviceGroupListReplyQueue)
            {
                deviceGroupListReplyQueue.Clear();
            }

            lock (deviceTypeListReplyQueue)
            {
                deviceTypeListReplyQueue.Clear();
            }
        }

        public bool HandShake()
        {
            Logger.LogItem(string.Format("Creating channel to Event Manager"), LogType.SYSTEMAPI);
            outputChannel = channelFactory.CreateChannel();
            try
            {
                Logger.LogItem(string.Format("Sending handshake to Event Manager"), LogType.SYSTEMAPI);
                return outputChannel.HandShake();
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending HandShake to Event Manager"), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();
            }
            return false;
        }

        public bool KeepAlive()
        {
            try
            {
                Logger.LogItem(string.Format("Sending ping to Event Manager"), LogType.SYSTEMAPI);
                return outputChannel.KeepAlive();
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending ping to Event Manager"), LogType.ERROR);
                
                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();
            }
            return false;
        }

        public void DevicePreUpdateReply(DevicePreUpdateReplyBody reply)
        {
            try
            {
                Logger.LogItem(string.Format("Sending DevicePreUpdateReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.SYSTEMAPI);
                outputChannel.DevicePreUpdateReply(new DevicePreUpdateReply(GenerateHeader("DevicePreUpdateReply"), reply));
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending DevicePreUpdateReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (devicePreUpdateReplyQueue)
                {
                    devicePreUpdateReplyQueue.Enqueue(reply);
                }
            }
        }

        public void DeviceUpdateReply(DeviceUpdateReplyBody reply)
        {
            try
            {
                Logger.LogItem(string.Format("Sending DeviceUpdateReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.SYSTEMAPI);
                outputChannel.DeviceUpdateReply(new DeviceUpdateReply(reply, GenerateHeader("DeviceUpdateReply")));
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending DeviceUpdateReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (deviceUpdateReplyQueue)
                {
                    deviceUpdateReplyQueue.Enqueue(reply);
                }
            }
        }

        public void ElevationCleanupRequest(ElevationCleanupRequestBody request)
        {
            try
            {
                Logger.LogItem(string.Format("Sending ElevationCleanupRequest to Event Manager with Elevation SensorID: {0}", request.Elevation_ID), LogType.SYSTEMAPI);
                outputChannel.ElevationCleanupRequest(new ElevationCleanupRequest()); // fix
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending ElevationCleanupRequest to Event Manager with Elevation SensorID: {0}", request.Elevation_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (elevationCleanupRequestQueue)
                {
                    elevationCleanupRequestQueue.Enqueue(request);
                }
            }
        }

        public void ElevationReachableRequest(ElevationReachableRequestBody request)
        {
            try
            {
                Logger.LogItem(string.Format("Sending ElevationReachableRequest to Event Manager with Elevation SensorID: {0}", request.Elevation_ID), LogType.SYSTEMAPI);
                outputChannel.ElevationReachableRequest(new ElevationReachableRequest()); //fix
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending ElevationReachableRequest to Event Manager with Elevation SensorID: {0}", request.Elevation_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (elevationReachableRequestQueue)
                {
                    elevationReachableRequestQueue.Enqueue(request);
                }
            }
        }

        public void ElevationRequestRequest(ElevationRequestRequestBody request)
        {
            try
            {
                Logger.LogItem(string.Format("Sending ElevationRequestRequest to Event Manager with Elevation SensorID: {0}", request.Elevation_ID), LogType.SYSTEMAPI);
                outputChannel.ElevationRequestRequest(new ElevationRequestRequest()); //fix
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending ElevationRequestRequest to Event Manager with Elevation SensorID: {0}", request.Elevation_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (elevationRequestRequestQueue)
                {
                    elevationRequestRequestQueue.Enqueue(request);
                }
            }
        }

        public void ElevationRequiredRequest(ElevationRequiredRequestBody request)
        {
            try
            {
                Logger.LogItem(string.Format("Sending ElevationRequiredRequest to Event Manager with Elevation SensorID: {0}", request.Elevation_ID), LogType.SYSTEMAPI);
                outputChannel.ElevationRequiredRequest(new ElevationRequiredRequest()); //fix
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending ElevationRequiredRequest to Event Manager with Elevation SensorID: {0}", request.Elevation_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (elevationRequiredRequestQueue)
                {
                    elevationRequiredRequestQueue.Enqueue(request);
                }
            }
        }

        public void SystemPassRequiredRequest(SystemPassRequiredRequestBody request)
        {
            try
            {
                Logger.LogItem(string.Format("Sending SystemPassRequiredRequest to Event Manager with Elevation SensorID: {0}", request.Elevation_ID), LogType.SYSTEMAPI);
                outputChannel.SystemPassRequiredRequest(new SystemPassRequiredRequest()); //fix
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending SystemPassRequiredRequest to Event Manager with Elevation SensorID: {0}", request.Elevation_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (systemPassRequiredRequestQueue)
                {
                    systemPassRequiredRequestQueue.Enqueue(request);
                }
            }
        }

        public void DeviceValueReply(DeviceValueReplyBody reply)
        {
            try
            {
                Logger.LogItem(string.Format("Sending DeviceValueReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.SYSTEMAPI);
                outputChannel.DeviceValueReply(new DeviceValueReply(reply, GenerateHeader("DeviceValueReply")));
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending DeviceValueReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (deviceValueReplyQueue)
                {
                    deviceValueReplyQueue.Enqueue(reply);
                }
            }
        }

        public void DeviceListReply(DeviceListReplyBody reply)
        {
            try
            {
                Logger.LogItem(string.Format("Sending DeviceListReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.SYSTEMAPI);
                outputChannel.DeviceListReply(new DeviceListReply(reply, GenerateHeader("DeviceListReply")));
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending DeviceListReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (deviceListReplyQueue)
                {
                    deviceListReplyQueue.Enqueue(reply);
                }
            }
        }

        public void DeviceZoneListReply(DeviceZoneListReplyBody reply)
        {
            try
            {
                Logger.LogItem(string.Format("Sending DeviceZoneListReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.SYSTEMAPI);
                outputChannel.DeviceZoneListReply(new DeviceZoneListReply(reply, GenerateHeader("DeviceZoneListReply")));
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending DeviceZoneListReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (deviceZoneListReplyQueue)
                {
                    deviceZoneListReplyQueue.Enqueue(reply);
                }
            }
        }

        public void DeviceGroupListReply(DeviceGroupListReplyBody reply)
        {
            try
            {
                Logger.LogItem(string.Format("Sending DeviceGroupListReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.SYSTEMAPI);
                outputChannel.DeviceGroupListReply(new DeviceGroupListReply(reply, GenerateHeader("DeviceGroupListReply")));
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending DeviceGroupListReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (deviceGroupListReplyQueue)
                {
                    deviceGroupListReplyQueue.Enqueue(reply);
                }
            }
        }

        public void DeviceTypeListReply(DeviceTypeListReplyBody reply)
        {
            try
            {
                Logger.LogItem(string.Format("Sending DeviceTypeListReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.SYSTEMAPI);
                outputChannel.DeviceTypeListReply(new DeviceTypeListReply(reply, GenerateHeader("DeviceTypeListReply")));
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending DeviceTypeListReply to Event Manager with EM SensorID: {0}", reply.EM_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (deviceTypeListReplyQueue)
                {
                    deviceTypeListReplyQueue.Enqueue(reply);
                }
            }
        }

        public void DeviceChangedEvent(DeviceChangedEventBody deviceEvent)
        {
            try
            {
                Logger.LogItem(string.Format("Sending DeviceChangedEvent to Event Manager with EM SensorID: {0}", deviceEvent.EM_ID), LogType.SYSTEMAPI);
                outputChannel.DeviceChangedEvent(new DeviceChangedEvent(deviceEvent, GenerateHeader("DeviceChangedEvent")));
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending DeviceChangedEvent to Event Manager with EM SensorID: {0}", deviceEvent.EM_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (deviceZoneListReplyQueue)
                {
                    deviceChangedEventQueue.Enqueue(deviceEvent);
                }
            }
        }

        
        public void DevicePreUpdateEvent(DevicePreUpdateEventBody deviceEvent)
        {
            try
            {
                Logger.LogItem(string.Format("Sending DevicePreUpdateEvent to Event Manager with EM SensorID: {0}", deviceEvent.EM_ID), LogType.SYSTEMAPI);
                outputChannel.DevicePreUpdateEvent(new DevicePreUpdateEvent(deviceEvent, GenerateHeader("DeviceChangedEvent")));
            }
            catch (Exception)
            {
                channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending DevicePreUpdateEvent to Event Manager with EM SensorID: {0}", deviceEvent.EM_ID), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();

                lock (devicePreUpdateEventQueue)
                {
                    devicePreUpdateEventQueue.Enqueue(deviceEvent);
                }
            }
        }

        private DeviceAPIHeader GenerateHeader(string requesttype)
        {
            return new DeviceAPIHeader("[LOS]" + Guid.NewGuid(), DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                       requesttype, CurrentRequestVersion);
        }
    }
}