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
//  File            :   ErrorItem.cs                                    //
//  Description     :   Contains error item data                        //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   18-04-2013                                      //
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

using System.Xml.Serialization;

namespace LyvinSystemLogicLib
{
    /// <summary>
    /// Contains error item data
    /// </summary>
    [XmlRoot("Error")]
    public class ErrorItem
    {

        public ErrorItem()
        {

        }

        public ErrorItem(string id, string description, bool fatal)
        {
            ID = id;
            Description = description;
            Fatal = fatal;
            Specifics = "";
            Handled = false;
        }

        public ErrorItem(string id, string description, bool fatal, string specifics, bool handled)
        {
            ID = id;
            Description = description;
            Specifics = specifics;
            Handled = handled;
            Fatal = fatal;
        }

        ~ErrorItem()
        {

        }

        public virtual void Dispose()
        {

        }

        [XmlAttribute("SensorID")]
        public string ID { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }

        [XmlAttribute("Fatal")]
        public bool Fatal { get; set; }

        public string Specifics { get; set; }

        public bool Handled { get; set; }
    }
}