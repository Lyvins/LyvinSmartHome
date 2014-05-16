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
//  File            :   LyvinWidget.cs                                //
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

using LyvinObjectsLib.Users;

namespace LyvinObjectsLib.WidgetDevices
{
    public class LyvinWidget
    {
        /// <summary>
        /// Boolean indicating whether this is a system widget
        /// </summary>
        public bool SystemWidget { get; set; }

        /// <summary>
        /// A constructor for the widget
        /// </summary>
        /// <param name="widgetID">The unique id of the widget</param>
        /// <param name="name">The name of the widget</param>
        /// <param name="description">A description of the widget</param>
        /// <param name="type">The type of the widget</param>
        /// <param name="systemWidget">A boolean indicating whether this is a systemwidget</param>
        public LyvinWidget(string widgetID, string name, string description, string type, bool systemWidget)
        {
            WidgetID = widgetID;
            Name = name;
            Description = description;
            Type = type;
            SystemWidget = systemWidget;
        }

        /// <summary>
        /// The current user of the widget if any
        /// </summary>
        public LyvinUser CurrentUser { get; set; }

        /// <summary>
        /// A description of the widget
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of the widget
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the widget
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The unique id of the widget
        /// </summary>
        public string WidgetID { get; set; }

    }
}