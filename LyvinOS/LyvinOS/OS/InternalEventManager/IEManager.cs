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
//  File            :   IEManager.cs                                    //
//  Description     :   Creates and manages the Internal Event Managers.//
//                      Also handles all events and requests            //
//                      from devices and the event manager.             //
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
//  ToDo: Log every event to database                                   //
//  ToDo: Turn the IEModules into libraries that subscribe to the IEMgr //
//  ToDo: Move the functions that create an EM event to a seperate      //
//        module and create an em event which can be raised in the IEM  //
//                                                                      //
//----------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.Linq;
using LyvinAILib;
using LyvinAILib.InternalEventMessages;
using LyvinDataStoreLib;
using LyvinDataStoreLib.LyvinLayoutData;
using LyvinDataStoreLib.LyvinLogData;
using LyvinDeviceAPIContracts.DeviceAPIMessages;
using LyvinOS.SystemAPI;
using LyvinObjectsLib.Devices;
using LyvinObjectsLib.Events;
using LyvinSystemLogicLib;

namespace LyvinOS.OS.InternalEventManager
{
    /// <summary>
    /// Creates and manages the Internal Event Managers.
    /// Also handles all events and requests from devices and the event manager.
    /// </summary>
    public class IEManager : IIEManager
    {
        public event EventHandler<InternalEventArgs<IE50DeviceEvent>> IE50DeviceEvent;
        public event EventHandler<InternalEventArgs<IE1000AIEvent>> IE1000AIEvent;
        public event EventHandler<InternalEventArgs<IE1010AIEvent>> IE1010AIEvent;
        public event EventHandler<InternalEventArgs<IE2001AIEvent>> IE2001AIEvent;
        public event EventHandler<InternalEventArgs<IE2000AIEvent>> IE2000AIEvent;


        public event EventHandler<InternalEventArgs<object>> IE100EMEvent;

        private DSManager dsManager;
        private SystemAPIManager systemAPI;
        private MotionPIREventManager motionPIREventManager;
        private OpenCloseEventManager openCloseEventManager;
        private DeviceManager deviceManager;

        private float low, high;

        public IEManager()
        {
            
        }

        public void Initialize(DSManager dsmanager, SystemAPIManager systemapi, DeviceManager devicemanager)
        {
            dsManager = dsmanager;
            deviceManager = devicemanager;
            systemAPI = systemapi;

            motionPIREventManager = new MotionPIREventManager();
            motionPIREventManager.Initialize(this, dsManager);
            openCloseEventManager = new OpenCloseEventManager();
            openCloseEventManager.Initialize(this, dsManager);

            float.TryParse(Configuration.GetValue("AI_Occupancy_Probability_Low").ToString(), out low);
            float.TryParse(Configuration.GetValue("AI_Occupancy_Probability_High").ToString(), out high);

            IE2000AIEvent += new EventHandler<InternalEventArgs<IE2000AIEvent>>(ReceiveAIEvent);
        }


        public bool RaiseDeviceEvent(object deviceevent)
        {
            if (deviceevent is IE50DeviceEvent)
            {
                IE50DeviceEvent.Raise(this, deviceevent as IE50DeviceEvent);
                return true;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aievent"></param>
        /// <returns></returns>
        public bool RaiseAIEvent(object aievent)
        {
            if (aievent is IE1000AIEvent)
            {
                IE1000AIEvent.Raise(this, aievent as IE1000AIEvent);
                return true;
            }
            if (aievent is IE1010AIEvent)
            {
                IE1010AIEvent.Raise(this, aievent as IE1010AIEvent);
                return true;
            }
            if (aievent is IE2000AIEvent)
            {
                IE2000AIEvent.Raise(this, aievent as IE2000AIEvent);
                return true;
            }
            if (aievent is IE2001AIEvent)
            {
                IE2001AIEvent.Raise(this, aievent as IE2001AIEvent);
                return true;
            }
            return false;
        }

        public bool RaiseEMEvent(InternalEventArgs<object> emevent)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceEvent"></param>
        /// <param name="emid"></param>
        /// <param name="changetype"></param>
        public void CreateEMDeviceEvent(IE50DeviceEvent deviceEvent, string emid, string changetype)
        {
            var device = deviceManager.GetDevice(deviceEvent.Device.DriverDeviceID);
            var attributes = new List<DeviceAPIAttribute>
                                                      {
                                                          new DeviceAPIAttribute(changetype,
                                                                                 deviceEvent
                                                                                     .Message,
                                                                                 deviceManager
                                                                                     .GetDeviceValue
                                                                                     (device.ID,
                                                                                      changetype))
                                                      };
            try
            {
                systemAPI.LyvinOSOutputProxy.DeviceChangedEvent(new DeviceChangedEventBody(changetype, deviceEvent.IEHeader.TimeStamp, new DeviceAPIDevice(device.Type, device.DeviceZones.FirstOrDefault().ID, device.ID, "", device.Name), null, device.Type, attributes, emid));
            }
            catch (Exception)
            {
                Logger.LogItem("Internal Event Manager failed to send Motion Event.", LogType.SYSTEMAPI);
            }
            deviceManager.ReceiveDeviceEvent(new LyvinEvent("SENSOR_MOTION", deviceEvent.Device.DriverDeviceID, deviceEvent.DeviceType.DeviceTypeID, deviceEvent.Message));
        }

        private void CreateEMOccupancyEvent(IE2000AIEvent aiEvent, string emid)
        {
            //ToDo: program and send an actual occupancy changed em event
        }

        private string CreateAIOccupancyEvent(string referenceid, ulong roomid, DateTime timestamp, float newoccupancy, float oldoccupancy)
        {
            string emid = Guid.NewGuid().ToString();
            IEHeader header = new IEHeader(Guid.NewGuid().ToString(), referenceid, DateTime.Now, "2001", 1);
            IE2001Input input = new IE2001Input(roomid, timestamp);
            IE2001Output output = new IE2001Output(new IE2001AbsoluteProbability(newoccupancy, oldoccupancy), emid, 1, new IE2001RelativeProbability(DetermineRelativeProbability(newoccupancy), DetermineRelativeProbability(oldoccupancy)));

            IE2001AIEvent.Raise(this, new IE2001AIEvent(header, input, output));
            return emid;
        }

        private void ReceiveAIEvent(object sender, InternalEventArgs<IE2000AIEvent> e)
        {
            IE2000AIEvent aiEvent = e.InternalEvent;
            foreach (var ie2000OutputRoom in aiEvent.Output.TriggeredRooms)
            {
                var building = dsManager.LayoutData.Buildings.Buildings.SingleOrDefault(b => b.Rooms.Exists(r => r.RoomID == ie2000OutputRoom.RoomID));
                if (building != null)
                {
                    var roomdata =
                        building.Rooms.SingleOrDefault(r => r.RoomID == ie2000OutputRoom.RoomID);
                    if (roomdata != null)
                    {
                        var oldOccupancy =
                    dsManager.LogData.Occupancy.GetLastOccupancy(ie2000OutputRoom.RoomID);
                        var newOccupancy = DetermineRelativeProbability(ie2000OutputRoom.UpdatedOccupancy);

                        if (oldOccupancy.ProbabilityRelative != newOccupancy)
                        {
                            UpdateOccupancyProbability(roomdata, ie2000OutputRoom.UpdatedOccupancy);
                            CreateEMOccupancyEvent(aiEvent, CreateAIOccupancyEvent(aiEvent.IEHeader.ID, roomdata.RoomID, aiEvent.IEHeader.TimeStamp, ie2000OutputRoom.UpdatedOccupancy, oldOccupancy.ProbabilityAbsolute));
                        }
                    }
                }
            }

            foreach (var ie2000OutputRoom in aiEvent.Output.ConnectingRooms)
            {
                var building = dsManager.LayoutData.Buildings.Buildings.SingleOrDefault(b => b.Rooms.Exists(r => r.RoomID == ie2000OutputRoom.RoomID));
                if (building != null)
                {
                    var roomdata =
                        building.Rooms.SingleOrDefault(r => r.RoomID == ie2000OutputRoom.RoomID);
                    if (roomdata != null)
                    {
                        var oldOccupancy =
                    dsManager.LogData.Occupancy.GetLastOccupancy(ie2000OutputRoom.RoomID);
                        var newOccupancy = DetermineRelativeProbability(ie2000OutputRoom.UpdatedOccupancy);

                        if (oldOccupancy.ProbabilityRelative != newOccupancy)
                        {
                            UpdateOccupancyProbability(roomdata, ie2000OutputRoom.UpdatedOccupancy);
                            CreateEMOccupancyEvent(aiEvent, CreateAIOccupancyEvent(aiEvent.IEHeader.ID, roomdata.RoomID, aiEvent.IEHeader.TimeStamp, ie2000OutputRoom.UpdatedOccupancy, oldOccupancy.ProbabilityAbsolute));
                        }
                    }
                }
            }
        }

        public void ReceiveDeviceEvent50(IE50DeviceEvent deviceEvent)
        {
            IE50DeviceEvent.Raise(this, deviceEvent);
        }

        public void ReceiveDeviceDiscoveryEvent10()
        {
            // ToDo: Add discovery handling
        }

        public string DetermineRelativeProbability(float probability)
        {
            if (probability > high)
                return "High";
            if (probability < low)
                return "Low";
            return "Medium";
        }


        private void UpdateOccupancyProbability(Room room, float occupancy)
        {
            var item = new OccupancyLog(room, DetermineRelativeProbability(occupancy), occupancy, 1, DateTime.Now);

            dsManager.LogData.Occupancy.LogOccupancy(item);
        }
    }
}