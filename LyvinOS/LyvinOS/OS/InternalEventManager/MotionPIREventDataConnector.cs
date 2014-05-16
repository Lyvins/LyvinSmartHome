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
//  File            :   MotionPIREventDataConnector.cs                  //
//  Description     :   The interface between the data store and the    //
//                      Motion PIR Event Manager                        //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   27-2-2014                                       //
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
using LyvinAILib.InternalEventMessages;
using LyvinDataStoreLib.LyvinDeviceData;
using LyvinDataStoreLib.LyvinLogData;

namespace LyvinOS.OS.InternalEventManager
{
    /// <summary>
    /// The interface between the data store and the Motion PIR Event Manager
    /// </summary>
    public class MotionPIREventDataConnector
    {
        private readonly DeviceData deviceData;
        private readonly LogData logData;

        public MotionPIREventDataConnector(DeviceData deviceData, LogData logData)
        {
            this.deviceData = deviceData;
            this.logData = logData;
        }

        /// <summary>
        /// Returns the DeviceID of the last motion pir event
        /// </summary>
        /// <returns>The DeviceID of the last motion pir event, or "None" if no such event.</returns>
        public ulong GetPreviousMotionPIREventID()
        {
            var motionlog = logData.Sensors.GetLastMotionPIRSensorLog();
            if (motionlog!=null)
                return motionlog.DeviceID;
            return 0;
        }

        public DateTime GetPreviousMotionPIREventTime()
        {
            return logData.Sensors.GetLastMotionPIRSensorLog().Triggered;
        }

        public void StoreMotionPIREvent(IE50DeviceEvent motionEvent)
        {
            logData.Sensors.LogMotionPIRSensor(new MotionPIRSensorLog(GetMotionPIRDevice(motionEvent.Device.DriverDeviceID).DeviceID, motionEvent.IEHeader.TimeStamp));   // ToDo: Optional add people and repeat pattern
        }

        public bool GetDeviceTimeOut(ulong deviceID)
        {
            return deviceData.DeviceTimeOuts.GetDeviceTimeOut(deviceID);
        }

        public bool GetDeviceTimeOut(string driverID)
        {
            return GetDeviceTimeOut(deviceData.Sensors.GetMotionPIRSensor(driverID).DeviceID);
        }
        
        public MotionPIRSensor GetMotionPIRDevice(ulong deviceID)
        {
            return deviceData.Sensors.GetMotionPIRSensor(deviceID);
        }

        public MotionPIRSensor GetMotionPIRDevice(string driverID)
        {
            return deviceData.Sensors.GetMotionPIRSensor(driverID);
        }
    }
}
