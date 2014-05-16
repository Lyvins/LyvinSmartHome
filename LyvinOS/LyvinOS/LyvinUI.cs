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
//  File            :   LyvinUI.cs                                      //
//  Description     :   Debug UI                                        //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   01-04-2013                                      //
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
using System.Windows.Forms;
using LyvinSystemLogicLib;

namespace LyvinOS
{
    public partial class LyvinUI : Form
    {
        public LyvinUI()
        {
            InitializeComponent();
        }

        public void LogItem(string item)
        {
            if (logListBox.InvokeRequired)
            {
                // after we've done all the processing, 
                logListBox.Invoke(new MethodInvoker(() => AddItemToListBox(item)));
            }
            else
            {
                AddItemToListBox(item);
            }
        }

        private void AddItemToListBox(string item)
        {
            logListBox.Items.Add(item);

            //The max number of items that the listbox can display at a time
            int numberOfItems = logListBox.ClientSize.Height / logListBox.ItemHeight;

            if (logListBox.TopIndex == logListBox.Items.Count - numberOfItems - 1)
            {
                //The item at the top when you can just see the bottom item
                logListBox.TopIndex = logListBox.Items.Count - numberOfItems + 1;
            }
        }

        private void LyvinUILoad(object sender, EventArgs e)
        {
            Logger.LogItem("LyvinOS has started with a form.", LogType.SYSTEM);
        }

        private void LyvinUIPaint(object sender, PaintEventArgs e)
        {
            Logger.LyvinUI = this;
        }
    }
}