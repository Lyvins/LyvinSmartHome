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
//  File            :   CloudAPIManager.cs                                //
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
using System.Globalization;
using System.ServiceModel;
using System.Timers;
using LyvinSystemLogicLib;

namespace LyvinOS.CloudAPI
{
    class CloudAPIManager
    {
        private readonly string connectionName = "LyvinCloud";
        private readonly string name = "LyvinOS";

        private double reconnectDelay = 5000;
        private const double ConnectionPing = 2500;

        private ServiceHost inputHost;
        private readonly LyvinCloudInputHost inputInstance;
        public LyvinCloudOutputProxy OutputProxy;

        private readonly Timer reconnectTimer;
        private readonly Timer connectionTimer;

        public bool ConnectedToCloud { get; set; }

        public void CloseCloudApi()
        {
            if (inputHost.State == CommunicationState.Opened)
            {
                inputHost.Close();
                OutputProxy.CLoseClient();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securitymanager"></param>
        /// <param name="devicerequesthandler"></param>
        public CloudAPIManager()
        {
            ConnectedToCloud = false;
            InitializeConfigValues();

            inputInstance = new LyvinCloudInputHost();

            reconnectTimer = new Timer(reconnectDelay);
            reconnectTimer.Elapsed += Reconnect;

            connectionTimer = new Timer(ConnectionPing);
            connectionTimer.Elapsed += PingConnection;

            StartRequestHosts();
            ConnectToClients();
        }

        private void Reconnect(object sender, ElapsedEventArgs e)
        {
            Logger.LogItem(string.Format("Reconnecting to {0}.", connectionName), LogType.EMAPI);
            reconnectTimer.Enabled = false;
            ConnectToClients();
        }

        private void PingConnection(object sender, ElapsedEventArgs e)
        {
            if (!OutputProxy.KeepAlive())
            {
                ConnectedToCloud = false;
                Reconnect(sender, e);
            }
        }

        private void InitializeConfigValues()
        {
            if (Configuration.Exists("LyvinCloudAPIReconnectDelay"))
                double.TryParse((string)Configuration.GetValue("LyvinCloudAPIReconnectDelay"), out reconnectDelay);
            else
                Configuration.AddVar("LyvinCloudAPIReconnectDelay", "int",
                                     reconnectDelay.ToString(CultureInfo.InvariantCulture));

            if (Configuration.Exists("LyvinCloudAPIConnectionPing"))
                double.TryParse((string)Configuration.GetValue("LyvinCloudAPIConnectionPing"), out reconnectDelay);
            else
                Configuration.AddVar("LyvinCloudAPIConnectionPing", "int",
                                     reconnectDelay.ToString(CultureInfo.InvariantCulture));
        }

        private void StartRequestHosts()
        {
            if (StartLyvinCloudInputHost())
            {
                Logger.LogItem(
                    string.Format("Opened {0} Input Host at {1}.", name, inputHost.BaseAddresses),
                    LogType.SYSTEMAPI);
            }
            else
            {
                Logger.LogItem(
                    string.Format("Could not open {0} Input Host at {1}.", name, inputHost.BaseAddresses),
                    LogType.ERROR);
            }
        }

        private void ConnectToClients()
        {
            if (ConnectToLyvinCloudOutputProxy())
            {
                Logger.LogItem(
                    string.Format("Connected to {0} Output Proxy at {1}.", connectionName, OutputProxy.GetClientAddress()),
                    LogType.SYSTEMAPI);
                OutputProxy.SendQueuedRequests();
                ConnectedToCloud = true;
                connectionTimer.Enabled = true;
            }
            else
            {
                Logger.LogItem(
                    string.Format("Could not connect to {0} Proxy at {1}.", connectionName,
                                  OutputProxy.GetClientAddress()),
                    LogType.ERROR);
                reconnectTimer.Enabled = true;
            }
        }

        private bool StartLyvinCloudInputHost()
        {
            inputHost = new ServiceHost(inputInstance);

            try
            {
                inputHost.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ConnectToLyvinCloudOutputProxy()
        {
            OutputProxy = new LyvinCloudOutputProxy();
            try
            {
                return OutputProxy.HandShake();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
