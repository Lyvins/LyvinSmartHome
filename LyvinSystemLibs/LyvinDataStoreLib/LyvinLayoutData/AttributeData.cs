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
//  File            :   AttributeData.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                     //
//  Created on      :   27-3-2014                                        //
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
    /// 
    /// </summary>
    class AttributeData
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Attribute> Attributes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AttributeData()
        {
            Attributes = new List<Attribute>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        public Attribute GetAttribute(ulong attributeID)
        {
            return Attributes.Exists(a => a.AttributeID == attributeID) ? Attributes.SingleOrDefault(a => a.AttributeID == attributeID) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        public void AddAttribute(Attribute attribute)
        {
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                lyvinDB.Save(attribute);
                Attributes.RemoveAll(a => a.AttributeID == attribute.AttributeID);
                if (attribute.Status == "CURRENT")
                    Attributes.Add(attribute);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeid"></param>
        public void RemoveActivity(ulong attributeid)
        {
            Attributes.RemoveAll(a => a.AttributeID == attributeid);

            using (var lyvinDB = new Database("lyvinsdb"))
            {
                var attribute = lyvinDB.SingleOrDefault<Attribute>("SELECT * FROM attribute WHERE AttributeID=@0", attributeid);

                if (attribute == null)
                    return;

                attribute.Status = "REMOVED";
                lyvinDB.Save(attribute);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadAttributes()
        {
            //ToDo: Figure out when to load from db and when from xml......

            Attributes.Clear();
            using (var lyvinDB = new Database("lyvinsdb"))
            {
                foreach (var a in lyvinDB.Fetch<Attribute>("SELECT * FROM attribute WHERE Status=@0", "CURRENT"))
                {
                    Attributes.Add(a);
                }
            }
        }
    }
}
