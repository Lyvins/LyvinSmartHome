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
//  File            :   LyvinAction.cs                                //
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

namespace LyvinObjectsLib.Actions
{
    public class LyvinAction
    {
        public LyvinAction()
        {

        }

        /// <summary>
        /// Basic constructor for adding a new action.
        /// </summary>
        /// <param name="code">The action code for this action</param>
        /// <param name="sourceID">The unique ID of the source of the action</param>
        /// <param name="sourceType">The type of the source of the action (Application, System, Device, etc)</param>
        /// <param name="targetID">The unique ID of the target of the action</param>
        /// <param name="targetType">The type of the target of the action (Device, DeviceGroup, DeviceZone, DeviceType, etc)</param>
        /// <param name="value">An optional value of the action</param>
        public LyvinAction(string code, string sourceID, string sourceType, string targetID, string targetType,
                           string value)
        {
            Code = code;
            SourceID = sourceID;
            SourceType = sourceType;
            TargetID = targetID;
            TargetType = targetType;
            Value = value;
        }

        /// <summary>
        /// The action code for this action.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The unique id of the source. Will be applicationID, deviceID, system, etc based on the Type of the action.
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// The type of the source
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// The unique id of the target.
        /// Can be deviceID, deviceGroupID, deviceZoneID, deviceTypeID, applicationID or
        /// system depending on the action Type.
        /// </summary>
        public string TargetID { get; set; }

        /// <summary>
        /// The type of the target
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// The unique user id that initiated the action
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// An optional value of the action
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The unique id of the widget device that first initiated the action.
        /// Will be blank if there is no widget device involved.
        /// </summary>
        public string WidgetDeviceID { get; set; }

        /// <summary>
        /// The unique id of the widget that first initiated the action.
        /// Will be blank if there is no widget involved.
        /// </summary>
        public string WidgetID { get; set; }

    }
}