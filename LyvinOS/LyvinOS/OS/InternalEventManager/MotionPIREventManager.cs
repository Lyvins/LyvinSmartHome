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
//  File            :   MotionPIREventManager.cs                        //
//  Description     :   Handles all Internal Device Events              //
//                      from Motion PIR sensors                         //
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
//----------------------------------------------------------------------//

using System;
using LyvinAILib;
using LyvinAILib.InternalEventMessages;
using LyvinDataStoreLib;
using LyvinSystemLogicLib;

namespace LyvinOS.OS.InternalEventManager
{
    /// <summary>
    /// Handles all Internal Device Events from Motion PIR sensors
    /// </summary>
    public class MotionPIREventManager
    {

        private IEManager ieManager;
        private MotionPIREventDataConnector dataConnector;
        
        /// <summary>
        /// 
        /// </summary>
        public MotionPIREventManager()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iemanager"></param>
        /// <param name="dsmanager"></param>
        public void Initialize(IEManager iemanager, DSManager dsmanager)
        {
            ieManager = iemanager;
            dataConnector = new MotionPIREventDataConnector(dsmanager.DeviceData, dsmanager.LogData);

            iemanager.IE50DeviceEvent += new EventHandler<InternalEventArgs<IE50DeviceEvent>>(SensorTriggered);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public bool CheckDeviceTimeOut(ulong deviceID)
        {
            return dataConnector.GetDeviceTimeOut(deviceID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        private bool CheckSameSensor(ulong deviceID)
        {
            var previousDeviceID = dataConnector.GetPreviousMotionPIREventID();
            return previousDeviceID == deviceID;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private int GetTimeDelay(DateTime timestamp)
        {
            try
            {
                var timedelay = (int)(timestamp - dataConnector.GetPreviousMotionPIREventTime()).TotalSeconds;
                return timedelay;
            }
            catch (Exception)
            {
                // Add error when errormanager is static
                return 0;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceEvent"></param>
        /// <param name="timedout"></param>
        /// <param name="emid"></param>
        /// <param name="samesensor"></param>
        /// <param name="timedelay"></param>
        /// <param name="people"></param>
        public void CreateAIEvent(IE50DeviceEvent deviceEvent, bool timedout, string emid, bool samesensor, int timedelay, int people)
        {
            IEHeader header = new IEHeader(Guid.NewGuid().ToString(), deviceEvent.IEHeader.ID, DateTime.Now, "1000",  1);
            IE1000Input input = new IE1000Input(dataConnector.GetMotionPIRDevice(deviceEvent.Device.DriverDeviceID).DeviceID, people, deviceEvent.IEHeader.TimeStamp);
            IE1000Output output = new IE1000Output(emid, samesensor, timedelay, timedout);

            ieManager.RaiseAIEvent(new IE1000AIEvent(header, input, output));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceEvent"></param>
        private void StoreDeviceEvent(IE50DeviceEvent deviceEvent)
        {
            dataConnector.StoreMotionPIREvent(deviceEvent);
        }

        /// <summary>
        /// Handles the Motion Sensor Triggered procedure and returns true if event should be forwarded to (external) Event Manager.
        /// </summary>
        /// <param name="sender">The sender of the motion event</param>
        /// <param name="e">The triggering motion event</param>
        private void SensorTriggered(object sender, InternalEventArgs<IE50DeviceEvent> e)
        {
            var deviceEvent = e.InternalEvent;
            string emid = "";
            bool samesensor = false;
            int timedelay= 0;
            ulong deviceID = dataConnector.GetMotionPIRDevice(deviceEvent.Device.DriverDeviceID).DatabaseHelperDevice.DeviceID;

            if (deviceEvent.DeviceType.DeviceTypeID == "MOTION_SENSOR")
            {
                if (deviceEvent.Message == "true")
                {
                    var timedout = CheckDeviceTimeOut(deviceID);
                    if (!timedout)
                    {
                        samesensor = CheckSameSensor(deviceID);
                        if (samesensor)
                        {
                            timedelay = GetTimeDelay(deviceEvent.IEHeader.TimeStamp);
                            try
                            {
                                if (timedelay >
                                    Int32.Parse(Configuration.GetValue("AI_Store_Motion_PIR_Delay").ToString()))
                                    StoreDeviceEvent(deviceEvent);
                                emid = Guid.NewGuid().ToString();
                            }
                            catch (Exception ex)
                            {
                                return;
                            }
                        }
                        else
                        {
                            StoreDeviceEvent(deviceEvent);
                            emid = Guid.NewGuid().ToString();
                        }
                    }
                    CreateAIEvent(deviceEvent, timedout, emid, samesensor, timedelay, GetPeopleInBuilding());
                    ieManager.CreateEMDeviceEvent(deviceEvent, emid, "MOTION");         // ToDo: Change this so that it raises an EMevent in the IEManager
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetPeopleInBuilding()
        {
            return 1;               // ToDo: Link this to datastore somehow
        }
    }
}