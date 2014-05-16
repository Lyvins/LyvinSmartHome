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
//  File            :   Activity.cs                                     //
//  Description     :   Represents the data of an activity              //
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
//  ToDo: Commenting                                                    //
//                                                                      //
//----------------------------------------------------------------------//

using System.Collections.Generic;
using LyvinDataStoreLib.Models;
using LyvinSystemLogicLib;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Represents the data of an activity
    /// </summary>
    [TableName("activity")]
    [PrimaryKey("ActivityID")]
    public class Activity
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong ActivityID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DescriptionLong { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DescriptionShort { get; set; }

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
        public Activity()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="descriptionLong"></param>
        /// <param name="descriptionShort"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        public Activity(string descriptionLong, string descriptionShort, string name, string status)
        {
            DescriptionLong = descriptionLong;
            DescriptionShort = descriptionShort;
            Name = name;
            Status = status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="occupancymodifier"></param>
        public void AddOccupancyModifer(OccupancyModifier occupancymodifier)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                if ((lyvinDB.Exists<Activity>(ActivityID)) && (occupancymodifier.ActivityID == ActivityID))
                {
                    lyvinDB.Save(occupancymodifier);
                }
                else
                {
                    ErrorManager.InvokeError("Database Error","Trying to add occupancymodifier item with invalid foreign key");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<OccupancyModifier> GetOccupancyModifiers()
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                return lyvinDB.Fetch<OccupancyModifier>("SELECT * FROM occupancymodifier WHERE ActivityID=@0", ActivityID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="occupancymodifierid"></param>
        public void RemoveOccupancyModifier(ulong occupancymodifierid)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                var occupancymodifier = lyvinDB.SingleOrDefault<OccupancyModifier>(occupancymodifierid);

                if (occupancymodifier == null)
                    return;
                
                lyvinDB.Delete(occupancymodifier);
            }
        }
    }
}