﻿/*
 Copyright 2020 Adobe. All rights reserved.
 This file is licensed to you under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License. You may obtain a copy
 of the License at http://www.apache.org/licenses/LICENSE-2.0
 Unless required by applicable law or agreed to in writing, software distributed under
 the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
 OF ANY KIND, either express or implied. See the License for the specific language
 governing permissions and limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Com.Adobe.Marketing.Mobile;

namespace ACPCoreTestApp.iOS
{
    public class ACPCoreExtensionService : IACPCoreExtensionService
    {
        TaskCompletionSource<string> stringOutput;

        public ACPCoreExtensionService()
        {
        }

        // ACPCore methods
        public TaskCompletionSource<string> GetExtensionVersionCore()
        {
            stringOutput = new TaskCompletionSource<string>();
            stringOutput.SetResult(ACPCore.ExtensionVersion());
            return stringOutput;
        }

        public TaskCompletionSource<string> DispatchEvent()
        {
            stringOutput = new TaskCompletionSource<string>();
            NSError error;
            var data = new NSMutableDictionary<NSString, NSObject>
            {
                ["dispatchResponseEventKey"] = new NSString("dispatchResponseEventValue")
            };
            ACPExtensionEvent sdkEvent = ACPExtensionEvent.ExtensionEventWithName("eventName", "eventType", "eventSource", data, out _);
            stringOutput.SetResult(ACPCore.DispatchEvent(sdkEvent, out error).ToString());
            if (error != null)
            {
                stringOutput.SetResult(error.LocalizedDescription);
            }
            return stringOutput;
        }

        public TaskCompletionSource<string> DispatchEventWithResponseCallback()
        {
            stringOutput = new TaskCompletionSource<string>();
            NSError error;
            var data = new NSMutableDictionary<NSString, NSObject>
            {
                ["dispatchResponseEventKey"] = new NSString("dispatchResponseEventValue")
            };
            ACPExtensionEvent sdkEvent = ACPExtensionEvent.ExtensionEventWithName("eventName", "eventType", "eventSource", data, out _);
            Action<ACPExtensionEvent> callback = new Action<ACPExtensionEvent>(handleCallback);
            stringOutput.SetResult(ACPCore.DispatchEventWithResponseCallback(sdkEvent, callback, out error).ToString());
            if(error != null)
            {
                stringOutput.SetResult(error.LocalizedDescription);
            }
            return stringOutput;
        }

        public TaskCompletionSource<string> DispatchResponseEvent()
        {
            stringOutput = new TaskCompletionSource<string>();
            NSError error;
            var data = new NSMutableDictionary<NSString, NSObject>
            {
                ["dispatchResponseEventKey"] = new NSString("dispatchResponseEventValue")
            };
            ACPExtensionEvent requestEvent = ACPExtensionEvent.ExtensionEventWithName("eventName", "eventType", "eventSource", data, out _);
            ACPExtensionEvent responseEvent = ACPExtensionEvent.ExtensionEventWithName("eventName", "eventType", "eventSource", data, out _);
            stringOutput.SetResult(ACPCore.DispatchResponseEvent(responseEvent, requestEvent, out error).ToString());
            if (error != null)
            {
                stringOutput.SetResult(error.LocalizedDescription);
            }
            return stringOutput;
        }

        public TaskCompletionSource<string> DownloadRules()
        {
            stringOutput = new TaskCompletionSource<string>();
            ACPCore.DownloadRules();
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> GetPrivacyStatus()
        {
            stringOutput = new TaskCompletionSource<string>();
            Action<ACPMobilePrivacyStatus> callback = new Action<ACPMobilePrivacyStatus>(handleCallback);
            ACPCore.GetPrivacyStatus(callback);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> GetSDKIdentities()
        {
            stringOutput = new TaskCompletionSource<string>();
            Action<NSString> callback = new Action<NSString>(handleCallback);
            ACPCore.GetSdkIdentities(callback);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> SetAdvertisingIdentifier()
        {
            stringOutput = new TaskCompletionSource<string>();
            ACPCore.SetAdvertisingIdentifier("testAdvertisingIdentifier");
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> SetLogLevel()
        {
            stringOutput = new TaskCompletionSource<string>();
            ACPCore.LogLevel = ACPMobileLogLevel.Verbose;
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> SetPrivacyStatus()
        {
            stringOutput = new TaskCompletionSource<string>();
            ACPCore.SetPrivacyStatus(ACPMobilePrivacyStatus.OptIn);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> TrackAction()
        {
            stringOutput = new TaskCompletionSource<string>();
            var data = new NSMutableDictionary<NSString, NSString>
            {
                ["key"] = new NSString("value")
            };
            ACPCore.TrackAction("action", data);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> TrackState()
        {
            stringOutput = new TaskCompletionSource<string>();
            var data = new NSMutableDictionary<NSString, NSString>
            {
                ["key"] = new NSString("value")
            };
            ACPCore.TrackState("state", data);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> UpdateConfig()
        {
            stringOutput = new TaskCompletionSource<string>();
            var config = new NSMutableDictionary<NSString, NSObject>
            {
                ["someConfigKey"] = new NSString("configValue")
            };
            ACPCore.UpdateConfiguration(config);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        // ACPIdentity methods
        public TaskCompletionSource<string> GetExtensionVersionIdentity()
        {
            stringOutput = new TaskCompletionSource<string>();
            stringOutput.SetResult(ACPIdentity.ExtensionVersion());
            return stringOutput;
        }

        public TaskCompletionSource<string> AppendVisitorInfoForUrl()
        {
            stringOutput = new TaskCompletionSource<string>();
            Action<NSUrl> callback = new Action<NSUrl>(handleCallback);
            var url = new NSUrl("https://example.com");
            ACPIdentity.AppendToUrl(url, callback);
            stringOutput.SetResult("");
            return stringOutput;
        }

        public TaskCompletionSource<string> GetExperienceCloudId()
        {
            stringOutput = new TaskCompletionSource<string>();
            Action<NSString> callback = new Action<NSString>(handleCallback);
            ACPIdentity.GetExperienceCloudId(callback);
            stringOutput.SetResult("");
            return stringOutput;
        }

        public TaskCompletionSource<string> GetIdentifiers()
        {
            stringOutput = new TaskCompletionSource<string>();
            Action<ACPMobileVisitorId[]> callback = new Action<ACPMobileVisitorId[]>(handleCallback);
            ACPIdentity.GetIdentifiers(callback);
            stringOutput.SetResult("");
            return stringOutput;
        }

        public TaskCompletionSource<string> GetUrlVariables()
        {
            stringOutput = new TaskCompletionSource<string>();
            Action<NSString> callback = new Action<NSString> (handleCallback);
            ACPIdentity.GetUrlVariables(callback);
            stringOutput.SetResult("");
            return stringOutput;
        }

        public TaskCompletionSource<string> SyncIdentifier()
        {
            stringOutput = new TaskCompletionSource<string>();
            ACPIdentity.SyncIdentifier("name", "john", ACPMobileVisitorAuthenticationState.Authenticated);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> SyncIdentifiers()
        {
            stringOutput = new TaskCompletionSource<string>();
            var ids = new NSMutableDictionary<NSString, NSObject>
            {
                ["lastName"] = new NSString("doe"),
                ["age"] = new NSString("38"),
                ["zipcode"] = new NSString("94403")
            };
            ACPIdentity.SyncIdentifiers(ids);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> SyncIdentifiersWithAuthState()
        {
            stringOutput = new TaskCompletionSource<string>();
            var ids = new NSMutableDictionary<NSString, NSObject>
            {
                ["lastName"] = new NSString("doe"),
                ["age"] = new NSString("38"),
                ["zipcode"] = new NSString("94403")
            };
            ACPIdentity.SyncIdentifiers(ids, ACPMobileVisitorAuthenticationState.LoggedOut);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        // ACPLifecycle methods
        public TaskCompletionSource<string> GetExtensionVersionLifecycle()
        {
            stringOutput = new TaskCompletionSource<string>();
            stringOutput.SetResult(ACPLifecycle.ExtensionVersion());
            return stringOutput;
        }

        // ACPSignal methods
        public TaskCompletionSource<string> GetExtensionVersionSignal()
        {
            stringOutput = new TaskCompletionSource<string>();
            stringOutput.SetResult(ACPSignal.ExtensionVersion());
            return stringOutput;
        }

        // callbacks
        private void handleCallback(ACPExtensionEvent responseEvent)
        {
            Console.WriteLine("Response event name: "+ responseEvent.EventName.ToString() + " type: " + responseEvent.EventType.ToString() + " source: " + responseEvent.EventSource.ToString() + " data: " + responseEvent.EventData.ToString());
        }

        private void handleCallback(ACPMobilePrivacyStatus privacyStatus)
        {
            Console.WriteLine("Privacy status: " + privacyStatus.ToString());
        }

        private void handleCallback(NSString content)
        {
            Console.WriteLine("String callback: " + content);
        }

        private void handleCallback(NSUrl url)
        {
            Console.WriteLine("Appended url: " + url.ToString());
        }

        private void handleCallback(ACPMobileVisitorId[] ids)
        {
            String visitorIdsString = "[]";
            if (ids.Length != 0)
            {
                visitorIdsString = "";
                    foreach (ACPMobileVisitorId id in ids)
                        {
                            visitorIdsString = visitorIdsString + "[Id: " + id.Identifier + ", Type: " + id.IdType + ", Origin: " + id.IdOrigin + ", Authentication: " + id.AuthenticationState + "]";
                        }
            }
            Console.WriteLine("Retrieved visitor ids: " + visitorIdsString);
        }
    }

}
