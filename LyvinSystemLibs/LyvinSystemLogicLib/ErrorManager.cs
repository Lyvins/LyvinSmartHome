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
//  File            :   ErrorManager.cs                                 //
//  Description     :   Manages and handles all errors                  //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   18-04-2013                                       //
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
//  ToDo: Add actual error handling                                     //
//                                                                      //
//----------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LyvinSystemLogicLib
{
    /// <summary>
    /// Manages and handles all errors
    /// </summary>
    public static class ErrorManager
    {

        private static readonly List<ErrorItem> currentErrors;
        private static readonly List<ErrorItem> errorList;
        private static readonly List<ErrorItem> handledErrors;
        private const string ErrorListFileName = "System\\Errors\\ErrorList.xml";

        static ErrorManager()
        {
            currentErrors = new List<ErrorItem>();
            handledErrors = new List<ErrorItem>();
            errorList = new List<ErrorItem>();
            Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorID"></param>
        public static void ErrorIsHandled(string errorID)
        {
            ErrorItem err = currentErrors.Single(e => e.ID == errorID);
            if (err != null)
            {
                err.Handled = true;
                handledErrors.Add(new ErrorItem(err.ID, err.Description, err.Fatal, err.Specifics, err.Handled));
                currentErrors.Remove(err);
                Logger.LogItem("Error: " + err.ID + " has been solved and handled.", LogType.ERROR);
            }
        }

        public static List<ErrorItem> GetCurrentError()
        {
            return currentErrors;
        }

        public static List<ErrorItem> GetHandledError()
        {
            return handledErrors;
        }

        private static void Initialize()
        {
            try
            {
                Logger.LogItem("Initializing the Error Manager.", LogType.SYSTEM);
                Directory.CreateDirectory("System");
                Directory.CreateDirectory("System\\Errors");

                if (File.Exists(ErrorListFileName))
                {
                    var objects = XMLParser.GetObjectsFromFile(typeof (ErrorItem), ErrorListFileName);

                    foreach (var o in objects)
                    {
                        errorList.Add((ErrorItem) o);
                    }
                }
                else
                {
                    Logger.LogItem("Errorlist missing: " + ErrorListFileName + ". Creating default Errorlist.",
                                   LogType.WARNING);

                    CreateDefaultErrorList();
                    WriteErrorXML();
                }
            }
            catch (Exception)
            {
                Logger.LogItem("Exception when Initializing ErrorManager!", LogType.ERROR);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorID"></param>
        /// <param name="specifics"></param>
        public static void InvokeError(string errorID, string specifics)
        {
            ErrorItem temp = errorList.Single(e => e.ID == errorID);

            if (temp != null)
            {
                currentErrors.Add(new ErrorItem(temp.ID, temp.Description, temp.Fatal, specifics, false));
                var message = "Error: " + temp.ID + "\nDescription: " + temp.Description + "\nSpecifics: " +
                              specifics;
                Logger.LogItem(message, LogType.ERROR);
                if (temp.Fatal)
                {
                    Logger.LogItem("Error: " + temp.ID + " is a fatal error. The system will shutdown.", LogType.ERROR);
                    throw (new Exception(message));
                }

                //ToDo: Add code for handling errors, probably via an event

            }
        }

        private static void WriteErrorXML()
        {
            List<object> objects = errorList.Cast<object>().ToList();

            XMLParser.WriteXMLFromObjects(ErrorListFileName, objects, typeof (ErrorItem));
        }

        private static void CreateDefaultErrorList()
        {
            errorList.Add(new ErrorItem("UnknownCriticalError",
                                    "A default error which handles all errors that have no error id.", true));
            errorList.Add(new ErrorItem("MissingFile", "Error indicating a file is missing.", true));
            errorList.Add(new ErrorItem("CorruptFile", "Error indicating a file is corrupt.", true));
        }
    }
}