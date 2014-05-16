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
//  File            :   EventManager.cs                                //
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

namespace LyvinObjectsLib.Events
{
    public class EventManager
    {

        private List<LyvinEvent> currentEvents;

        public EventManager()
        {

        }

        /// <summary>
        /// This function will add the event to the list of current events if its is not already added.
        /// </summary>
        /// <param name="currentEvent">The event to be added to the current events list</param>
        public void AddEvent(LyvinEvent currentEvent)
        {
            if (!currentEvents.Contains(currentEvent))
            {
                currentEvents.Add(currentEvent);
            }
        }

        /// <summary>
        /// Returns a list of all current events with a specific events code
        /// </summary>
        /// <param name="eventCode">The event code of the events to be returned</param>
        /// <returns>A list of events containing the event code</returns>
        public List<LyvinEvent> GetEvents(string eventCode)
        {
            if (currentEvents != null)
            {
                return currentEvents.Where(e => e.Code == eventCode).ToList();
            }
            else
            {
                return new List<LyvinEvent>();
            }
        }

        /// <summary>
        /// not used atm
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// Returns all current events
        /// </summary>
        /// <returns>A list of all current events</returns>
        public List<LyvinEvent> ListEvents()
        {
            return currentEvents;
        }

    }
}