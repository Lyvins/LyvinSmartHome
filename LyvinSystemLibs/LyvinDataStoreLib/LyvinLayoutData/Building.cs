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
//  File            :   Building.cs                                     //
//  Description     :   Represents the data of a building               //
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
//  ToDo:   Commenting                                                  //
//                                                                      //
//----------------------------------------------------------------------//

using System.Collections.Generic;
using LyvinDataStoreLib.Models;
using LyvinSystemLogicLib;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Represents the data of a building
    /// </summary>
    [TableName("building")]
    [PrimaryKey("BuildingID")]
    public class Building
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong BuildingID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Ignore]public List<Room> Rooms { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Ignore]public List<Address> Addresses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Building()
        {
            Rooms = new List<Room>();
            Addresses = new List<Address>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        public Building(string description, string name, string status)
        {
            Description = description;
            Name = name;
            Status = status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        public void AddAddress(Address address)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Building>(address.BuildingID)) && (address.BuildingID == BuildingID))
                {
                    lyvinDB.Save(address);
                    Addresses.RemoveAll(a => a.AddressID == address.AddressID);
                    if (address.State == "CURRENT")
                    {
                        Addresses.Add(address);
                    }
                }
                else
                {
                    ErrorManager.InvokeError("Database Error", "Trying to add address without building");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressid"></param>
        public void RemoveAddress(ulong addressid)
        {
            Addresses.RemoveAll(a => a.AddressID == addressid);

            using (var lyvinDB = new Database("lyvinsdb"))
            {
                var address = lyvinDB.SingleOrDefault<Address>("SELECT * FROM address WHERE AddressID=@0",
                                                                 addressid);

                if (address == null)
                    return;

                address.Status = "REMOVED";
                lyvinDB.Save(address);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        public void AddRoom(Room room)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Building>(room.BuildingID)) && (room.BuildingID == BuildingID))
                {
                    lyvinDB.Save(room);
                    Rooms.RemoveAll(r => r.RoomID == room.RoomID);
                    if (room.Status == "CURRENT")
                    {
                        Rooms.Add(room);
                    }
                }
                else
                {
                    ErrorManager.InvokeError("Database Error", "Trying to add room without building");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rooms"></param>
        public void AddRooms(List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                AddRoom(room);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomid"></param>
        public void RemoveRoom(ulong roomid)
        {
            Rooms.RemoveAll(r => r.RoomID == roomid);

            using (var lyvinDB = new Database("lyvinsdb"))
            {
                var room = lyvinDB.SingleOrDefault<Room>("SELECT * FROM room WHERE RoomID=@0",
                                                                 roomid);

                if (room == null)
                    return;

                room.Status = "REMOVED";
                lyvinDB.Save(room);
            }
        }
    }
}