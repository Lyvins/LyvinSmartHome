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
//  File            :   ApplicationManager.cs                                //
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

namespace LyvinObjectsLib.Applications
{
    public class ApplicationManager
    {
        private List<LyvinApplication> applications { get; set; }

        public ApplicationManager()
        {

        }

        /// <summary>
        /// Returns the application with a certain application ID.
        /// </summary>
        /// <param name="applicationID">The application ID of the application to be gotten</param>
        /// <returns>Returns the application containing the application ID. Returns null if application does not exist.</returns>
        public LyvinApplication GetApplication(string applicationID)
        {
            try
            {
                if (applications != null)
                {
                    return applications.Single(a => a.ApplicationID == applicationID);
                }
                else
                {
                    return null;
                }
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

        /// <summary>
        /// Lists all applications
        /// </summary>
        /// <returns>Returns a list of all applications</returns>
        public List<LyvinApplication> ListApplications()
        {
            return applications;
        }

        /// <summary>
        /// Adds an application if it does not already exist
        /// </summary>
        /// <param name="application">The application to be added</param>
        /// <returns>Returns 1 if succesful. Returns 0 if applications is null. Returns 2 if application is already registered.</returns>
        public int RegisterApplication(LyvinApplication application)
        {
            if (applications != null)
            {
                if (applications.Any(a => a.ApplicationID == application.ApplicationID))
                {
                    return 2;
                }
                applications.Add(application);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Removes an application.
        /// </summary>
        /// <param name="applicationID">The id of the application to be removed.</param>
        /// <returns>1 if succesfull. 0 if application does not exist.</returns>
        public int RemoveApplication(string applicationID)
        {
            try
            {
                if (applications != null)
                {
                    if (applications.Remove(applications.Single(a => a.ApplicationID == applicationID)))
                    {
                        return 1;
                    }
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
            catch (ArgumentNullException)
            {
                return 0;
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

    }
}