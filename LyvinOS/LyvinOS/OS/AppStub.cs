////----------------------------------------------------------------------//
////  ___           ___    ___ ___      ___ ___  ________   ________      //
//// |\  \         |\  \  /  /|\  \    /  /|\  \|\   ___  \|\   ____\     //
//// \ \  \        \ \  \/  / | \  \  /  / | \  \ \  \\ \  \ \  \___|_    //
////  \ \  \        \ \    / / \ \  \/  / / \ \  \ \  \\ \  \ \_____  \   //
////   \ \  \____    \/  /  /   \ \    / /   \ \  \ \  \\ \  \|____|\  \  //
////    \ \_______\__/  / /      \ \__/ /     \ \__\ \__\\ \__\____\_\  \ //
////     \|_______|\___/ /        \|__|/       \|__|\|__| \|__|\_________\//
////              \|___|/                                     \|_________|//
////                                                                      //
////----------------------------------------------------------------------//
////  File            :   AppStub.cs                                      //
////  Description     :   Performs some actions that should be in an app  //
////  Original author :   J.Klessens                                      //
////  Company         :   Lyvins                                          //
////  Project         :   LyvinOS                                         //
////  Created on      :   26-2-2014                                       //
////----------------------------------------------------------------------//
////                                                                      //
////  Copyright (c) 2013, 2014 Lyvins                                     //
////                                                                      //
////  Licensed under the Apache License, Version 2.0 (the "License");     //
////  you may not use this file except in compliance with the License.    //
////  You may obtain a copy of the License at                             //
////                                                                      //
////      http://www.apache.org/licenses/LICENSE-2.0                      //
////                                                                      //
////  Unless required by applicable law or agreed to in writing, software //
////  distributed under the License is distributed on an "AS IS" BASIS,   //
////  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or     //
////  implied. See the License for the specific language governing        //
////  permissions and limitations under the License.                      //
////                                                                      //
////----------------------------------------------------------------------//
////                                                                      //
////  Prerequisites / Return Codes / Notes                                //
////                                                                      //
////----------------------------------------------------------------------//
////                                                                      //
////  Tasks / Features / Bugs                                             //
////  ToDo: Move most of the functionality to an app                      //
////                                                                      //
////----------------------------------------------------------------------//
//
//
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Timers;
//using LyvinObjectsLib.Devices;
//using LyvinObjectsLib.Events;
//using LyvinObjectsLib.Actions;
//
//namespace LyvinOS.VI
//{
//    class AppStub
//    {
//        //private readonly VIMain viMain;
//
//        private readonly DeviceManager deviceManager;
//
//        private readonly Dictionary<string, string> hueLocations = new Dictionary<string, string> {
//                                                                                             { "[00:17:88:09:ef:12]Extended color light 1-1", "Living-Room"}, 
//                                                                                             { "[00:17:88:09:ef:12]Dimmable light 1-2", "Living-Room" }, 
//                                                                                             { "[00:17:88:09:ef:12]Extended color light 2-3", "Living-Room" },
//                                                                                             { "[00:17:88:09:ef:12]Extended color light 3-4", "Living-Room"},
//                                                                                             { "[00:17:88:09:ef:12]Dimmable light 2-5", "Bedroom"},
//                                                                                             { "[00:17:88:09:ef:12]Dimmable light 3-6", "Bedroom"},
//                                                                                             { "[00:17:88:09:ef:12]Dimmable light 4-7", "Bedroom"},
//                                                                                             { "[00:17:88:09:ef:12]Dimmable light 5-8", "Bedroom"},
//                                                                                             { "[00:17:88:09:ef:12]Bar light-9", "Hall"},
//                                                                                             { "[00:17:88:09:ef:12]Halogeen lamp-10", "Living-Room"}};
//
//        private readonly Dictionary<string, string> sensorLocations = new Dictionary<string, string> {
//                                                                                             { "[Motion-PIR]1.3.1", "Living-Room"}, 
//                                                                                             { "[Motion-PIR]2.2.1", "Living-Room" }, 
//                                                                                             { "[Motion-PIR]2.1.1", "Bathroom" },
//                                                                                             { "[Motion-PIR]1.2.1", "Hall"},
//                                                                                             { "[Motion-PIR]1.1.1", "Kitchen"},
//                                                                                             { "[Motion-PIR]2.3.1", "Bedroom"}};
//
//        private readonly Dictionary<string, DateTime> lastMotion = new Dictionary<string, DateTime>();
//        private const int Delay = 60;
//
//        private Timer aTimer;ries
//
//        public AppStub(VIMain vi, DeviceManager devicemanager)
//        {
//            viMain = vi;
//            deviceManager = devicemanager;
//        }
//
//        public void Initialize()
//        {
//            CreateDeviceZone("Living-Room");
//            CreateDeviceZone("Kitchen");
//            CreateDeviceZone("Hall");
//            CreateDeviceZone("Bathroom");
//            CreateDeviceZone("Bedroom");
//
//            foreach (var hue in hueLocations.Keys)
//            {
//                var dz = deviceManager.GetDeviceZone(hueLocations[hue]);
//                var d = deviceManager.GetDevice(hue);
//                if ((d!=null)&&(dz!=null))
//                {
//                    dz.Devices.Add(d);
//                    d.DeviceZones.Add(dz);
//                }
//            }
//
//            foreach (var sensor in sensorLocations.Keys)
//            {
//                var dz = deviceManager.GetDeviceZone(sensorLocations[sensor]);
//                var d = deviceManager.GetDevice(sensor);
//                if ((d != null) && (dz != null))
//                {
//                    dz.Devices.Add(d);
//                    d.DeviceZones.Add(dz);
//                }
//            }
//
//            SetBrightnessAndColor();
//            
//
//            aTimer = new Timer(1000);
//            aTimer.Elapsed += OnTimedEvent;
//            aTimer.Enabled = true;
//        }
//
//        private void OnTimedEvent(object source, ElapsedEventArgs e)
//        {
//            var zone = (from key in lastMotion.Keys let s = DateTime.Now - lastMotion[key] where s.TotalSeconds > Delay select key).ToList();
//            if (zone.Count <= 0) return;
//            foreach (var z in zone)
//            {
//                lastMotion.Remove(z);
//                TurnOffLights(z);
//            }
//        }
//    
//
//        private void CreateDeviceZone(string zone)
//        {
//            var dz = deviceManager.GetDeviceZone(zone);
//
//            if (dz == null)
//            {
//                dz = new DeviceZone(zone, zone, zone, 0, 0, 0, 0);
//                deviceManager.DeviceZones.Add(dz);
//            }
//        }
//
//        public int ReceiveEvent(LyvinEvent e)
//        {
//            if (e.Code == "SENSOR_MOTION")
//            {
//                if (sensorLocations.ContainsKey(e.SourceID))
//                {
//                    var d = deviceManager.GetDevice(e.SourceID);
//                    if (d != null)
//                    {
//                        switch (e.Value)
//                        {
//                            case "false":
//
//                                foreach (var deviceZone in d.DeviceZones)
//                                {
//                                    SetLastMotion(deviceZone.SensorID);
//                                }
//
//                                return 0;
//                            case "true":
//                                foreach (var deviceZone in d.DeviceZones)
//                                {
//                                    TurnOnLights(deviceZone.SensorID);
//                                    if (lastMotion.ContainsKey(deviceZone.SensorID))
//                                        lastMotion.Remove(deviceZone.SensorID);
//                                }
//                                return 0;
//                        }
//                    }
//                }
//            }
//            return 0;
//        }
//
//        private void TurnOnLights(string zone)
//        {
//            var dz = deviceManager.GetDeviceZone(zone);
//
//            if (dz != null)
//            {
//                foreach (var d in dz.Devices)
//                {
//                    switch (d.Type)
//                    {
//                        case "COLORED_LIGHT":
//                        case "LIGHT":
//                        case "DIMMABLE_DEVICE":
//                            viMain.DoAction(new LyvinAction("SET_STATUS", "System", "System", d.SensorID, d.Type, "ON"));
//                            break;
//                    }
//                }
//            }
//        }
//
//        private void SetBrightnessAndColor()
//        {
//            foreach (var d in hueLocations.Keys.Select(hue => deviceManager.GetDevice(hue)).Where(d => d != null))
//            {
//                viMain.DoAction(new LyvinAction("SET_BRIGHTNESS", "System", "System", d.SensorID, d.Type, "100"));
//                if (d.Type == "COLORED_LIGHT")
//                {
//                    viMain.DoAction(new LyvinAction("SET_RGB", "System", "System", d.SensorID, d.Type, "255,255,255"));
//                }
//            }
//        }
//
//        private void TurnOffLights(string zone)
//        {
//            var dz = deviceManager.GetDeviceZone(zone);
//
//            if (dz != null)
//            {
//                foreach (var d in dz.Devices)
//                {
//                    switch (d.Type)
//                    {
//                        case "COLORED_LIGHT":
//                        case "LIGHT":
//                        case "DIMMABLE_DEVICE":
//                            viMain.DoAction(new LyvinAction("SET_STATUS", "System", "System", d.SensorID, d.Type, "OFF"));
//                            break;
//                    }
//                }
//            }
//        }
//
//        private void SetLastMotion(string zone)
//        {
//            if (lastMotion.ContainsKey(zone))
//            {
//                lastMotion[zone] = DateTime.Now;
//            }
//            else
//            {
//                lastMotion.Add(zone, DateTime.Now);
//            }
//        }
//            
//    }
//}
