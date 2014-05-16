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
//  File            :   DevicePreUpdateReplyBody.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinDeviceAPIContracts                                     //
//  Created on      :   16-5-2014                                        //
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
using System.Runtime.Serialization;

namespace LyvinDeviceAPIContracts.DeviceAPIMessages
{
    [DataContract]
    public class DevicePreUpdateReplyBody
    {

        [DataMember]
        public bool Direct_Control { get; set; }

        [DataMember]
        public string EM_ID { get; set; }

        [DataMember]
        public List<DeviceAPIError> Errors { get; set; }

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public List<DeviceAPIWarning> Warnings { get; set; }

        public DevicePreUpdateReplyBody()
        {
            Errors = new List<DeviceAPIError>();
            Warnings = new List<DeviceAPIWarning>();
        }

        public DevicePreUpdateReplyBody(bool direct_control, string em_ID, List<DeviceAPIError> errors, int priority,
                                        List<DeviceAPIWarning> warnings)
        {
            Direct_Control = direct_control;
            EM_ID = em_ID;
            Errors = errors;
            Priority = priority;
            Warnings = warnings;
        }

    }
}