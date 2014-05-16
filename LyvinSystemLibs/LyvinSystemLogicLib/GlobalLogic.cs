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
//  File            :   GlobalLogic.cs                                //
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

using System.Threading;

namespace LyvinSystemLogicLib
{
    public static class GlobalLogic
    {
        public enum LyvinProcess
        {
            LyvinOS, LyvinEM, LyvinAM, LyvinWM

        }

        static GlobalLogic()
        {
            
        }

        public static bool IsRunning(LyvinProcess lp)
        {
            try
            {
                Mutex.OpenExisting(GetProcessRunningID(lp));
            }
            catch(WaitHandleCannotBeOpenedException)
            {
                return false;
            }
            return true;
        }

        public static string GetProcessRunningID(LyvinProcess lp)
        {
            switch (lp)
            {
                case LyvinProcess.LyvinOS:
                    return @"Global\LyvinOS#Running";
                case LyvinProcess.LyvinEM:
                    return @"Global\LyvinEM#Running";
                case LyvinProcess.LyvinWM:
                    return @"Global\LyvinWM#Running";
                case LyvinProcess.LyvinAM:
                    return @"Global\LyvinAM#Running";
                default:
                    return "";
            }
        }
    }
}
