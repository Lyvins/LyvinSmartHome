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
//  File            :   Main.cs                                         //
//  Description     :   Starts the Lyvin System                         //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   01-04-2013                                      //
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
//  ToDo: Check how to run Lyvin System as a service                    //
//                                                                      //
//----------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using LyvinOS.OS;
using LyvinSystemLogicLib;
using Application = System.Windows.Forms.Application;

namespace LyvinOS
{
    /// <summary>
    /// Starts the Lyvin System as a form or a console app. 
    /// </summary>
    public class LyvinSystem
    {

        private readonly OSManager osManager;
        private readonly LyvinUI lyvinUI;
        
        [FlagsAttribute]
        public enum ExecutionState : uint
        {
            EsAwaymodeRequired = 0x00000040,
            EsContinuous = 0x80000000,
            EsDisplayRequired = 0x00000002,
            EsSystemRequired = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        internal static Assembly AssemblyResolveHandler(object sender, ResolveEventArgs args)
        {
            string libPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                             Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar +
                             "CommonFiles" +
                             Path.DirectorySeparatorChar;

            Assembly assembly = Assembly.LoadFrom(libPath + args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll");


            return assembly;
        }

        private static List<Var> CreateDefaultConfig()
        {
            List<Var> varKeeper = new List<Var>
                                      {
                                          new Var("Version", "string", "LyvinOS-0.1"),
                                          new Var("LinkedStart", "bool", "false"),
                                          new Var("GUI", "bool", "false"),
                                          new Var("ALLOW_ELEVATION", "bool", "false"),
                                          new Var("ALLOW_INTERACTIVE_ELEVATION", "bool", "false"),
                                          new Var("REQUIRE_PERSONAL_ELEVATION", "bool", "false"),
                                          new Var("SystemPassHash", "string",
                                                  Cryptography.HashPassword("SystemPass", "LyvinSystemPass")),
                                          new Var("Use_Database", "bool", "true"),
                                          new Var("LogData_Primary_DataStore", "string", "XML"),               // Database = get primary data from database and save to xml and database
                                          new Var("DeviceData_Primary_DataStore", "string", "None"),                // XML = get primary data from xml and save to xml and database
                                          new Var("LayoutData_Primary_DataStore", "string", "XML"),                // None = Either detect it from the system or start empty and save it to both xml and database
                                          new Var("Database_Connected", "bool", "true"),
                                          new Var("Occupancy_LogData_XML_File", "string", "DataStore\\LogData\\OccupancyLogData.xml"),
                                          new Var("Sensor_LogData_XML_File", "string", "DataStore\\LogData\\SensorLogData.xml"),
                                          new Var("LayoutData_XML_File", "string", "DataStore\\LogData\\LayoutData.xml"),
                                          new Var("DeviceData_XML_File", "string", "DataStore\\LogData\\DeviceData.xml"),
                                          new Var("AI_Store_Motion_PIR_Delay", "int", "60"),
                                          new Var("AI_Store_Open_Close_Delay", "int", "3600"),
                                          new Var("AI_Motion_Event_Multiple_Room_Probability", "signed int", "10"),
                                          new Var("AI_Occupancy_Probability_High", "int", "85"),
                                          new Var("AI_Occupancy_Probability_Low", "int", "35"),
                                          new Var("AI_Gradual_Decay_Delay", "int", "60"),
                                          new Var("AI_Gradual_Decay_Value", "int", "-10"),
                                          new Var("Audio_Output_DeviceData_XML_File", "string", "DataStore\\DeviceData\\AudioOutputDeviceData.xml"),
                                          new Var("AV_Input_DeviceData_XML_File", "string", "DataStore\\DeviceData\\AVInputDeviceData.xml"),
                                          new Var("Lighting_DeviceData_XML_File", "string", "DataStore\\DeviceData\\LightingDeviceData.xml"),
                                          new Var("Security_DeviceData_XML_File", "string", "DataStore\\DeviceData\\SecurityDeviceData.xml"),
                                          new Var("Sensor_DeviceData_XML_File", "string", "DataStore\\DeviceData\\SensorDeviceData.xml"),
                                          new Var("Switch_DeviceData_XML_File", "string", "DataStore\\DeviceData\\SwitchDeviceData.xml"),
                                          new Var("Video_Output_DeviceData_XML_File", "string", "DataStore\\DeviceData\\VideoOutputDeviceData.xml"),
                                          new Var("Device_TimeOut_Data_XML_File", "string", "DataStore\\DeviceData\\DeviceTimeOutData.xml"),
                                          new Var("Activity_LayoutData_XML_File", "string", "DataStore\\LayoutData\\ActivityLayoutData.xml"),
                                          new Var("Building_LayoutData_XML_File", "string", "DataStore\\LayoutData\\BuildingLayoutData.xml"),
                                          new Var("Load_Lyvins_Layout", "bool", "true"),
                                          new Var("Debug_Logging", "bool", "false"),
                                          new Var("Log_Level", "string", "DEVICES")
                                      };
            //ToDo: Add values when necessary

            return varKeeper;
        }

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolveHandler;
            SubMain(args.ToList());
        }

        private static void SubMain(List<string> args)
        {
            if (!GlobalLogic.IsRunning(GlobalLogic.LyvinProcess.LyvinOS))
            {
                using (var appMutex = new Mutex(true, GlobalLogic.GetProcessRunningID(GlobalLogic.LyvinProcess.LyvinOS))
                    )
                {
                    Configuration.SetDefaultConfig(CreateDefaultConfig());

                    if (args.Contains("GUI"))
                    {
                        Configuration.SetValue("GUI", "true");
                    }
                    else if (args.Contains("CMD"))
                    {
                        Configuration.SetValue("GUI", "false");
                    }
                    else
                    {
                        DialogResult res = MessageBox.Show("Start with form?", "Startup", MessageBoxButtons.YesNo,
                                                           MessageBoxIcon.Question,
                                                           MessageBoxDefaultButton.Button1);
                        switch (res)
                        {
                            case DialogResult.Yes:
                                Configuration.SetValue("GUI", "true");
                                break;
                            case DialogResult.No:
                                Configuration.SetValue("GUI", "false");
                                break;
                        }
                    }
                    if (args.Contains("LS"))
                    {
                        Configuration.SetValue("LinkedStart", "true");
                    }
                    else if (args.Contains("NoLS"))
                    {
                        Configuration.SetValue("LinkedStart", "false");
                    }
                    else
                    {
                        var res = MessageBox.Show("Do a Linked start of all Lyvins Processes?", "Startup",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question,
                                                  MessageBoxDefaultButton.Button1);
                        switch (res)
                        {
                            case DialogResult.Yes:
                                Configuration.SetValue("LinkedStart", "true");
                                break;
                            case DialogResult.No:
                                Configuration.SetValue("LinkedStart", "false");
                                break;
                        }
                    }
                    if (args.Contains("DEBUG"))
                    {
                        Configuration.SetValue("Debug_Logging", "true");
                        Console.WriteLine("Setting debug logging to true.");
                    }
                    new LyvinSystem(Convert.ToBoolean(Configuration.GetValue("GUI", "bool")));
                }
            }
            else
            {
                MessageBox.Show("The DeviceAPIApplication is already running.");
            }
        }

        private LyvinSystem(bool form)
        {
            Logger.LogFileName = "LyvinOS.log";
#if DEBUG
            // debugging enabled, show all log items
            Logger.LogLevel = LogType.DEBUG;
#else
            bool debug = false;
            bool.TryParse(Configuration.GetValue("Debug_Logging", "bool").ToString(), out debug);
            if (debug)
                Logger.LogLevel = LogType.DEBUG;
            else
            {
                switch (Configuration.GetValue("Log_Level", "string").ToString())
                {
                    case "DEVICES":
                        Logger.LogLevel = LogType.DEVICES;
                        break;
                    case "OFF":
                        Logger.LogLevel = LogType.OFF;
                        break;
                    default:
                        Logger.LogLevel = LogType.DEVICES;
                        break;
                        //Add other options
                }
            }
                 //debugging disabled, read from config what level to log
  /*          switch (config.GetValue("LogLevel", "string"))
            {
                case "OFF":
                    Logger.LogLevel = LogType.OFF;
                    break;
                case "SYSTEM":
                    Logger.LogLevel = LogType.SYSTEM;
                    break;
                case "WARNING":
                    Logger.LogLevel = LogType.WARNING;
                    break;
                case "ERROR":
                    Logger.LogLevel = LogType.ERROR;
                    break;
                default: Logger.LogLevel = LogType.ALL;
                    break;
            }*/
#endif
            SetThreadExecutionState(ExecutionState.EsContinuous | ExecutionState.EsSystemRequired |
                                    ExecutionState.EsAwaymodeRequired);
            switch (form)
            {
                case true:
                    lyvinUI = new LyvinUI();
                    osManager = new OSManager();
                    Application.Run(lyvinUI);
                    break;
                case false:
                    osManager = new OSManager();
                    Console.WriteLine("Press key to exit...");
                    Console.ReadLine();
                    /*
                        ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[] {new LyvinSystemService()};
                        ServiceBase.Run(ServicesToRun);*/
                    break;
            }
            Logger.FlushLog();
            SetThreadExecutionState(ExecutionState.EsContinuous);
            Application.Exit();
        }
    }

    /// <summary>
    /// Starts the Lyvin system as a service.
    /// </summary>
    public class LyvinSystemService : ServiceBase
    {
        private readonly OSManager osManager;
        
        public LyvinSystemService()
        {
            osManager = new OSManager();
            InitializeComponent();
            Logger.LogItem("LyvinSystem has started as a service.", LogType.SYSTEM);
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }


        public void InitializeComponent()
        {
            
        }
    }
}