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
//  File            :   SecurityManager.cs                              //
//  Description     :   Creates and handles all security functionality  //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinOS                                         //
//  Created on      :   05-07-2013                                      //
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
//  ToDo: Go through the file to check what still needs to be done      //
//  and what is potentially deprecated                                  //
//                                                                      //
//----------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.Linq;
using LyvinElevationAPIContracts.ElevationAPIMessages;
using LyvinOS.SystemAPI;
using LyvinObjectsLib.Actions;
using LyvinObjectsLib.Applications;
using LyvinObjectsLib.Devices;
using LyvinObjectsLib.Users;
using LyvinObjectsLib.WidgetDevices;
using LyvinSystemLogicLib;

namespace LyvinOS.OS.Security
{
    /// <summary>
    /// Creates and handles all security functionality
    /// </summary>
    public class SecurityManager
    {
        //private readonly VIMain viMain;
        private ActionManager actionManager;
        private ApplicationManager applicationManager;
        private readonly Dictionary<string, ElevationRequest> currentElevationRequests;
        private readonly DeviceManager deviceManager;
        private readonly ElevationManager elevationManager;
        private readonly PolicyManager policyManager;
        private readonly SystemAPIManager systemAPIManager;
        private readonly UserManager userManager;
        private WidgetDeviceManager widgetDeviceManager;

        public SecurityManager()
        {
            // ToDo: Delete this constructor and use the one below, when everything is set up correctly with the OSmanager
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viMain"></param>
        /// <param name="actionManager"></param>
        /// <param name="applicationManager"></param>
        /// <param name="userManager"></param>
        /// <param name="deviceManager"></param>
        /// <param name="widgetDeviceManager"></param>
        /// <param name="systemAPIManager"></param>
        public SecurityManager(ActionManager actionManager, ApplicationManager applicationManager,
                               UserManager userManager, DeviceManager deviceManager,
                               WidgetDeviceManager widgetDeviceManager,
                               SystemAPIManager systemAPIManager)
        {
            
            this.actionManager = actionManager;
            this.applicationManager = applicationManager;
            this.userManager = userManager;
            this.deviceManager = deviceManager;
            this.widgetDeviceManager = widgetDeviceManager;
            this.systemAPIManager = systemAPIManager;
            elevationManager = new ElevationManager();
            policyManager = new PolicyManager(userManager);

            currentElevationRequests = new Dictionary<string, ElevationRequest>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elevationID"></param>
        /// <param name="elevationResult"></param>
        public void ElevationCleanup(string elevationID, bool elevationResult)
        {
            if (currentElevationRequests.Keys.Contains(elevationID))
            {
                currentElevationRequests.Remove(elevationID);
                Logger.LogItem(
                    "Notice: Elevation " + elevationID + " has been resolved with the result: " +
                    elevationResult.ToString() + ". And it has been cleaned by the system.", LogType.SYSTEM);
            }
            else
            {
                Logger.LogItem("Error: Cannot clean Elevation request - unknown elevationID", LogType.ERROR);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elevationID"></param>
        /// <param name="cleaned"></param>
        public void ElevationCleanupReply(string elevationID, bool cleaned)
        {
            Logger.LogItem("Notice: Elevation " + elevationID + " has been cleaned by a the widget manager.",
                           LogType.SYSTEM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elevationID"></param>
        /// <param name="userStatuses"></param>
        public void ElevationReachableReply(string elevationID, Dictionary<string, string> userStatuses)
        {
            if (currentElevationRequests.Keys.Contains(elevationID))
            {
                var currentRequest = currentElevationRequests[elevationID];
                currentRequest.UsersCanGrant = new List<LyvinUser>();

                foreach (string userid in userStatuses.Keys.Where(userid => userStatuses[userid] == "Reachable"))
                {
                    currentRequest.UsersCanGrant.Add(userManager.GetUser(userid));
                }

                systemAPIManager.LyvinOSOutputProxy.ElevationRequiredRequest(new ElevationRequiredRequestBody());
                    // fix with constructor taking: ElevationID, currentRequest.ActionPolicices, currentRequest.UsersCanGrant,currentRequest.CanGrantSelf);
            }
            else
            {
                Logger.LogItem("Error: Received elevation_reachable_reply with unknown elevationID", LogType.ERROR);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elevationID"></param>
        /// <param name="elevation"></param>
        /// <param name="time"></param>
        /// <param name="permanent"></param>
        public void ElevationRequestReply(string elevationID, bool elevation, int time, bool permanent)
        {
            if (currentElevationRequests.Keys.Contains(elevationID))
            {
                var currentRequest = currentElevationRequests[elevationID];

                if (elevation)
                {
                    elevationManager.AddElevation(new Elevation(currentRequest.ElevationID,
                                                                userManager.GetUser(currentRequest.Action.UserID),
                                                                currentRequest.ActionPolicices, permanent, time));
                    //viMain.ActionAllowed(currentRequest.Action); // ToDo: Move this to the requesthandler
                }
                else
                {
                    //viMain.ActionDenied(currentRequest.Action); // ToDo: Move this to the requesthandler
                }

                systemAPIManager.LyvinOSOutputProxy.ElevationCleanupRequest(new ElevationCleanupRequestBody());
                    //fix with constructor taking: ElevationID, Elevation);

                ElevationCleanup(elevationID, elevation);
            }
            else
            {
                Logger.LogItem("Error: Received Elevation_Request_Reply with unknown elevationID", LogType.ERROR);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elevationID"></param>
        /// <param name="elevation"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        public void ElevationRequiredReply(string elevationID, bool elevation, string userID, bool password)
        {
            if (currentElevationRequests.Keys.Contains(elevationID))
            {
                var currentRequest = currentElevationRequests[elevationID];
                if (!elevation)
                {
                    //viMain.ActionDenied(currentRequest.Action); // ToDo: Move this to the requesthandler
                    currentElevationRequests.Remove(currentRequest.ElevationID);
                }
                else
                {
                    var devices = new List<LyvinDevice>();
                    switch (currentRequest.Action.TargetType)
                    {
                        case "Device":
                            devices.Add(deviceManager.GetDevice(currentRequest.Action.TargetID));
                            break;
                        case "DeviceGroup":
                            devices.AddRange(deviceManager.GetDeviceGroup(currentRequest.Action.TargetID).Devices);
                            break;
                        case "DeviceZone":
                            devices.AddRange(deviceManager.GetDeviceZone(currentRequest.Action.TargetID).Devices);
                            break;
                        case "DeviceType":
                            devices.AddRange(deviceManager.GetDeviceType(currentRequest.Action.TargetID).Devices);
                            break;
                    }
                    if (password)
                    {
                        systemAPIManager.LyvinOSOutputProxy.SystemPassRequiredRequest(
                            new SystemPassRequiredRequestBody());
                            //fix with constructor taking: ElevationID, applicationManager.GetApplication(currentRequest.Action.SourceID), widgetDeviceManager.GetWidgetDevice(currentRequest.Action.WidgetDeviceID), widgetDeviceManager.GetWidget(currentRequest.Action.WidgetID), currentRequest.ActionPolicices, userManager.GetUser(currentRequest.Action.UserID), UserID, devices);
                    }
                    else
                    {
                        systemAPIManager.LyvinOSOutputProxy.ElevationRequestRequest(new ElevationRequestRequestBody());
                            // fix with constructor taking: ElevationID, applicationManager.GetApplication(currentRequest.Action.SourceID), widgetDeviceManager.GetWidgetDevice(currentRequest.Action.WidgetDeviceID), widgetDeviceManager.GetWidget(currentRequest.Action.WidgetID), currentRequest.ActionPolicices, userManager.GetUser(currentRequest.Action.UserID), UserID, devices);
                    }
                }
            }
            else
            {
                Logger.LogItem("Error: Received elevation_required_reply with unknown elevationID", LogType.ERROR);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elevationRequest"></param>
        /// <returns></returns>
        public int RequestForElevation(ElevationRequest elevationRequest)
        {
            bool canGrantSelf = false;

            if (userManager.CheckUserPermission(elevationRequest.Action.UserID, "GRANT_ALL_POLICIES"))
            {
                if (!(bool) Configuration.GetValue("REQUIRE_PERSONAL_ELEVATION", "bool")) return 1;
            }

            var usersCanGrant = policyManager.UsersCanGrantPolicies(elevationRequest.ActionPolicices);

            if (usersCanGrant.Any(u => u.UserID == elevationRequest.Action.UserID))
            {
                usersCanGrant.Remove(usersCanGrant.Single(u => u.UserID == elevationRequest.Action.UserID));
                canGrantSelf = true;
            }

            elevationRequest.UsersCanGrant = usersCanGrant;
            elevationRequest.CanGrantSelf = canGrantSelf;

            systemAPIManager.LyvinOSOutputProxy.ElevationReachableRequest(new ElevationReachableRequestBody());
                //fix with constructor taking: elevationRequest.ElevationID, elevationRequest.UsersCanGrant);
            return 2;
        }

        private static string CreateElevationID(LyvinAction action)
        {
            return DateTime.Now + action.Code + action.UserID; // Probably change this to a GUID
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public int SecurityCheck(LyvinAction action)
        {
            var actionPolicies = CreateActionPolicies(action);
            if ((bool) Configuration.GetValue("ALLOW_ELEVATION", "bool"))
            {
                foreach (var actionPolicy in actionPolicies)
                {
                    if (elevationManager.CheckCurrentElevation(action.UserID, actionPolicy))
                    {
                        actionPolicies.Remove(actionPolicy);
                    }
                }
            }

            if (actionPolicies.Count == 0)
            {
                return 1;
            }
            var denied = false;
            foreach (var actionPolicy in actionPolicies)
            {
                if (policyManager.CheckGlobalPolicy(actionPolicy)) denied = true;
                if (policyManager.CheckUserPolicy(actionPolicy, action.UserID)) denied = true;

                foreach (var userGroup in userManager.ListUserGroups())
                {
                    if (userGroup.ListUsers().Any(u => u.UserID == action.UserID))
                    {
                        if (policyManager.CheckUserGroupPolicy(actionPolicy, userGroup.UserGroupID)) denied = true;
                    }
                }
            }

            if (!denied) return 1;
            if (!(bool) Configuration.GetValue("ALLOW_INTERACTIVE_ELEVATION", "bool")) return 0;
            var elevationRequest = new ElevationRequest(CreateElevationID(action), action, new List<LyvinUser>(),
                                                        actionPolicies, false);
            currentElevationRequests.Add(elevationRequest.ElevationID, elevationRequest);
            return RequestForElevation(elevationRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elevationID"></param>
        /// <param name="passwordHash"></param>
        public void SystemPassRequiredReply(string elevationID, string passwordHash)
        {
            if (currentElevationRequests.Keys.Contains(elevationID))
            {
                var currentRequest = currentElevationRequests[elevationID];

                if ((string) Configuration.GetValue("SystemPassHash") == passwordHash)
                {
                    //viMain.ActionAllowed(currentRequest.Action); // ToDo: Move this to the requesthandler
                    ElevationCleanup(elevationID, true);
                }
                else
                {
                    //viMain.ActionDenied(currentRequest.Action); // ToDo: Move this to the requesthandler
                    ElevationCleanup(elevationID, false);
                }
            }
            else
            {
                Logger.LogItem("Error: Received System_Pass_Required_Request_Reply with unknown elevationID",
                               LogType.ERROR);
            }
        }

        private static List<Policy> CreateActionPolicies(LyvinAction action)
        {
            var actionPolicies = new List<Policy> {new Policy()};
            //ToDo: create a list of policies for the current action

            return actionPolicies;
        }
    }
}