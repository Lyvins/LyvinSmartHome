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
//  File            :   Logger.cs                                //
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
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LyvinSystemLogicLib
{
    public static class Logger
    {

        private static Form lyvinUI = null;

        private static LogType level;
        private static string logFileName;

        private static Queue<XElement> logQueue;
        private static DateTime LastFlushed = DateTime.Now;

        private static Queue<string> formQueue;

        static Logger()
        {
            logQueue = new Queue<XElement>();
            formQueue = new Queue<string>();
            Initialize();
        }

        /// <summary>
        /// Initializes the class and opens the filestream or database connection.
        /// </summary>
        private static void Initialize()
        {
            // Default logfile will always be Logfile.log
            logFileName = "Logfile.log";

            // Default loglevel will be set to ALL
            level = LogType.ALL;

            LogItem("The logger has been initialized.", LogType.NOTICE);
        }

        /// <summary>
        /// Property to override the default loglevel
        /// </summary>
        public static LogType LogLevel
        {
            get { return level; }
            set
            {
                level = value;
                LogItem("The loglevel has been set to: " + LogLevel.ToString(), LogType.NOTICE);
            }
        }

        /// <summary>
        /// Property to override the default logfilename
        /// </summary>
        public static string LogFileName
        {
            get { return logFileName; }
            set
            {
                logFileName = value;
                LogItem("The filename of the log has been set to: " + logFileName, LogType.NOTICE);
            }
        }

        public static Form LyvinUI
        {
            set { lyvinUI = value; }
        }

        /// 
        /// <param name="value"></param>
        /// <param name="type"></param>
        public static void LogItem(string value, LogType severity)
        {
            if (level < severity) return;
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                                     CultureInfo.InvariantCulture);
            //string timeStamp = DateTime.Now.ToString();

            XElement xmlEntry = new XElement("logEntry",
                                             new XElement("Severity", severity),
                                             new XElement("Timestamp", timeStamp),
                                             new XElement("Message", value));


            lock (logQueue)
            {
                formQueue.Enqueue("[" + severity + "] " + timeStamp + ": " + value);
                LogToForm();
            }

            // Lock the queue while writing to prevent contention for the log file
            lock (logQueue)
            {
                logQueue.Enqueue(xmlEntry);

                // If we have reached the Queue Size then flush the Queue
                if (logQueue.Count >= 10 || DoPeriodicFlush())
                {
                    FlushLog();
                }
            }

            if (LogLevel == LogType.DEBUG)
            {
                Console.WriteLine("[" + severity + "] " + timeStamp + ": " + value);
            }
        }

        private static bool DoPeriodicFlush()
        {
            TimeSpan logAge = DateTime.Now - LastFlushed;
            if (logAge.TotalSeconds >= 10)
            {

                LastFlushed = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        public static void FlushLog()
        {
            while (logQueue.Count > 0)
            {
                XElement entry = logQueue.Dequeue();

                using (StreamWriter log = new StreamWriter(logFileName, true))
                {
                    log.WriteLine(entry);
                    log.Close();
                }
            }
        }

        private static void LogToForm()
        {
            if (lyvinUI != null)
            {
                var autoscroll = false;
                if (lyvinUI.Controls.ContainsKey("checkBoxAutoScroll"))
                {
                    autoscroll = ((CheckBox) lyvinUI.Controls["checkBoxAutoScroll"]).Checked;
                }
                if (lyvinUI.Controls.ContainsKey("logListBox"))
                {
                    while (formQueue.Count > 0)
                    {
                        string item = formQueue.Dequeue();
                        var logListBox = (ListBox) lyvinUI.Controls["logListBox"];
                        if (logListBox.InvokeRequired)
                        {
                            // after we've done all the processing, 
                            logListBox.Invoke(new MethodInvoker(delegate
                                                                    {
                                                                        logListBox.Items.Add(item);
                                                                        if (autoscroll)
                                                                        {
                                                                            logListBox.SelectedIndex =
                                                                                logListBox.Items.Count - 1;
                                                                            logListBox.SelectedIndex = -1;
                                                                        }
                                                                    }));
                        }
                        else
                        {
                            logListBox.Items.Add(item);
                            if (autoscroll)
                            {
                                logListBox.SelectedIndex = logListBox.Items.Count - 1;
                                logListBox.SelectedIndex = -1;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// A more precisely implementation of some DateTime properties on mobile devices.
    /// </summary>
    /// <devdoc>Tested on a HTC Touch Pro2.</devdoc>
    public static class DateTimePrecisely
    {
        /// <summary>
        /// Remembers the start time when this model was created.
        /// </summary>
        private static DateTime _start = DateTime.Now;

        /// <summary>
        /// Remembers the system uptime ticks when this model was created. This
        /// serves as a more precise time provider as DateTime.Now can do.
        /// </summary>
        private static int _startTick = Environment.TickCount;

        /// <summary>
        /// Gets a DateTime object that is set exactly to the current date and time on this computer, expressed as the local time.
        /// </summary>
        /// <returns></returns>
        public static DateTime Now
        {
            get { return _start.AddMilliseconds(Environment.TickCount - _startTick); }
        }

        public static void Reset()
        {
            TimeSpan logAge = DateTime.Now - _start;
            if (logAge.TotalHours >= 1)
            {
                _start = DateTime.Now;
                _startTick = Environment.TickCount;
            }
        }
    }
}