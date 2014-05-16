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
//  File            :   RoomData.cs                                     //
//  Description     :   Represents the data of a room                   //
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
using LyvinDataStoreLib.LyvinDeviceData.DatabaseHelperObjects;
using LyvinDataStoreLib.Models;
using LyvinSystemLogicLib;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Represents the data of a room
    /// </summary>
    [TableName("room")]
    [PrimaryKey("RoomID")]
    public class Room
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong RoomID { get; set; }
        
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
        public ulong BuildingID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Room()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="buildingid"></param>
        /// <param name="status"></param>
        public Room(string description, string name, ulong buildingid, string status)
        {
            Description = description;
            Name = name;
            BuildingID = buildingid;
            Status = status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floor"></param>
        public void SetElevation(int floor)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                var elevation = lyvinDB.SingleOrDefault<Elevation>("SELECT * FROM elevation WHERE RoomID=@0", RoomID);

                if (elevation == null)
                    elevation = new Elevation(floor, RoomID);
                else
                    elevation.Floor = floor;

                lyvinDB.Save(elevation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Elevation GetElevation()
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                return lyvinDB.SingleOrDefault<Elevation>("SELECT * FROM elevation WHERE RoomID=@0", RoomID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="volume"></param>
        public void SetDimensions(int height, int length, int width, float volume)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                var dimension = lyvinDB.SingleOrDefault<Dimension>("SELECT * FROM dimension WHERE RoomID=@0", RoomID);

                if (dimension == null)
                    dimension = new Dimension(height, length, volume, width, RoomID);
                else
                {
                    dimension.Height = height;
                    dimension.Length = length;
                    dimension.Width = width;
                    dimension.Volume = volume;
                }

                lyvinDB.Save(dimension);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dimension GetDimensions()
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                return lyvinDB.SingleOrDefault<Dimension>("SELECT * FROM dimension WHERE RoomID=@0", RoomID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetroomid"></param>
        /// <param name="probability"></param>
        public void ConnectToRoom(ulong targetroomid, float probability)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Room>(RoomID)) && (lyvinDB.Exists<Room>(targetroomid)))
                {
                    var connectedtoroom =
                        lyvinDB.SingleOrDefault<ConnectedToRoom>(
                            "SELECT * FROM connectedtoroom WHERE SourceRoomID=@0 AND TargetRoomID=@1",
                            RoomID, targetroomid);

                    if (connectedtoroom == null)
                        connectedtoroom = new ConnectedToRoom(targetroomid, probability, RoomID);
                    else
                        connectedtoroom.Probability = probability;

                    lyvinDB.Save(connectedtoroom);
                }
                else
                {
                    ErrorManager.InvokeError("Database Error", "Trying to connect non existing rooms");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ConnectedToRoom> GetConnectingRooms()
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                return lyvinDB.Fetch<ConnectedToRoom>("SELECT * FROM connectedtoroom WHERE SourceRoomID=@0", RoomID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomid"></param>
        public void RemoveConnectingRoom(ulong roomid)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Room>(RoomID)) && (lyvinDB.Exists<Room>(roomid)))
                {
                    var connectedtoroom =
                        lyvinDB.SingleOrDefault<ConnectedToRoom>(
                            "SELECT * FROM connectedtoroom WHERE SourceRoomID=@0 AND TargetRoomID=@1",
                            RoomID, roomid);

                    if (connectedtoroom == null)
                        return;

                    lyvinDB.Delete(connectedtoroom);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeid"></param>
        /// <param name="manual"></param>
        /// <param name="probability"></param>
        public void AddAttribute(ulong attributeid, bool manual, float probability)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Room>(RoomID)) && (lyvinDB.Exists<Attribute>(attributeid)))
                {
                    var attributeinroom =
                        lyvinDB.SingleOrDefault<AttributeInRoom>(
                            "SELECT * FROM attributeinroom WHERE RoomID=@0 AND AttributeID=@1",
                            RoomID, attributeid);

                    if (attributeinroom == null)
                        attributeinroom = new AttributeInRoom(attributeid, RoomID, manual, probability);
                    else
                    {
                        attributeinroom.Manual = manual;
                        attributeinroom.Probability = probability;
                    }

                    lyvinDB.Save(attributeinroom);
                }
                else
                {
                    ErrorManager.InvokeError("Database Error",
                                                      "Trying to add attributeinroom item with invalid foreign key");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<AttributeInRoom> GetAttributes()
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                return lyvinDB.Fetch<AttributeInRoom>("SELECT * FROM attributeinroom WHERE RoomID=@0", RoomID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeid"></param>
        public void RemoveAttribute(ulong attributeid)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Room>(RoomID)) && (lyvinDB.Exists<Attribute>(attributeid)))
                {
                    var attributeinroom =
                        lyvinDB.SingleOrDefault<AttributeInRoom>(
                            "SELECT * FROM attributeinroom WHERE RoomID=@0 AND AttributeID=@1",
                            RoomID, attributeid);

                    if (attributeinroom == null)
                        return;

                    lyvinDB.Delete(attributeinroom);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityid"></param>
        /// <param name="manual"></param>
        /// <param name="probability"></param>
        public void AddActivity(ulong activityid, bool manual, float probability)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Room>(RoomID)) && (lyvinDB.Exists<Activity>(activityid)))
                {
                    var activityinroom = lyvinDB.SingleOrDefault<ActivityInRoom>(
                        "SELECT * FROM activityinroom WHERE RoomID=@0 AND ActivityID=@1",
                        RoomID, activityid);

                    if (activityinroom == null)
                        activityinroom = new ActivityInRoom(activityid, RoomID, manual, probability);
                    else
                    {
                        activityinroom.Manual = manual;
                        activityinroom.Probability = probability;
                    }

                    lyvinDB.Save(activityinroom);
                }
                else
                {
                    ErrorManager.InvokeError("Database Error",
                                                      "Trying to add activityinroom item with invalid foreign key");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ActivityInRoom> GetActivities()
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                return lyvinDB.Fetch<ActivityInRoom>("SELECT * FROM activityinroom WHERE RoomID=@0", RoomID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityid"></param>
        public void RemoveActivity(ulong activityid)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Room>(RoomID)) && (lyvinDB.Exists<Activity>(activityid)))
                {
                    var activityinroom = lyvinDB.SingleOrDefault<ActivityInRoom>(
                        "SELECT * FROM activityinroom WHERE RoomID=@0 AND ActivityID=@1",
                        RoomID, activityid);

                    if (activityinroom == null)
                        return;

                    lyvinDB.Delete(activityinroom);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceid"></param>
        /// <param name="probability"></param>
        public void AddDevice(ulong deviceid, float probability)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Room>(RoomID)) && (lyvinDB.Exists<DatabaseHelperDevice>(deviceid)))
                {
                    var deviceinroom =
                        lyvinDB.SingleOrDefault<DeviceInRoom>(
                            "SELECT * FROM deviceinroom WHERE RoomID=@0 AND DeviceID=@1",
                            RoomID, deviceid);

                    if (deviceinroom == null)
                        deviceinroom = new DeviceInRoom(RoomID, probability, deviceid);
                    else
                        deviceinroom.Probability = probability;

                    lyvinDB.Save(deviceinroom);
                }
                else
                {
                    ErrorManager.InvokeError("Database Error",
                                                      "Trying to add deviceinroom item with invalid foreign key");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DeviceInRoom> GetDevices()
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                return lyvinDB.Fetch<DeviceInRoom>("SELECT * FROM deviceinroom WHERE RoomID=@0", RoomID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceid"></param>
        public void RemoveDevice(ulong deviceid)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Room>(RoomID)) && (lyvinDB.Exists<DatabaseHelperDevice>(deviceid)))
                {
                    var deviceinroom =
                        lyvinDB.SingleOrDefault<DeviceInRoom>(
                            "SELECT * FROM deviceinroom WHERE RoomID=@0 AND DeviceID=@1",
                            RoomID, deviceid);

                    if (deviceinroom == null)
                        return;

                    lyvinDB.Delete(deviceinroom);
                }
            }
        }
    }
}