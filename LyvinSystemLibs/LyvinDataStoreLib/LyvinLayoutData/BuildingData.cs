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
//  File            :   BuildingData.cs                                 //
//  Description     :   Contains all Building Data                      //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   7-3-2014                                        //
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
//  ToDo:   Loading                                                     //
//          Commenting                                                  //
//                                                                      //
//----------------------------------------------------------------------//

using System.Collections.Generic;
using System.Linq;
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Contains all Building Data
    /// </summary>
    public class BuildingData
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Building> Buildings { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public BuildingData()
        {
            Buildings = new List<Building>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingid"></param>
        /// <returns></returns>
        public Building GetBuilding(ulong buildingid)
        {
            return Buildings.Exists(b => b.BuildingID == buildingid) ? Buildings.SingleOrDefault(b => b.BuildingID == buildingid) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="building"></param>
        public void AddBuilding(Building building)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                lyvinDB.Save(building);
                Buildings.RemoveAll(b => b.BuildingID == building.BuildingID);
                if (building.Status == "CURRENT")
                    Buildings.Add(building);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingid"></param>
        public void RemoveBuilding(ulong buildingid)
        {
            Buildings.RemoveAll(b => b.BuildingID == buildingid);

            using (var lyvinDB = new Database("lyvinsdb"))
            {
                var building = lyvinDB.SingleOrDefault<Building>("SELECT * FROM building WHERE BuildingID=@0",
                                                                 buildingid);

                if (building == null)
                    return;

                foreach (var address in lyvinDB.Fetch<Address>("SELECT * FROM address WHERE BuildingID=@0", buildingid))
                {
                    building.RemoveAddress(address.AddressID);
                }

                foreach (var room in lyvinDB.Fetch<Room>("SELECT * FROM room WHERE BuildingID=@0", buildingid))
                {
                    building.RemoveRoom(room.RoomID);
                }

                building.Status = "REMOVED";
                lyvinDB.Save(building);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadFromDatabase()
        {
            // ToDo: Figure this out

            /*Buildings.Clear();

            using (var lyvinDB = new Database("lyvinsdb"))
            {
                foreach (var b in lyvinDB.Query<Building>("SELECT * FROM building"))
                {
                    foreach (var a in lyvinDB.Query<Address>("SELECT * FROM addressdata").Where(a => a.BuildingID == b.BuildingID))
                    {
                        b.Address = a;
                    }

                    b.Rooms.Clear();

                    foreach (var r in lyvinDB.Query<Room>("SELECT * FROM roomdata"))
                    {
                        if (r.BuildingID == b.BuildingID)
                        {
                            b.Rooms.Add(r);
                        }

                        r.Activities.Clear();
                        foreach (var act in lyvinDB.Query<ActivityInRoom>("SELECT * FROM activityinroom").Where(act => act.RoomID == r.RoomID))
                        {
                            r.Activities.Add(act);
                        }

                        r.Attributes.Clear();
                        foreach (var attr in lyvinDB.Query<AttributeInRoom>("SELECT * FROM attributeinroom").Where(attr => attr.RoomID == r.RoomID))
                        {
                            r.Attributes.Add(attr);
                        }

                        r.ConnectedTo.Clear();
                        foreach (var conn in lyvinDB.Query<ConnectedToRoom>("SELECT * FROM connectedtoroom").Where(conn => conn.SourceRoomID == r.RoomID))
                        {
                            r.ConnectedTo.Add(conn);
                        }

                        foreach (var dim in lyvinDB.Query<Dimension>("SELECT * FROM dimensiondata"))
                        {
                            if (dim.RoomID == r.RoomID)
                            {
                                r.Dimensions = dim;
                            }

                            dim.Elevation.Clear();
                            foreach (var el in lyvinDB.Query<Elevation>("SELECT * FROM elevationdata").Where(el => el.DimensionDataID == dim.DimensionDataID))
                            {
                                dim.Elevation.Add(el);
                            }
                        }
                    }
                    Buildings.Add(b);
                }
            }*/
        }
    }
}
