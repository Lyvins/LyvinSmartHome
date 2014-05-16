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
//  File            :   Configuration.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinSystemLogicLib                                     //
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
using System.IO;
using System.Linq;

namespace LyvinSystemLogicLib
{
    public static class Configuration
    {

        private static readonly List<Var> varKeeper;

        private const string configFileName = "Config\\Config.xml";

        public static bool Exists(string name)
        {
            return varKeeper.Exists(v => v.Name == name);
        }

        public static object GetValue(string name)
        {
            Var temp = varKeeper.Single(var => var.Name == name);
            if (temp != null)
            {
                return temp.Value;
            }
            return null;
        }

        public static object GetValue(string name, string type)
        {
            Var temp = varKeeper.Single(var => ((var.Name == name) && (var.Type == type)));
            if (temp != null)
            {
                return temp.Value;
            }
            return null;
        }

        public static string GetType(string name)
        {
            Var temp = varKeeper.Single(var => var.Name == name);
            if (temp != null)
            {
                return temp.Type;
            }
            return null;
        }

        public static void SetValue(string name, string value)
        {
            try
            {
                if (varKeeper.Exists(v => v.Name == name))
                {
                    var temp = varKeeper.Single(var => var.Name == name);
                    if (temp != null)
                    {
                        temp.Value = value;
                    }
                    else
                    {
                        AddVar(name, "object", value);
                    }
                }
                else
                {
                    AddVar(name, "object", value);
                }
            }
            catch (Exception)
            {
                Logger.LogItem("An error occurred setting a configuration value.", LogType.ERROR);
            }
        }

        public static void RemoveVar(string name)
        {
            Var temp = varKeeper.Single(var => var.Name == name);
            if (temp != null)
                varKeeper.Remove(temp);
            WriteConfig();
        }

        public static void AddVar(string name, string type, string value)
        {
            varKeeper.Add(new Var(name, type, value));
            WriteConfig();
        }

        /// <summary>
        /// Initializes the class and reads configuration from file or database.
        /// </summary>
        private static void Initialize()
        {
            Logger.LogItem("Initializing the configuration.", LogType.SYSTEM);
            Directory.CreateDirectory("Config");

            if (File.Exists(configFileName))
            {
                var objects = XMLParser.GetObjectsFromFile<Var>(configFileName, "", false, true);

                foreach (var o in objects)
                {
                    varKeeper.Add((Var) o);
                }
            }
            else
            {
                Logger.LogItem("Configuration file missing: " + configFileName + ". Using default configuration.",
                               LogType.WARNING);
            }
        }

        public static void SetDefaultConfig(List<Var> varkeeper)
        {
            foreach (var defaultvar in varkeeper.Where(defaultvar => !varKeeper.Exists(v => v.Name == defaultvar.Name)))
            {
                varKeeper.Add(defaultvar);
            }
            WriteConfig();
        }

        /// <summary>
        /// Writes the configuration to database and/or file(s).
        /// </summary>
        private static void WriteConfig()
        {
            XMLParser.WriteXMLFromObjects<Var>(configFileName, "", varKeeper, false, true);
        }

        static Configuration()
        {
            varKeeper = new List<Var>();
            Initialize();
        }

    }
}