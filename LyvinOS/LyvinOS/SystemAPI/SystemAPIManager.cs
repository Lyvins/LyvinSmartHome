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
//  File            :   SystemAPIManager.cs                             //
//  Description     :   Creates and manages the System API              //
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
///////////////////////////////////////////////////////////
//  SystemAPIManager.cs
//  Implementation of the Class SystemAPIManager
//  Generated by Enterprise Architect
//  Created on:      05-jul-2013 2:33:20
//  Original author: J.Klessens
///////////////////////////////////////////////////////////


using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.Timers;
using LyvinOS.OS;
using LyvinOS.OS.Security;
using LyvinSystemLogicLib;
using Timer = System.Timers.Timer;

namespace LyvinOS.SystemAPI
{
    /// <summary>
    /// Creates and manages the System API
    /// </summary>
    public class SystemAPIManager
    {

        private SecurityManager securityManager;
        private DeviceRequestHandler deviceRequestHandler;

        private string emName = "LyvinEventManager";
        private string emLocation = "../LyvinEventManager/";
        private string emExtension = ".exe";
        private double reconnectDelay = 5000;
        private const double ConnectionPing = 2500;

        private ServiceHost lyvinOSInputHost;
        private readonly LyvinOSInputHost lyvinOSInputInstance;
        public LyvinOSOutputProxy LyvinOSOutputProxy;

        private Process eventManager; //Will eventually be used to check if the process is still running, etc.

        private readonly Timer reconnectTimer;
        private readonly Timer connectionTimer;

        public bool LyvinEMRunning { get; set; }
        public bool ConnectedToEM { get; set; }

        public SystemAPIManager()
        {
        }

        public void CloseSystemApi()
        {
            if (lyvinOSInputHost.State == CommunicationState.Opened)
            {
                lyvinOSInputHost.Close();
                LyvinOSOutputProxy.CLoseClient();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securitymanager"></param>
        /// <param name="devicerequesthandler"></param>
        public SystemAPIManager(SecurityManager securitymanager, DeviceRequestHandler devicerequesthandler)
        {
            ConnectedToEM = false;
            InitializeConfigValues();

            securityManager = securitymanager;
            deviceRequestHandler = devicerequesthandler;

            lyvinOSInputInstance = new LyvinOSInputHost(devicerequesthandler);

            reconnectTimer = new Timer(reconnectDelay);
            reconnectTimer.Elapsed += Reconnect;

            connectionTimer = new Timer(ConnectionPing);
            connectionTimer.Elapsed += PingConnection;

            StartRequestHosts();
            ConnectToClients();
        }

        private void Reconnect(object sender, ElapsedEventArgs e)
        {
            Logger.LogItem("Reconnecting to LyvinEM.", LogType.EMAPI);
            if (StartEventManager())
            {
                reconnectTimer.Enabled = false;
                ConnectToClients();
            }
        }

        private void PingConnection(object sender, ElapsedEventArgs e)
        {
            if (!LyvinOSOutputProxy.KeepAlive())
            {
                ConnectedToEM = false;
                Reconnect(sender, e);
            }
        }

        private void InitializeConfigValues()
        {
            if (Configuration.Exists("EventManagerProcessName"))
                emName = (string) Configuration.GetValue("EventManagerProcessName");
            else
                Configuration.AddVar("EventManagerProcessName", "string", emName);

            if (Configuration.Exists("EventManagerLocation"))
                emLocation = (string) Configuration.GetValue("EventManagerLocation");
            else
                Configuration.AddVar("EventManagerLocation", "string", emLocation);

            if (Configuration.Exists("EventManagerExtension"))
                emExtension = (string) Configuration.GetValue("EventManagerExtension");
            else
                Configuration.AddVar("EventManagerExtension", "string", emExtension);

            if (Configuration.Exists("LyvinOSAPIReconnectDelay"))
                double.TryParse((string) Configuration.GetValue("LyvinOSAPIReconnectDelay"), out reconnectDelay);
            else
                Configuration.AddVar("LyvinOSAPIReconnectDelay", "int",
                                     reconnectDelay.ToString(CultureInfo.InvariantCulture));

            if (Configuration.Exists("LyvinOSAPIConnectionPing"))
                double.TryParse((string) Configuration.GetValue("LyvinOSAPIConnectionPing"), out reconnectDelay);
            else
                Configuration.AddVar("LyvinOSAPIConnectionPing", "int",
                                     reconnectDelay.ToString(CultureInfo.InvariantCulture));
        }

        private bool StartEventManager()
        {
            LyvinEMRunning = GlobalLogic.IsRunning(GlobalLogic.LyvinProcess.LyvinEM);
            if (Convert.ToBoolean(Configuration.GetValue("LinkedStart", "bool")))
            {
                if (!LyvinEMRunning)
                {
                    var fi = new FileInfo(emLocation + emName + emExtension);
                    if (fi.Exists)
                    {
                        Logger.LogItem("Starting LyvinEM.", LogType.EMAPI);
                        var start = new ProcessStartInfo
                                        {
                                            FileName = fi.FullName,
                                            Arguments =
                                                Convert.ToBoolean(Configuration.GetValue("GUI", "bool"))
                                                    ? "GUI"
                                                    : "CMD"
                                        };
                        start.Arguments += " LS";
                        start.WorkingDirectory = emLocation;
                        eventManager = Process.Start(start);
                        if (eventManager != null)
                        {
                            Logger.LogItem("LyvinEM is started.", LogType.EMAPI);
                            LyvinEMRunning = true;
                        }
                    }
                    else
                    {
                        Logger.LogItem("LyvinEM could not be started.", LogType.ERROR);
                        LyvinEMRunning = false;
                    }
                }
            }
            return LyvinEMRunning;
        }

        private void StartRequestHosts()
        {
            if (StartLyvinOSInputHost())
            {
                Logger.LogItem(
                    string.Format("Opened LyvinOS Input Host at {0}.", lyvinOSInputHost.BaseAddresses),
                    LogType.SYSTEMAPI);
            }
            else
            {
                Logger.LogItem(
                    string.Format("Could not open Lyvin OS Input Host at {0}.", lyvinOSInputHost.BaseAddresses),
                    LogType.ERROR);
            }
        }

        private void ConnectToClients()
        {
            if (ConnectToLyvinOSOutputProxy())
            {
                Logger.LogItem(
                    string.Format("Connected to Lyvin OS Output Proxy at {0}.", LyvinOSOutputProxy.GetClientAddress()),
                    LogType.SYSTEMAPI);
                lyvinOSInputInstance.OutputProxy = LyvinOSOutputProxy;
                LyvinOSOutputProxy.SendQueuedRequests();
                ConnectedToEM = true;
                connectionTimer.Enabled = true;
            }
            else
            {
                Logger.LogItem(
                    string.Format("Could not connect to Lyvin OS Output Proxy at {0}.",
                                  LyvinOSOutputProxy.GetClientAddress()),
                    LogType.ERROR);
                reconnectTimer.Enabled = true;
            }
        }

        private bool StartLyvinOSInputHost()
        {
            lyvinOSInputHost = new ServiceHost(lyvinOSInputInstance);

            try
            {
                lyvinOSInputHost.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ConnectToLyvinOSOutputProxy()
        {
            LyvinOSOutputProxy = new LyvinOSOutputProxy();
            try
            {
                return LyvinOSOutputProxy.HandShake();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}