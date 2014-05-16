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
//  File            :   DeviceType.cs                                //
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
using System.Xml.Serialization;

namespace LyvinObjectsLib.Devices
{
    [XmlRoot("DeviceType")]
    public class DeviceType
    {
        public DeviceType()
        {
            Devices = new List<LyvinDevice>();
        }

        /// <summary>
        /// A constructor for the device type
        /// </summary>
        /// <param name="id">The unique id of the device type</param>
        /// <param name="name">The name of the device type</param>
        /// <param name="description">A description of the device type</param>
        public DeviceType(string id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
            Devices = new List<LyvinDevice>();
        }

        /// <summary>
        /// A description of the device type
        /// </summary>
        [XmlAttribute("Description")]
        public string Description { get; set; }

        /// <summary>
        /// All devices of this device type
        /// </summary>
        [XmlIgnore]
        public List<LyvinDevice> Devices { get; set; }

        /// <summary>
        /// The unique id of the device type
        /// </summary>
        [XmlAttribute("ID")]
        public string ID { get; set; }

        /// <summary>
        /// The name of this device type
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

    }
}