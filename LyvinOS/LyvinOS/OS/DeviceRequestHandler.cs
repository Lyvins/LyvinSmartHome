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
//  File            :   DeviceRequestHandler.cs                         //
//  Description     :   Handles and forwards all device requests        //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   01-10-2013                                      //
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
//  ToDo: Convert all device requests into internal events              //
//  ToDo: Raise these events in the IEManager, and subscribe an IEModule//
//  ToDo: This IEModule raises device action events in the IEManager    //
//  ToDo: Devices subscribe to these action events and handle them      //
//  ToDo: Probably also have devices raise their own internal events    //
//                                                                      //
//----------------------------------------------------------------------//


using System;
using System.Collections.Generic;
using System.Linq;
using LyvinDeviceDriverLib;
using LyvinObjectsLib.Devices;
using LyvinDeviceAPIContracts.DeviceAPIMessages;
using LyvinObjectsLib.Devices.Types;

namespace LyvinOS.OS
{
    /// <summary>
    /// Handles and forwards all device requests
    /// </summary>
    public class DeviceRequestHandler
    {
        private readonly DeviceManager deviceManager;
        
        public DeviceRequestHandler(DeviceManager devicemanager)
        {
            deviceManager = devicemanager;
        }

        public DevicePreUpdateReplyBody DevicePreUpdateRequest(DevicePreUpdateRequestBody request)
        {
            if (request.Direct_Control)
            {
                var device = deviceManager.GetDevice(request.Device.ID);
                var reply = new DevicePreUpdateReplyBody(request.Direct_Control, request.EM_ID, new List<DeviceAPIError>(),
                                                             request.Priority, new List<DeviceAPIWarning>());

                foreach (var attr in request.DeviceTypeAttributes)
                {
                    switch (attr.Name)
                    {
                        case "STATUS":
                            if (device.Driver.Actions.Exists(a => a.Code == "SET_STATUS"))
                            {
                                if (device.Driver.DoAction(
                                    new DeviceAction("SET_STATUS", device.Type, attr.New_Value, device.ID),
                                    device.DeviceSettings) == 0)
                                {
                                    device.Status = attr.New_Value;
                                }
                            }
                            break;
                        case "BRIGHTNESS":
                            if (device.Driver.Actions.Exists(a => a.Code == "SET_BRIGHTNESS"))
                            {
                                if (device.Driver.DoAction(
                                    new DeviceAction("SET_BRIGHTNESS", device.Type, attr.New_Value, device.ID),
                                    device.DeviceSettings) == 0)
                                {
                                    // Make sure the value is set in the device
                                    try
                                    {
                                        ((Lighting)device).Brightness = Convert.ToInt32(attr.New_Value);
                                    }
                                    catch (Exception)
                                    {
                                        
                                    }
                                }
                            }
                            break;
                        case "INTENSITY":
                            if (device.Driver.Actions.Exists(a => a.Code == "SET_BRIGHTNESS"))
                            {
                                if (device.Driver.DoAction(
                                    new DeviceAction("SET_BRIGHTNESS", device.Type, attr.New_Value, device.ID),
                                    device.DeviceSettings) == 0)
                                {
                                    try
                                    {
                                        ((DimmableDevice)device).Intensity = Convert.ToInt32(attr.New_Value);
                                    }
                                    catch (Exception)
                                    {
                                        
                                    }
                                }
                            }
                            break;
                        case "RGB":
                            if (device.Driver.Actions.Exists(a => a.Code == "SET_RGB"))
                            {
                                if (device.Driver.DoAction(
                                    new DeviceAction("SET_RGB", device.Type, attr.New_Value, device.ID),
                                    device.DeviceSettings) == 0)
                                {
                                    // Make sure the value is set in the device
                                    try
                                    {
                                        ((ColoredLighting)device).RGB = attr.New_Value;
                                    }
                                    catch (Exception)
                                    {
                                        
                                    }
                                }
                            }
                            break;
                        case "HUE":
                            if (device.Driver.Actions.Exists(a => a.Code == "SET_HUE"))
                            {
                                if (device.Driver.DoAction(
                                    new DeviceAction("SET_HUE", device.Type, attr.New_Value, device.ID),
                                    device.DeviceSettings) == 0)
                                {
                                    try
                                    {
                                        ((ColoredLighting)device).Hue = Convert.ToInt32(attr.New_Value);
                                    }
                                    catch (Exception)
                                    {
                                        
                                    }
                                }
                            }
                            break;
                        case "SATURATION":
                            if (device.Driver.Actions.Exists(a => a.Code == "SET_SATURATION"))
                            {
                                if (device.Driver.DoAction(
                                    new DeviceAction("SET_SATURATION", device.Type, attr.New_Value, device.ID),
                                    device.DeviceSettings) == 0)
                                {
                                    try
                                    {
                                        ((ColoredLighting)device).Sat = Convert.ToInt32(attr.New_Value);
                                    }
                                    catch (Exception)
                                    {
                                        
                                    }
                                }
                            }
                            break;
                        default:
                            reply.Warnings.Add(new DeviceAPIWarning("stubWarning", "Unsupported attribute"));
                            break;
                    }
                }
                return reply;
            }

            //
            // Actual pre-update code goes here!
            //

            return new DevicePreUpdateReplyBody(request.Direct_Control, request.EM_ID, null, request.Priority, null);
        }

        public DeviceUpdateReplyBody DeviceUpdateRequest(DeviceUpdateRequestBody request)
        {
            var device = deviceManager.GetDevice(request.Device.ID);
            var reply = new DeviceUpdateReplyBody(request.EM_ID, new List<DeviceAPIError>(), "SUCCESS", new List<DeviceAPIWarning>());

            foreach (var attr in request.DeviceTypeAttributes)
            {
                switch (attr.Name)
                {
                    case "STATUS":
                        if (device.Driver.DoAction(
                            new DeviceAction("SET_STATUS", device.Type, attr.New_Value, device.ID),
                            device.DeviceSettings) == 0)
                        {
                            device.Status = attr.New_Value;
                        }
                        break;
                    default:
                        reply.Warnings.Add(new DeviceAPIWarning("stubWarning", "Unsupported attribute"));
                        reply.Status = "WARNING";
                        break;
                }
            }
            return reply;
        }

        public DeviceValueReplyBody DeviceValueRequest(DeviceValueRequestBody request)
        {
            foreach (var attribute in request.DeviceTypeAttributes)
            {
                attribute.New_Value = deviceManager.GetDeviceValue(request.Device.ID, attribute.Name);
            }
            return new DeviceValueReplyBody(request.DeviceAPIApplication, request.Device, request.DeviceTypeAttributes, request.EM_ID, request.Importance);
        }

        public DeviceListReplyBody DeviceListRequest(DeviceListRequestBody request)
        {
            var list = new List<DeviceAPIDevice>();
            var originaldevicelist = new List<LyvinDevice>();
            var done = false;
            
            if (request.DeviceGroups != null)
            {
                if (request.DeviceGroups.Count>0)
                {
                    foreach (var dg in request.DeviceGroups.Select(rdg => deviceManager.GetDeviceGroup(rdg.ID)).Where(dg => dg != null))
                    {
                        originaldevicelist.AddRange(dg.Devices);
                    }
                    done = true;
                }
            }
            if (request.DeviceTypes != null)
            {
                if (request.DeviceTypes.Count > 0)
                {
                    foreach (var dt in request.DeviceTypes.Select(rdt => deviceManager.GetDeviceType(rdt.ID)).Where(dt => dt != null))
                    {
                        originaldevicelist.AddRange(dt.Devices);
                    }
                    done = true;
                }
            }
            if (request.DeviceZones != null)
            {
                if (request.DeviceZones.Count > 0)
                {
                    foreach (var dz in request.DeviceZones.Select(rdz => deviceManager.GetDeviceZone(rdz.ID)).Where(dz => dz != null))
                    {
                        originaldevicelist.AddRange(dz.Devices);
                    }
                    done = true;
                }
            }
            if (done == false)
            {
                foreach (var device in deviceManager.Devices)
                {
                    originaldevicelist.AddRange(deviceManager.GetAllSubDevices(device));
                }
            }

            foreach (var device in originaldevicelist)
            {
                if (list.Exists(d => d.ID == device.ID) == false)
                {
                    var devicezone = device.DeviceZones.FirstOrDefault(dz => dz != null);
                    string location = "", zoneid="";
                    if (devicezone!= null)
                    {
                        location = devicezone.Name;
                        zoneid = devicezone.ID;
                    }
                    list.Add(new DeviceAPIDevice(device.Type, zoneid, device.ID, location, device.Name));
                }
            }
            return new DeviceListReplyBody(request.DeviceAPIApplication, list, request.DeviceGroups, request.DeviceTypes, request.DeviceZones, request.EM_ID, request.Importance);
        }

        public DeviceZoneListReplyBody DeviceZoneListRequest(DeviceZoneListRequestBody request)
        {
            var list = deviceManager.DeviceZones.Select(dz => new DeviceAPIDeviceZone(dz.ID, dz.Name)).ToList();

            return new DeviceZoneListReplyBody(request.DeviceAPIApplication, list, request.EM_ID, request.Importance);
        }

        public DeviceGroupListReplyBody DeviceGroupListRequest(DeviceGroupListRequestBody request)
        {
            var list = deviceManager.DeviceGroups.Select(dg => new DeviceAPIDeviceGroup(dg.ID, dg.Name)).ToList();

            return new DeviceGroupListReplyBody(request.DeviceAPIApplication, list, request.EM_ID, request.Importance);
        }

        public DeviceTypeListReplyBody DeviceTypeListRequest(DeviceTypeListRequestBody request)
        {
            var devicetypes = deviceManager.DeviceTypes.Select(dt => new DeviceAPIDeviceType(dt.ID, dt.Name)).ToList();

            return new DeviceTypeListReplyBody(request.DeviceAPIApplication, devicetypes, request.EM_ID, request.Importance);
        }
    }
}