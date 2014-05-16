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
//  File            :   Address.cs                                      //
//  Description     :   Represents address data                         //
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

using System;
using LyvinDataStoreLib.Models;

namespace LyvinDataStoreLib.LyvinLayoutData
{
    /// <summary>
    /// Represents address data
    /// </summary>
    [TableName("address")]
    [PrimaryKey("AddressID")]
    public class Address
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong AddressID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Country { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int HouseNumberFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String HouseNumberFromAdd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int HouseNumberTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String HouseNumberToAdd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Neighbourhood { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String PostalCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Street1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Street2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float Y { get; set; }

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
        public Address()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="houseNumberFrom"></param>
        /// <param name="houseNumberFromAdd"></param>
        /// <param name="houseNumberTo"></param>
        /// <param name="houseNumberToAdd"></param>
        /// <param name="neighbourhood"></param>
        /// <param name="postalCode"></param>
        /// <param name="state"></param>
        /// <param name="street1"></param>
        /// <param name="street2"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="buildingid"></param>
        /// <param name="status"></param>
        public Address(string city, string country, int houseNumberFrom, string houseNumberFromAdd,
                           int houseNumberTo, string houseNumberToAdd, string neighbourhood, string postalCode,
                           string state, string street1, string street2, float x, float y, ulong buildingid, string status)
        {
            City = city;
            Country = country;
            HouseNumberFrom = houseNumberFrom;
            HouseNumberFromAdd = houseNumberFromAdd;
            HouseNumberTo = houseNumberTo;
            HouseNumberToAdd = houseNumberToAdd;
            Neighbourhood = neighbourhood;
            PostalCode = postalCode;
            State = state;
            Street1 = street1;
            Street2 = street2;
            X = x;
            Y = y;
            BuildingID = buildingid;
            Status = status;
        }
    }
}