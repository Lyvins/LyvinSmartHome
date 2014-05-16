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
//  File            :   InternalEventArgs.cs                            //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinAILib                                      //
//  Created on      :   16-5-2014                                       //
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

namespace LyvinAILib
{
    /// <summary>
    /// Generic EventArgs class to hold internal event data.
    /// </summary>
    /// <typeparam name="T">Any internal event</typeparam>
    public class InternalEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public T InternalEvent { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalEvent"></param>
        public InternalEventArgs(T internalEvent)
        {
            InternalEvent = internalEvent;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class InternalEventExtensions
    {
        /// <summary>
        /// Tell subscribers, if any, that this event has been raised.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The generic event handler</param>
        /// <param name="sender">this or null, usually</param>
        /// <param name="internalEvent">Whatever you want sent</param>
        public static void Raise<T>(this EventHandler<InternalEventArgs<T>> handler, object sender, T internalEvent)
        {
            if (handler != null)
            {
                handler(sender, new InternalEventArgs<T>(internalEvent));
            }
        }
    }
}
