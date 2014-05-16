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
//  File            :   SensorData.cs                                   //
//  Description     :   Represents all sensor data                      //
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
//  -Implemented for 1.5 [AI] Presence                                  //
//  -ToDo: Test                                                         //
//                                                                      //
//----------------------------------------------------------------------//

using System.Collections.Generic;
using System.Linq;
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLogData
{
    /// <summary>
    /// Represents all sensor data
    /// </summary>
    public class SensorLogData
    {

        private readonly List<MotionPIRSensorLog> motionPIRSensors;
        private readonly List<OpenCloseSensorLog> openCloseSensors;

        public SensorLogData()
        {
            motionPIRSensors = new List<MotionPIRSensorLog>();
            openCloseSensors = new List<OpenCloseSensorLog>();
        }

        /// <summary>
        /// Adds a motion sensor log item to the data store
        /// </summary>
        /// <param name="item">The motion log item to be added</param>
        public void LogMotionPIRSensor(MotionPIRSensorLog item)
        {
            DBLogMotionPIRSensor(item);
            MemLogMotionPIRSensor(item);
        }

        /// <summary>
        /// Adds a motion sensor log item to the database
        /// </summary>
        /// <param name="item">The motion log item to be added</param>
        private void DBLogMotionPIRSensor(MotionPIRSensorLog item)
        {
            using (var lyvinsDb = new Database("lyvinsdb"))
            {
                lyvinsDb.Insert(item);
            }
        }

        /// <summary>
        /// Adds a motion sensor log item to the internal data store
        /// Also removes any entries with the same sensor DriverID, so only the last entry is in memory
        /// </summary>
        /// <param name="item">The motion log item to be added</param>
        private void MemLogMotionPIRSensor(MotionPIRSensorLog item)
        {
            lock (motionPIRSensors)
            {
                if (motionPIRSensors.Exists(s=> s.DeviceID == item.DeviceID))
                {
                    var items = motionPIRSensors.Where(s => s.DeviceID == item.DeviceID).ToList();
                    foreach (var motionPIRSensorLogItem in items)
                    {
                        motionPIRSensors.Remove(motionPIRSensorLogItem);
                    }
                }
                motionPIRSensors.Add(item);
            }
        }

        /// <summary>
        /// Adds an open close sensor log item to the data store
        /// </summary>
        /// <param name="item">The sensor log item to be added</param>
        public void LogOpenCloseSensor(OpenCloseSensorLog item)
        {
            DBLogOpenCloseSensor(item);
            MemLogOpenCloseSensor(item);
        }

        /// <summary>
        /// Adds an open close sensor log item to the database
        /// </summary>
        /// <param name="item">The sensor log item to be added</param>
        private void DBLogOpenCloseSensor(OpenCloseSensorLog item)
        {
            using (var lyvinsDb = new Database("lyvinsdb"))
            {
                lyvinsDb.Insert(item);
            }
        }

        /// <summary>
        /// Adds an open close sensor log item to the internal data store
        /// </summary>
        /// <param name="item">The sensor log item to be added</param>
        private void MemLogOpenCloseSensor(OpenCloseSensorLog item)
        {
            lock (openCloseSensors)
            {
                if (openCloseSensors.Exists(s => s.DeviceID == item.DeviceID))
                {
                    var items = openCloseSensors.Where(s => s.DeviceID == item.DeviceID);
                    foreach (var openCloseSensorLogItem in items)
                    {
                        openCloseSensors.Remove(openCloseSensorLogItem);
                    }
                }
                openCloseSensors.Add(item);
            }
        }

        /// <summary>
        /// Returns the last motion sensor log item that was added
        /// </summary>
        /// <returns>The last motion sensor log item that was added, or null if none</returns>
        public MotionPIRSensorLog GetLastMotionPIRSensorLog()
        {
            return motionPIRSensors.OrderByDescending(m => m.Triggered).FirstOrDefault();
        }

        /// <summary>
        /// Returns the last motion sensor log item that was added of a given sensor
        /// </summary>
        /// <param name="sensorid">The id of the sensor</param>
        /// <returns>The last sensor log item of the given sensor id</returns>
        public MotionPIRSensorLog GetLastMotionPIRSensorLog(ulong deviceid)
        {
            return motionPIRSensors.Where(m=> m.DeviceID == deviceid).OrderByDescending(m => m.Triggered).FirstOrDefault();
        }

        /// <summary>
        /// Returns the last open close sensor log item that was added
        /// </summary>
        /// <returns>The last open close sensor log item that was added</returns>
        public OpenCloseSensorLog GetLastOpenCloseSensorLog()
        {
            return openCloseSensors.OrderByDescending(m => m.Triggered).FirstOrDefault();
        }

        /// <summary>
        /// Returns the last open close sensor log item that was added of a given sensor
        /// </summary>
        /// <param name="sensorid">The id of the sensor</param>
        /// <returns>The last sensor log item of the given sensor</returns>
        public OpenCloseSensorLog GetLastOpenCloseSensorLog(ulong deviceid)
        {
            return openCloseSensors.Where(m => m.DeviceID == deviceid).OrderByDescending(m => m.Triggered).FirstOrDefault();
        }
    }
}