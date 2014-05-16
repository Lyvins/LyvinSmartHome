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
//  File            :   ISCLyvinWidgetWebContract.cs                                //
//  Description     :                                                   //
//  Original author :   J.Klessens                                      //
//  Company         :   Lyvins                                          //
//  Project         :   LyvinWidgetAPIContracts                                     //
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

using System.ServiceModel;
using System.ServiceModel.Web;
using LyvinDeviceAPIContracts.DeviceAPIMessages;
using LyvinWidgetAPIContracts.WidgetAPIMessages;

namespace LyvinWidgetAPIContracts
{
    [ServiceContract]
    public interface ISCLyvinWidgetWebContract
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ApplicationReply ApplicationMethod(ApplicationMethod method);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "BasicDeviceValueRequest/{deviceID}/{valueName}",
                ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string BasicDeviceValueRequest(string deviceID, string valueName);

        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DeviceListReply DeviceListRequest(DeviceListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DeviceZoneListReply DeviceZoneListRequest(DeviceZoneListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DeviceGroupListReply DeviceGroupListRequest(DeviceGroupListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DeviceTypeListReply DeviceTypeListRequest(DeviceTypeListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DevicePreUpdateReply DevicePreUpdateRequest(DevicePreUpdateRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DeviceUpdateReply DeviceUpdateRequest(DeviceUpdateRequest request);

        [OperationContract]
        [WebInvoke(Method = "GET",
                ResponseFormat = WebMessageFormat.Json)]
        bool HandShake();
    }
}
