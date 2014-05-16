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
//  File            :   Sensors.cs                                      //
//  Description     :   Contains all sensor data                        //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   26-2-2014                                       //
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
//  ToDo: Add comments to methods                                       //
//  ToDo: Add errors                                                    //
//                                                                      //
//----------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.Linq;
using LyvinDataStoreLib.LyvinDeviceData.DatabaseHelperObjects;
using LyvinDataStoreLib.Models;
using LyvinObjectsLib.Devices;

namespace LyvinDataStoreLib.LyvinDeviceData
{
    /// <summary>
    /// Contains all sensor data
    /// </summary>
    public class SensorData
    {

        public List<object> BreakSensor;
        public List<object> GasSensors;
        public List<object> HumiditySensors;
        public List<object> LightSensors;
        public List<MotionPIRSensor> MotionPIRSensors;
        public List<OpenCloseSensor> OpenCloseSensors;
        public List<object> SmokeSensors;
        public List<object> TemperatureSensors;

        public SensorData()
        {
            MotionPIRSensors = new List<MotionPIRSensor>();
            OpenCloseSensors = new List<OpenCloseSensor>();
        }

        public MotionPIRSensor GetMotionPIRSensor(string driverID)
        {
            return MotionPIRSensors.Exists(m => m.DatabaseHelperDevice.DriverID == driverID) ? MotionPIRSensors.SingleOrDefault(m => m.DatabaseHelperDevice.DriverID == driverID) : null;
        }

        public MotionPIRSensor GetMotionPIRSensor(ulong deviceID)
        {
            return MotionPIRSensors.Exists(m => m.DeviceID == deviceID) ? MotionPIRSensors.SingleOrDefault(m => m.DeviceID == deviceID) : null;
        }

        public OpenCloseSensor GetOpenCloseSensor(string driverID)
        {
            return OpenCloseSensors.Exists(s => s.DatabaseHelperDevice.DriverID == driverID) ? OpenCloseSensors.SingleOrDefault(s => s.DatabaseHelperDevice.DriverID == driverID) : null;
        }

        public OpenCloseSensor GetOpenCloseSensor(ulong deviceID)
        {
            return OpenCloseSensors.Exists(s => s.DeviceID == deviceID) ? OpenCloseSensors.SingleOrDefault(s => s.DeviceID == deviceID) : null;
        }

        public DatabaseHelperDevice DiscoveredMotionPIRSensorDevice(LyvinDevice sensor)
        {
            using (var lyvinsdb = new Database("lyvinsdb"))
            {
                var device = lyvinsdb.SingleOrDefault<DatabaseHelperDevice>(
                    "SELECT * from device WHERE DriverID=@0", sensor.ID);
                if (device == null)
                {
                    device = new DatabaseHelperDevice(sensor.DeviceSettingsFile, sensor.ID, sensor.DriverFile, "ACTIVE", sensor.Type);
                }
                else
                {
                    device.Status = "ACTIVE";
                }
                lyvinsdb.Save(device);

                var motionpir = lyvinsdb.SingleOrDefault<MotionPIRSensor>(
                    "SELECT * from motionpirsensor WHERE DeviceID=@0", device.DeviceID);
                if (motionpir == null)
                {
                    motionpir = new MotionPIRSensor(device);
                }
                else
                {
                    motionpir.DatabaseHelperDevice = device;
                }
                lyvinsdb.Save(motionpir);

                MotionPIRSensors.RemoveAll(m => m.DeviceID == device.DeviceID);
                MotionPIRSensors.Add(motionpir);
                return device;
            }
        }

        public void RemoveMotionPIRSensorDevice(ulong deviceid)
        {
            try
            {
                MotionPIRSensors.RemoveAll(s => s.DeviceID == deviceid);
            }
            catch (Exception e)
            {
                // ToDo: Add error
            }
        }

        public DatabaseHelperDevice DiscoveredOpenCloseSensorDevice(LyvinDevice sensor)
        {
            using (var lyvinsdb = new Database("lyvinsdb"))
            {
                var device = lyvinsdb.SingleOrDefault<DatabaseHelperDevice>(
                    "SELECT * from device WHERE DriverID=@0", sensor.ID);
                if (device == null)
                {
                    device = new DatabaseHelperDevice(sensor.DeviceSettingsFile, sensor.ID, sensor.DriverFile, "ACTIVE", sensor.Type);
                }
                else
                {
                    device.Status = "ACTIVE";
                }
                lyvinsdb.Save(device);

                var openclose = lyvinsdb.SingleOrDefault<OpenCloseSensor>(
                    "SELECT * from openclosesensor WHERE DeviceID=@0", device.DeviceID);
                if (openclose == null)
                {
                    openclose = new OpenCloseSensor(sensor.Type, device);
                }
                else
                {
                    openclose.DatabaseHelperDevice = device;
                    openclose.Type = sensor.Type;
                }
                lyvinsdb.Save(openclose);

                OpenCloseSensors.RemoveAll(s => s.DeviceID == device.DeviceID);
                OpenCloseSensors.Add(openclose);
                return device;
            }
        }

        public void RemoveOpenCloseSensorDevice(ulong deviceid)
        {
            try
            {
                OpenCloseSensors.RemoveAll(s => s.DeviceID == deviceid);
            }
            catch (Exception e)
            {
                // ToDo: Add error
            }
        }
    }
}