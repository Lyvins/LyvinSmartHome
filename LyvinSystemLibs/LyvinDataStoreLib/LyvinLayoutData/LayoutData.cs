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
//  File            :   Layout.cs                                       //
//  Description     :   Represents all layout data                      //
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
//  ToDo: Commenting and error handling                                 //
//  ToDo: Add Lyvins activities and buildings                           //
//  ToDo: Loading                                                       //
//                                                                      //
//----------------------------------------------------------------------//

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Represents all layout data
    /// </summary>
    public class LayoutData
    {
        /// <summary>
        /// 
        /// </summary>
        public ActivityData Activities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BuildingData Buildings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ZoneData Zones { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public LayoutData()
        {
            Buildings = new BuildingData();
            Zones = new ZoneData();
            Activities = new ActivityData();
            Load();
        }
        
        public void Load()
        {
            /*
            switch (Configuration.GetValue("LayoutData_Primary_DataStore", "string").ToString())
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
            }*/
        }

        public void SaveToXML()
        {
            /*
            XMLParser.WriteXMLFromObject<ActivityData>(Configuration.GetValue("Activity_LayoutData_XML_File", "string").ToString(), "", Activities, false, true);
            XMLParser.WriteXMLFromObject<BuildingData>(Configuration.GetValue("Building_LayoutData_XML_File", "string").ToString(), "", Buildings, false, true);
            */
        }

        public void LoadFromXML()
        {
            /*
            if (File.Exists(Configuration.GetValue("Activity_LayoutData_XML_File", "string").ToString()))
            {
                Activities = XMLParser.GetObjectFromFile<ActivityData>(
                    Configuration.GetValue("Activity_LayoutData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                Activities = new ActivityData();
                if (Configuration.GetValue("Load_Lyvins_Layout").ToString() == "true")
                    LoadLyvinsActivities();
            }

            if (File.Exists(Configuration.GetValue("Building_LayoutData_XML_File", "string").ToString()))
            {
                Buildings = XMLParser.GetObjectFromFile<BuildingData>(
                    Configuration.GetValue("Building_LayoutData_XML_File", "string").ToString(), "", false, true);
            }
            else
            {
                Buildings = new BuildingData();
                if (Configuration.GetValue("Load_Lyvins_Layout").ToString() == "true")
                    LoadLyvinsBuildings();
            }*/
        }

        public void LoadFromDatabase()
        {
            /*
            Buildings.LoadFromDatabase();
            Activities.LoadFromDatabase();
            */
        }

        public void LoadLyvinsActivities()
        {
            // ToDo: Add all Lyvins activities
        }

        public void LoadLyvinsBuildings()
        {
            /*
            Address lyvinAddress = new Address("Eindhoven", "Netherlands", 115, "", 115, "", "Stratum", "5615CD", "Noord-Brabant", "Aalsterweg", "", 0, 0, 0);
            List<Room> rooms = new List<Room>();
            rooms.Add(new Room(new List<ActivityInRoom>(), new List<AttributeInRoom>(), new List<ConnectedToRoom>(), "The hall", new Dimension(new List<Elevation>(),0,2,5,20,2,0), Guid.NewGuid().ToString(), "Hall", 0));
            Building lyvinhq = new Building(lyvinAddress,"Lyvin HQ", Guid.NewGuid().ToString(), "LyvinHQ", rooms);
            Buildings.AddBuilding(lyvinhq);
             * */
            
            // ToDo: Add all Lyvins buildings
        }
    }
}