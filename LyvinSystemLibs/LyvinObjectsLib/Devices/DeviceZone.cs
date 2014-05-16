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
//  File            :   DeviceZone.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinObjectsLib                                     //
//  Created on      :   17-5-2014                                        //
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

using System.Collections.Generic;

namespace LyvinObjectsLib.Devices
{
    public class DeviceZone
    {

        public DeviceZone()
        {
            Devices = new List<LyvinDevice>();
        }

        /// <summary>
        /// A constructor for the device zone
        /// </summary>
        /// <param name="id">The unique ID of the device zone</param>
        /// <param name="name">The name of the device zone</param>
        /// <param name="description">A description of the device zone</param>
        /// <param name="x">The x value of the location of the device zone</param>
        /// <param name="y">The y value of the location of the device zone</param>
        /// <param name="dx">The dX value of the size of the device zone</param>
        /// <param name="dy">The dY value of the size of the device zone</param>
        public DeviceZone(string id, string name, string description, float x, float y, float dx, float dy)
        {
            ID = id;
            Name = name;
            Description = description;
            X = x;
            Y = y;
            dX = dx;
            dY = dy;
            Devices = new List<LyvinDevice>();
        }

        /// <summary>
        /// A description of the device zone
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// All devices in the device zone
        /// </summary>
        public List<LyvinDevice> Devices { get; set; }

        /// <summary>
        /// The dX value of the size of the device zone
        /// </summary>
        public float dX { get; set; }

        /// <summary>
        /// The dY value of the size of the device zone
        /// </summary>
        public float dY { get; set; }

        /// <summary>
        /// The unique id of the device zone
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The name of the device zone
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The X value of the position of the device zone
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The Y value of the position of the device zone
        /// </summary>
        public float Y { get; set; }

    }
}