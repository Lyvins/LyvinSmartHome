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
//  File            :   LyvinEvent.cs                                //
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

namespace LyvinObjectsLib.Events
{
    public class LyvinEvent
    {
        public LyvinEvent()
        {

        }

        /// <summary>
        /// Basic constructor for adding a new event.
        /// </summary>
        /// <param name="code">The event code for this action</param>
        /// <param name="sourceID">The unique ID of the source of the event</param>
        /// <param name="sourceType">The type of the source of the event (Application, System, Device, etc)</param>
        /// <param name="value">An optional value of the event</param>
        public LyvinEvent(string code, string sourceID, string sourceType, string value)
        {
            Code = code;
            SourceID = sourceID;
            SourceType = sourceType;
            Value = value;
        }

        /// <summary>
        /// The event code for this event.
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
        /// An optional value of the event
        /// </summary>
        public string Value { get; set; }

    }
}