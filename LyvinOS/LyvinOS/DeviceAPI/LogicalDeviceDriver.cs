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
//  File            :   LogicalDeviceDriver.cs                          //
//  Description     :   Interface between the driver libraries          //
//                      and the LyvinOS system.                         //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   18-04-2013                                      //
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
//  ToDo: Make a clearer distinction between internal device 
//                                                                      //
//----------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.Linq;
using LyvinAILib.InternalEventMessages;
using LyvinDeviceDriverLib;
using LyvinOS.OS.InternalEventManager;
using LyvinObjectsLib;
using LyvinObjectsLib.Actions;
using LyvinObjectsLib.Devices;
using LyvinSystemLogicLib;

namespace LyvinOS.DeviceAPI
{
    /// <summary>
    /// Interface between the driver libraries and the LyvinOS system
    /// </summary>
    public class LogicalDeviceDriver : ILogicalDeviceDriver
    {
        private readonly DeviceDataConnector deviceDataConnector;
        private readonly DeviceManager deviceManager;
        private readonly DriverManager driverManager;
        private readonly CommunicationManager communicationManager;
        private readonly IEManager ieManager; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devicemanager"></param>
        /// <param name="comManager"></param>
        /// <param name="iemanager"></param>
        /// <param name="deviceDataConnector"></param>
        public LogicalDeviceDriver(DeviceManager devicemanager, CommunicationManager comManager, IEManager iemanager, DeviceDataConnector deviceDataConnector)
        {
            deviceManager = devicemanager;
            driverManager = new DriverManager();
            communicationManager = comManager;
            ieManager = iemanager;
            this.deviceDataConnector = deviceDataConnector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public int DoAction(LyvinAction action)
        {
            Logger.LogItem(
                "Sending action \"" + action.Code + "\" from \"" + action.SourceID + "\" to \"" + action.TargetID +
                "\" with value \"" + action.Value + "\".", LogType.DEVICES);
            switch (action.Code)
            {
                case "DISCOVER":
                    switch (action.TargetID)
                    {
                        case "ALL":
                            foreach (var pdd in driverManager.DeviceDrivers)
                            {
                                pdd.DoAction(Converter.ConvertToDeviceAction(action), null);
                            }
                            return 0;
                        default:
                            return 1;
                    }
                default:
                    switch (action.TargetID)
                    {
                        case "ALL":
                            foreach (var device in deviceManager.Devices)
                            {
                                var temp = deviceManager.GetAllSubDevices(device);
                                foreach (var d in temp)
                                {
                                    d.Driver.DoAction(Converter.ConvertToDeviceAction(action), d.DeviceSettings);
                                }
                                device.Driver.DoAction(Converter.ConvertToDeviceAction(action), device.DeviceSettings);
                            }
                            return 0;
                        default:
                            var defaultdevice = deviceManager.GetDevice(action.TargetID);
                            if (defaultdevice != null)
                            {
                                defaultdevice.Driver.DoAction(Converter.ConvertToDeviceAction(action),
                                                              defaultdevice.DeviceSettings);
                                return 0;
                            }
                            return 1;
                    }
            }
        }

        /*
        //ToDo: Check if this method is even needed anymore
	    public List<Action> GetActions()
        {
            List<Action> temp = new List<Action>();
            if (driverManager.DeviceDrivers != null)
            {
                foreach (var physicalDeviceDriver in driverManager.DeviceDrivers)
                {
                    if (physicalDeviceDriver.Actions != null)
                    {
                        foreach (var action in physicalDeviceDriver.Actions)
                        {
                            temp.Add(new Action(action.Code, action.SourceID, action.SourceType,
                                                action.TargetID, action.TargetType, ""));
                        }
                    }
                }
            }
	        return temp;
        }*/

        /// <summary>
        /// For now this does nothing as all possible events are already loaded by viMain by using the functions in this class.
        /// If it is necessary for this class to also know events and/or actions, this will load all possible events and actions from the Drivers.
        /// </summary>
        public int Initialize()
        {
            return driverManager.LoadDrivers(this, communicationManager);
        }

        public int DiscoveredDevices(List<PhysicalDevice> devices, IPhysicalDeviceDriver pdd)
        {
            foreach (var physicalDevice in devices)
            {
                if (deviceManager.Devices.Exists(d => d.ID == physicalDevice.ID))
                {
                    deviceManager.Devices.Remove(deviceManager.Devices.Single(d => d.ID == physicalDevice.ID));
                }

                Logger.LogItem(
                    "Discovered new device \"" + physicalDevice.Name + "\" of type \"" + physicalDevice.Type +
                    "\" with SensorID \"" + physicalDevice.ID + "\" and using the driver \"" + physicalDevice.DriverType +
                    "\".", LogType.DEVICES);
                AddDevice(physicalDevice, pdd);
                InitializeDevice(physicalDevice.ID);
            }
            return 1;
        }

        private void AddDevice(PhysicalDevice device, IPhysicalDeviceDriver pdd)
        {
            var lyvinDevice = Converter.ConvertPhysicalDevice(device, pdd,
                                                              "Devices\\Settings\\" + pdd.Type + "\\" +
                                                              Converter.CleanFileName(device.ID) + ".xml");

            deviceManager.AddDevice(lyvinDevice);
            deviceDataConnector.DiscoveredDevice(lyvinDevice);

//            switch (device.Type)
//            {
//                case "MOTION_SENSOR":
//                    deviceData.Sensors.DiscoveredMotionPIRSensorDevice(new MotionPIRSensorDevice("", device.ID, new List<DeviceInRoom>(), new List<DeviceInZone>(), new DeviceUniqueIdentifier(device.ID)));
//                    break;
//                    // ToDo: Add other devices when necessary
//                case "HUB":
//                    foreach (var physicalDevice in device.DeviceList)
//                    {
//                        if (physicalDevice.Type == "HUB")
//                        {
//                            foreach (var physicalDevice1 in physicalDevice.DeviceList)
//                            {
//                                if (physicalDevice1.Type == "MOTION_SENSOR")
//                                {
//                                    deviceData.Sensors.DiscoveredMotionPIRSensorDevice(new MotionPIRSensorDevice("", device.ID, new List<DeviceInRoom>(), new List<DeviceInZone>(), new DeviceUniqueIdentifier(device.ID)));
//                                }
//                            }
//                        }
//                        if (physicalDevice.Type == "MOTION_SENSOR")
//                        {
//                            deviceData.Sensors.DiscoveredMotionPIRSensorDevice(new MotionPIRSensorDevice("", device.ID, new List<DeviceInRoom>(), new List<DeviceInZone>(), new DeviceUniqueIdentifier(device.ID)));
//                        }
//                    }
//                    break;
//            }
        }

        private void InitializeDevice(string deviceID)
        {
            foreach (var device in deviceManager.GetAllSubDevices(deviceManager.GetDevice(deviceID)))
            {
                Logger.LogItem(
                    "Initializing new device \"" + device.Name + "\" of type \"" + device.Type + "\" with SensorID \"" +
                    device.ID + "\".", LogType.DEVICES);
                //device.Driver.DoAction(new DeviceAction("GET_ALL_STATES", device.Type, "", device.SensorID), device.DeviceSettings);
                //System.Threading.Thread.Sleep(250);
                ieManager.ReceiveDeviceDiscoveryEvent10();
                //viMain.ReceiveEvent(new LyvinEvent("DEVICE_DISCOVERY", device.SensorID, device.Type, device.Name));
            }
        }

        private void WriteDeviceSettings(PhysicalDevice device, IPhysicalDeviceDriver pdd)
        {
            if (device.DeviceList != null)
            {
                foreach (var d in device.DeviceList)
                {
                    WriteDeviceSettings(d, pdd);
                }
            }
            var temp = new List<object> {device.DeviceSettings};
            XMLParser.WriteXMLFromObjects(
                "Devices\\Settings\\" + pdd.Type + "\\" + Converter.CleanFileName(device.ID) + ".xml", temp,
                device.DeviceSettingsType);
        }

        private void WriteDeviceSettings(LyvinDevice device)
        {
            var temp = new List<object> {device.DeviceSettings};
            XMLParser.WriteXMLFromObjects(
                "Devices\\Settings\\" + device.Driver.Type + "\\" + Converter.CleanFileName(device.ID) + ".xml", temp,
                device.DeviceSettingsType);
        }

        public int HandleDeviceEvent(DeviceEvent deviceEvent, IPhysicalDeviceDriver physicalDeviceDriver,
                                     object deviceSettings)
        {
            Logger.LogItem(
                "Received event \"" + deviceEvent.Code + "\" from \"" + deviceEvent.SourceID + "\" with value \"" +
                deviceEvent.Value + "\".", LogType.DEVICES);

            var device = deviceManager.GetDevice(deviceEvent.SourceID);
            if (device == null)
            {
                return 1;
            }

            device.DeviceSettings = deviceSettings;

            //ToDo: Save devicesettings to disk or database

            //ToDo: Create an eventhandler that raises a device event, to which the IEManager can subscribe
            var header = new IEHeader(Guid.NewGuid().ToString(), null, DateTime.Now, "50", 1);
            var iedevice = new IE50Device(device.ID, device.Name);
            var iedevicetype = new IE50DeviceType(device.Type, device.Type);
            ieManager.ReceiveDeviceEvent50(new IE50DeviceEvent(iedevice, iedevicetype, header, deviceEvent.Value));

            return 0;
        }

        public int LogRawData(RawDeviceData deviceData, IPhysicalDeviceDriver physicalDeviceDriver)
        {
            deviceDataConnector.StoreRawDeviceData(deviceData, physicalDeviceDriver.Type);
            return (int)ReturnCodes.Success;
        }
    }
}