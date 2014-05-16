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
//  File            :   Devices.cs                                      //
//  Description     :   Contains the data of all devices.               //
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

using LyvinDataStoreLib.LyvinDeviceData.DatabaseHelperObjects;
using LyvinDataStoreLib.Models;
using LyvinObjectsLib.Devices;
using LyvinSystemLogicLib;

namespace LyvinDataStoreLib.LyvinDeviceData
{
    /// <summary>
    /// Contains the data of all devices.
    /// </summary>
    public class DeviceData
    {

        public AudioOutputData AudioOutput { get; set; }
        public AVInputData AVInput { get; set; }
        public LightingData Lighting { get; set; }
        public SecurityData Security { get; set; }
        public SensorData Sensors { get; set; }
        public SwitchData Switches { get; set; }
        public VideoOutputData VideoOutput { get; set; }
        public DeviceTimeOutData DeviceTimeOuts { get; set; }

        public DeviceData()
        {
            AudioOutput = new AudioOutputData();
            AVInput = new AVInputData();
            Lighting = new LightingData();
            Security = new SecurityData();
            Sensors = new SensorData();
            Switches = new SwitchData();
            VideoOutput = new VideoOutputData();
            DeviceTimeOuts = new DeviceTimeOutData();
            InitializeDatabase();
            Initialize();
        }

        private void Initialize()
        {
            switch (Configuration.GetValue("DeviceData_Primary_DataStore", "string").ToString())
            {
                    // When primary store is database and the db is connected nothing needs to be loaded, because it will be loaded when devices are discovered.
                case "Database":
                    bool connected;
                    bool.TryParse(Configuration.GetValue("Database_Connected", "bool").ToString(), out connected);

                    if (!connected)
                        LoadFromXML();
                    break;
                case "XML":
                    LoadFromXML();
                    break;
                default:
                    break;
            }
        }

        private void InitializeDatabase()
        {
            using (var lyvinsdb = new Database("lyvinsdb"))
            {
                foreach (var device in lyvinsdb.Fetch<DatabaseHelperDevice>("SELECT * FROM device"))
                {
                    device.Status = "INACTIVE";
                    lyvinsdb.Update(device);
                }
            }
        }

        /// <summary>
        /// Used to save all device related data to xml files if not running with a database.
        /// </summary>
        public void SaveToXML()
        {
            // ToDo: Figure out how to best save everything to xml after database implementation is done.

            /*
            XMLParser.WriteXMLFromObject<AudioOutputData>(
                Configuration.GetValue("Audio_Output_DeviceData_XML_File", "string").ToString(), "", AudioOutput, false,
                true);
            XMLParser.WriteXMLFromObject<AVInputData>(
                Configuration.GetValue("AV_Input_DeviceData_XML_File", "string").ToString(), "", AVInput, false, true);
            XMLParser.WriteXMLFromObject<LightingData>(
                Configuration.GetValue("Lighting_DeviceData_XML_File", "string").ToString(), "", Lighting, false, true);
            XMLParser.WriteXMLFromObject<SecurityData>(
                Configuration.GetValue("Security_DeviceData_XML_File", "string").ToString(), "", Security, false, true);
            XMLParser.WriteXMLFromObject<SensorData>(
                Configuration.GetValue("Sensor_DeviceData_XML_File", "string").ToString(), "", Sensors, false, true);
            XMLParser.WriteXMLFromObject<SwitchData>(
                Configuration.GetValue("Switch_DeviceData_XML_File", "string").ToString(), "", Switches, false, true);
            XMLParser.WriteXMLFromObject<VideoOutputData>(
                Configuration.GetValue("Video_Output_DeviceData_XML_File", "string").ToString(), "", VideoOutput, false,
                true);
            XMLParser.WriteXMLFromObject<DeviceTimeOutData>(
                Configuration.GetValue("Device_TimeOut_Data_XML_File", "string").ToString(), "", DeviceTimeOuts, false,
                true);*/
        }

        /// <summary>
        /// Used to load all device related data from xml files if not running with database
        /// </summary>
        public void LoadFromXML()
        {
            // ToDo:figure out how to best load everything from the xml after database implementation

            /*if (File.Exists(Configuration.GetValue("Audio_Output_DeviceData_XML_File", "string").ToString()))
            {
                AudioOutput = XMLParser.GetObjectFromFile<AudioOutputData>(
                    Configuration.GetValue("Audio_Output_DeviceData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                AudioOutput = new AudioOutputData();
            }

            if (File.Exists(Configuration.GetValue("AV_Input_DeviceData_XML_File", "string").ToString()))
            {
                AVInput = XMLParser.GetObjectFromFile<AVInputData>(
                    Configuration.GetValue("AV_Input_DeviceData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                AVInput = new AVInputData();
            }

            if (File.Exists(Configuration.GetValue("Lighting_DeviceData_XML_File", "string").ToString()))
            {
                Lighting = XMLParser.GetObjectFromFile<LightingData>(
                    Configuration.GetValue("Lighting_DeviceData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                Lighting = new LightingData();
            }

            if (File.Exists(Configuration.GetValue("Security_DeviceData_XML_File", "string").ToString()))
            {
                Security = XMLParser.GetObjectFromFile<SecurityData>(
                    Configuration.GetValue("Security_DeviceData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                Security = new SecurityData();
            }

            if (File.Exists(Configuration.GetValue("Sensor_DeviceData_XML_File", "string").ToString()))
            {
                Sensors = XMLParser.GetObjectFromFile<SensorData>(
                    Configuration.GetValue("Sensor_DeviceData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                Sensors = new SensorData();
            }

            if (File.Exists(Configuration.GetValue("Switch_DeviceData_XML_File", "string").ToString()))
            {
                Switches = XMLParser.GetObjectFromFile<SwitchData>(
                    Configuration.GetValue("Switch_DeviceData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                Switches = new SwitchData();
            }

            if (File.Exists(Configuration.GetValue("Video_Output_DeviceData_XML_File", "string").ToString()))
            {
                VideoOutput = XMLParser.GetObjectFromFile<VideoOutputData>(
                    Configuration.GetValue("Video_Output_DeviceData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                VideoOutput = new VideoOutputData();
            }

            if (File.Exists(Configuration.GetValue("Device_TimeOut_Data_XML_File", "string").ToString()))
            {
                DeviceTimeOuts = XMLParser.GetObjectFromFile<DeviceTimeOutData>(
                    Configuration.GetValue("Device_TimeOut_Data_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                DeviceTimeOuts = new DeviceTimeOutData();
            }*/
        }

        /*public void LoadFromDatabase()
        {
            AudioOutput.LoadFromDatabase();
            AVInput.LoadFromDatabase();
            Lighting.LoadFromDatabase();
            Security.LoadFromDatabase();
            Sensors.LoadFromDatabase();
            Switches.LoadFromDatabase();
            VideoOutput.LoadFromDatabase();
            DeviceTimeOuts.LoadFromDatabase();
        }*/

        public string GetDeviceType(ulong deviceID)
        {
            using (var lyvinsdb = new Database("lyvinsdb"))
            {
                var device = lyvinsdb.SingleOrDefault<DatabaseHelperDevice>(deviceID);
                return device == null ? "Error" : device.Type;
            }
        }

        /*private void SaveToDatabase()
        {
            AudioOutput.SaveToDatabase();
            AVInput.SaveToDatabase();
            Lighting.SaveToDatabase();
            Security.SaveToDatabase();
            Sensors.SaveToDatabase();
            Switches.SaveToDatabase();
            VideoOutput.SaveToDatabase();
            DeviceTimeOuts.SaveToDatabase();
        }*/

        /// <summary>
        /// Initializes and adds a device
        /// </summary>
        /// <param name="device">The device to be added</param>
        /// <returns>Returns 1 if successful, otherwise 0</returns>
        public void DiscoveredDevice(LyvinDevice device)
        {
            if (device != null)
            {
                InitializeDevice(device);
            }
            else
            {
                // ToDo: Throw error
            }
            
        }

        /// <summary>
        /// Initializes a device, analyzing its type and converting it to the proper devicetype. Also adds it to the proper device type list.
        /// </summary>
        /// <param name="device">The device to be initialized</param>
        private void InitializeDevice(LyvinDevice device)
        {
            switch (device.Type)
            {
                case "HUB":
                    break;
                case "LIGHT":
                    break;
                case "COLORED_LIGHT":
                    break;
                case "DIMMABLE_DEVICE":
                    break;
                case "MOTION_SENSOR":
                    var motionpir = Sensors.DiscoveredMotionPIRSensorDevice(device);
                    DeviceTimeOuts.DeviceDiscovered(motionpir);
                    break;
                case "TEMPERATURE_SENSOR":
                    break;
                case "LIGHT_SENSOR":
                    break;
                default:
                    break;
            }
            if (device.DeviceList != null)
            {
                foreach (var d in device.DeviceList)
                {
                    InitializeDevice(d);
                }
            }
        }

        /*private void ClearDatabase()
        {
            bool connected;
            bool.TryParse(Configuration.GetValue("Database_Connected", "bool").ToString(), out connected);
            if (connected)
            {
                //Clearing Database
                using (var lyvinDB = new Database("lyvinsdb"))
                {
                    foreach (var d in lyvinDB.Fetch<DeviceTimeOut>("SELECT * FROM devicetimeout"))
                    {
                        lyvinDB.Delete(d);
                    }

                    //Clearing all device related tables
                    foreach (var d in lyvinDB.Fetch<DeviceInRoom>("SELECT * FROM deviceinroom"))
                    {
                        lyvinDB.Delete(d);
                    }

                    foreach (var d in lyvinDB.Fetch<DeviceInZone>("SELECT * FROM deviceinzone"))
                    {
                        lyvinDB.Delete(d);
                    }
                    
                    foreach (var d in lyvinDB.Fetch<MotionPIRSensor>("SELECT * FROM motionpirsensordevice"))
                    {
                        lyvinDB.Delete(d);
                    }

                    foreach (var d in lyvinDB.Fetch<OpenCloseSensor>("SELECT * FROM openclosesensordevice"))
                    {
                        lyvinDB.Delete(d);
                    }

                    foreach (var d in lyvinDB.Fetch<DeviceUniqueIdentifier>("SELECT * FROM deviceuniqueidentifier"))
                    {
                        lyvinDB.Delete(d);
                    }
                }
            }
        }*/
    }
}