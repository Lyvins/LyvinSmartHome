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
//  File            :   LyvinCloudOutputProxy.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                     //
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

using System;
using LyvinSystemLogicLib;

namespace LyvinOS.CloudAPI
{
    internal class LyvinCloudOutputProxy
    {
        private readonly string connectionName = "Lyvin Cloud";

        //private ISCLyvinOSOutputContract outputChannel;
        //private readonly ChannelFactory<ISCLyvinOSOutputContract> channelFactory;

        private const string CurrentRequestVersion = "0.1";

        //private readonly Queue<DevicePreUpdateReplyBody> devicePreUpdateReplyQueue;

        public LyvinCloudOutputProxy()
        {
            //channelFactory = new ChannelFactory<ISCLyvinOSOutputContract>("outputChannel");

            //devicePreUpdateReplyQueue = new Queue<DevicePreUpdateReplyBody>();
        }

        public string GetClientAddress()
        {
            return ""; // channelFactory.Endpoint.Address.ToString();
        }

        public bool CLoseClient()
        {
            /*if (channelFactory.State == CommunicationState.Opened)
            {
                Logger.LogItem(string.Format("Closing channel to Event Manager"), LogType.SYSTEMAPI);
                channelFactory.Close();
                outputChannel = null;
                return true;
            }*/
            return false;
        }

        public void SendQueuedRequests()
        {
            /*lock (devicePreUpdateReplyQueue)
            {
                while (devicePreUpdateReplyQueue.Count > 0)
                {
                    DevicePreUpdateReply(devicePreUpdateReplyQueue.Dequeue());
                }
            }*/
        }

        public void ClearQueuedRequests()
        {
            /*lock (devicePreUpdateReplyQueue)
            {
                devicePreUpdateReplyQueue.Clear();
            }*/
        }

        public bool HandShake()
        {
            Logger.LogItem(string.Format("Creating channel to {0}", connectionName), LogType.SYSTEMAPI);
            //outputChannel = channelFactory.CreateChannel();
            try
            {
                Logger.LogItem(string.Format("Sending handshake to {0}", connectionName), LogType.SYSTEMAPI);
                //return outputChannel.HandShake();
            }
            catch (Exception)
            {
                //channelFactory.Abort();
                Logger.LogItem(string.Format("Error: Sending HandShake to {0}", connectionName), LogType.ERROR);

                Logger.LogItem(string.Format("Closing channel to {0}", connectionName), LogType.SYSTEMAPI);
                //channelFactory.Close();
            }
            return false;
        }

        public bool KeepAlive()
        {
            /*try
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
            }*/
            return false;
        }
    }
}