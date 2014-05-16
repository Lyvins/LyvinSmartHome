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
//  File            :   ZoneData.cs                                     //
//  Description     :   Contains all zone related data                  //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   27-3-2014                                       //
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
//  ToDo: Loading                                                       //
//  ToDo: Commenting                                                    //
//                                                                      //
//----------------------------------------------------------------------//

using System.Collections.Generic;
using System.Linq;
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Contains all zone related data
    /// </summary>
    public class ZoneData
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Zone> Zones { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ZoneData()
        {
            Zones = new List<Zone>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        public void AddZone(Zone zone)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                lyvinDB.Save(zone);
                Zones.RemoveAll(z => z.ZoneID == zone.ZoneID);
                Zones.Add(zone);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneid"></param>
        /// <returns></returns>
        public Zone GetZone(ulong zoneid)
        {
            return Zones.Exists(z => z.ZoneID == zoneid) ? Zones.SingleOrDefault(z => z.ZoneID == zoneid) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneid"></param>
        public void RemoveZone(ulong zoneid)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                var zone = lyvinDB.SingleOrDefault<Zone>("SELECT * FROM zone WHERE ZoneID=@0", zoneid);

                if (zone == null)
                    return;

                zone.Status = "REMOVED";
                lyvinDB.Save(zone);
                Zones.RemoveAll(z => z.ZoneID == zoneid);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void LoadZones()
        {
            //ToDo: this
        }
    }
}
