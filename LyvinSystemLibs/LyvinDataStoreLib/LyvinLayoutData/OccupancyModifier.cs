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
//  File            :   OccupancyModifierData.cs                        //
//  Description     :   Represents Ocupancy modifier data               //
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

using System;
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Represents Ocupancy modifier data
    /// </summary>
    [TableName("occupancymodifier")]
    [PrimaryKey("OccupancyModifierID")]
    public class OccupancyModifier
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong OccupancyModifierID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong ActivityID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OccupancyModifier()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="priority"></param>
        /// <param name="value"></param>
        /// <param name="activityid"></param>
        public OccupancyModifier(string @operator, int priority, float value, ulong activityid)
        {
            Operator = @operator;
            Priority = priority;
            Value = value;
            ActivityID = activityid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public float CalculateOccupancy(float input)
        {
            float output = 0;

            switch (Operator)
            {
                case "Add":
                    output = input + Value;
                    if (output > 100) output = 100;
                    break;
                case "Subtract":
                    output = input - Value;
                    if (output < 0) output = 0;
                    break;
                case "Divide":
                    if (Math.Abs(input - 0.0) < float.Epsilon)
                    {
                        output = 0;
                        break;
                    }
                    output = input/Value;
                    break;
                case "Multiply":
                    output = input*Value;
                    if (output > 100)
                        output = 100;
                    break;
            }
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="absolute"></param>
        public void SetPeople(int max, int min, int absolute)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                var people = lyvinDB.SingleOrDefault<People>("SELECT * FROM people WHERE OccupancyModifierID=@0", OccupancyModifierID);
                
                if (people==null)
                    people = new People(absolute, max, min, OccupancyModifierID);
                else
                {
                    people.Absolute = absolute;
                    people.Max = max;
                    people.Min = min;
                }

                lyvinDB.Save(people);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public People GetPeople()
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                return lyvinDB.SingleOrDefault<People>("SELECT * FROM people WHERE OccupancyModifierID=@0", OccupancyModifierID);
            }
        }
    }
}