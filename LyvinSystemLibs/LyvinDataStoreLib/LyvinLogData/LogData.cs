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
//  File            :   LogData.cs                                      //
//  Description     :   Represents all log data                         //
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
//  -ToDo: Add database and/or xml functionality                        //
//                                                                      //
//----------------------------------------------------------------------//

using LyvinSystemLogicLib;

namespace LyvinDataStoreLib.LyvinLogData
{
    /// <summary>
    /// Represents log data
    /// </summary>
    public class LogData
    {

        public OccupancyLogData Occupancy { get; set; }
        public SensorLogData Sensors { get; set; }
        public DeviceRawDataLog RawDataLog { get; set; }

        public LogData()
        {
            Occupancy = new OccupancyLogData();
            Sensors = new SensorLogData();
            RawDataLog = new DeviceRawDataLog();
            Load();
        }

        public void Save()
        {
            bool connected;
            bool.TryParse(Configuration.GetValue("Database_Connected", "bool").ToString(), out connected);

            if (connected)
                SaveToDatabase();
            SaveToXML();
        }

        public void Load()
        {
            switch (Configuration.GetValue("LogData_Primary_DataStore", "string").ToString())
            {
                case "Database":
                    bool connected;
                    bool.TryParse(Configuration.GetValue("Database_Connected", "bool").ToString(), out connected);

                    if (connected)
                        LoadFromDatabase();
                    SaveToXML();
                    break;
                case "XML":
                    LoadFromXML();
                    Save();
                    break;
                case "None":
                    break;
            }
        }
            
        public void SaveToXML()
        {
            /*XMLParser.WriteXMLFromObject<OccupancyLogData>(Configuration.GetValue("Occupancy_LogData_XML_File", "string").ToString(), "", Occupancy, false, true);
            XMLParser.WriteXMLFromObject<SensorLogData>(Configuration.GetValue("Sensor_LogData_XML_File", "string").ToString(), "", Sensors, false, true);*/
        }

        public void SaveToDatabase()
        {
            /*Occupancy.SaveToDatabase();
            Sensors.SaveToDatabase();*/
        }

        public void LoadFromXML()
        {
            /*if (File.Exists(Configuration.GetValue("Occupancy_LogData_XML_File", "string").ToString()))
            {
                Occupancy = XMLParser.GetObjectFromFile<OccupancyLogData>(
                    Configuration.GetValue("Occupancy_LogData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                Occupancy = new OccupancyLogData();
            }

            if (File.Exists(Configuration.GetValue("Sensor_LogData_XML_File", "string").ToString()))
            {
                Sensors = XMLParser.GetObjectFromFile<SensorLogData>(
                    Configuration.GetValue("Sensor_LogData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                Sensors = new SensorLogData();
            }*/
        }

        public void LoadFromDatabase()
        {
            /*Occupancy.LoadFromDatabase();
            Sensors.LoadFromDatabase();*/
        }
    }
}