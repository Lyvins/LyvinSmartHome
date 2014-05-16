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
//  File            :   ActionManager.cs                                //
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

namespace LyvinObjectsLib.Actions
{
    public class ActionManager
    {
        private List<LyvinAction> currentActions;

        public ActionManager()
        {

        }

        /// <summary>
        /// This function will add the action to the list of current actions if its is not already added.
        /// </summary>
        /// <param name="currentAction">The action to be added to the current actions list</param>
        public void AddAction(LyvinAction currentAction)
        {
            if (!currentActions.Contains(currentAction))
            {
                currentActions.Add(currentAction);
            }
        }


        /// <summary>
        /// This function will get a list of all actions from the list of current actions which have a certain action code, if any.
        /// </summary>
        /// <param name="actionCode">The action code of the actions to be gotten</param>
        /// <returns>Returns a list of actions with the action code, or an empty list if there are no actions with the action code.</returns> 
        public List<LyvinAction> GetActions(string actionCode)
        {
            if (currentActions != null)
            {
                return currentActions.Where(c => c.Code == actionCode).ToList();
            }
            else
            {
                return new List<LyvinAction>();
            }
        }

        /// <summary>
        /// Not used atm.
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// Returns all current actions.
        /// </summary>
        /// <returns>Returns the current actions</returns>
        public List<LyvinAction> ListActions()
        {
            return currentActions;
        }

    }
}