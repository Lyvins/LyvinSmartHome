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
//  File            :   LyvinWidgetDevice.cs                                //
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
using System.Linq;

namespace LyvinObjectsLib.WidgetDevices
{
    public class LyvinWidgetDevice
    {
        /// <summary>
        /// The system widget of this device
        /// </summary>
        private LyvinWidget systemWidget;

        /// <summary>
        /// A list containing all widgets on this device
        /// </summary>
        private List<LyvinWidget> widgets;

        /// <summary>
        /// A constructor for the widget device
        /// </summary>
        /// <param name="widgetDeviceID">The unique id of the widget device</param>
        /// <param name="widgetDeviceName">The name of the widget device</param>
        /// <param name="description">A description of the widget device</param>
        public LyvinWidgetDevice(string widgetDeviceID, string name, string description)
        {
            WidgetDeviceID = widgetDeviceID;
            Name = name;
            Description = description;
            widgets = new List<LyvinWidget>();
            systemWidget = null;
        }

        /// <summary>
        /// Adds a widget to the widget device
        /// </summary>
        /// <param name="widget">The widget to be added</param>
        public void AddWidget(LyvinWidget widget)
        {
            if (!widgets.Contains(widget))
            {
                widgets.Add(widget);
            }
        }

        /// <summary>
        /// A boolean indicating whether the widget device is currently connected
        /// </summary>
        public bool Connected { get; set; }

        /// <summary>
        /// A description of the widget device
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Lists all widgets in the widget device
        /// </summary>
        /// <returns>A list of all widgets</returns>
        public List<LyvinWidget> ListWidgets()
        {
            return widgets;
        }

        /// <summary>
        /// Removes a widget from the widget device
        /// </summary>
        /// <param name="widgetID">The id of the widget to be removed</param>
        public void RemoveWidget(string widgetID)
        {
            if (widgets.Any(w => w.WidgetID == widgetID))
            {
                widgets.Remove(widgets.Single(w => w.WidgetID == widgetID));
            }
        }

        /// <summary>
        /// Sets the system widget of this widget device
        /// </summary>
        /// <param name="widget">The system widget</param>
        public void SetSystemWidget(LyvinWidget widget)
        {
            systemWidget = widget;
            systemWidget.SystemWidget = true;
        }

        /// <summary>
        /// The unique id of the widget device
        /// </summary>
        public string WidgetDeviceID { get; set; }

        /// <summary>
        /// The name of the widget device.
        /// </summary>
        public string Name { get; set; }

    }
}