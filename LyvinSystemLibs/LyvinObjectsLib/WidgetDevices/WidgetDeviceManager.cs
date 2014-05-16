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
//  File            :   WidgetDeviceManager.cs                                //
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace LyvinObjectsLib.WidgetDevices
{
    public class WidgetDeviceManager
    {
        /// <summary>
        /// A list of all widget devices
        /// </summary>
        private List<LyvinWidgetDevice> widgetDevices;

        public WidgetDeviceManager()
        {
            widgetDevices = new List<LyvinWidgetDevice>();
        }

        /// <summary>
        /// Returns a specific widget device
        /// </summary>
        /// <param name="widgetDeviceID">The id of the widget device to be returned</param>
        /// <returns>The widget device containing the id, otherwise null</returns>
        public LyvinWidgetDevice GetWidgetDevice(string widgetDeviceID)
        {
            try
            {
                return widgetDevices.Single(w => w.WidgetDeviceID == widgetDeviceID);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public LyvinWidget GetWidget(string WidgetID)
        {
            LyvinWidget widget = null;
            foreach (LyvinWidgetDevice wd in widgetDevices)
            {
                widget = wd.ListWidgets().SingleOrDefault(w => w.WidgetID == WidgetID);
            }
            return widget;
        }

        /// <summary>
        /// Initializes the widgets from XML/database (not implemented yet).
        /// </summary>
        public void Initialize()
        {
            //ToDo: implement this
        }

        /// <summary>
        /// Lists all widget devices
        /// </summary>
        /// <returns>A list containing all widget devices</returns>
        public List<LyvinWidgetDevice> ListWidgetDevices()
        {
            return widgetDevices;
        }

        /// <summary>
        /// Removes a specific widget device
        /// </summary>
        /// <param name="widgetDeviceID">The id of the widget device to be removed</param>
        public void RemoveWidgetDevice(string widgetDeviceID)
        {
            if (widgetDevices.Any(w => w.WidgetDeviceID == widgetDeviceID))
            {
                widgetDevices.Remove(widgetDevices.Single(w => w.WidgetDeviceID == widgetDeviceID));
            }
        }

        /// <summary>
        /// Updates the list of widget devices and widgets (Not implemented)
        /// </summary>
        public void UpdateWidgets()
        {
            
        }

    }
}