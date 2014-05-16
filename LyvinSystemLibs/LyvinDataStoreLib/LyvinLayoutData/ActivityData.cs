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
//  File            :   ActivityData.cs                                 //
//  Description     :   Contains all Activity Data                      //
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
    /// Contains all Activity Data
    /// </summary>
    public class ActivityData
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Activity> Activities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ActivityData()
        {
            Activities = new List<Activity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityID"></param>
        /// <returns></returns>
        public Activity GetActivity(ulong activityID)
        {
            return Activities.Exists(a => a.ActivityID == activityID) ? Activities.SingleOrDefault(a => a.ActivityID == activityID) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        public void AddActivity(Activity activity)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                lyvinDB.Save(activity);
                Activities.RemoveAll(a => a.ActivityID == activity.ActivityID);
                if (activity.Status == "CURRENT")
                    Activities.Add(activity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityid"></param>
        public void RemoveActivity(ulong activityid)
        {
            Activities.RemoveAll(a => a.ActivityID == activityid);

            using (var lyvinDB = new Database("lyvinsdb"))
            {
                var activity = lyvinDB.SingleOrDefault<Activity>("SELECT * FROM activity WHERE ActivityID=@0",
                                                                 activityid);

                if (activity == null)
                    return;

                activity.Status = "REMOVED";
                lyvinDB.Save(activity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadActivities()
        {
            //ToDo: Figure out when to load from db and when from xml......

            Activities.Clear();
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                foreach (var a in lyvinDB.Fetch<Activity>("SELECT * FROM activity WHERE Status=@0", "CURRENT"))
                {
                    Activities.Add(a);
                }
            }
        }
    }
}
