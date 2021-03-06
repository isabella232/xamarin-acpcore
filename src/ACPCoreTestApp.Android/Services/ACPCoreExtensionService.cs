﻿/*
Copyright 2020 Adobe
All Rights Reserved.

NOTICE: Adobe permits you to use, modify, and distribute this file in
accordance with the terms of the Adobe license agreement accompanying
it. If you have received this file from a source other than Adobe,
then your use, modification, or distribution of it requires the prior
written permission of Adobe. (See LICENSE-MIT for details)
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Android.Runtime;
using Com.Adobe.Marketing.Mobile;
using System.Collections;
using System.Threading;

namespace ACPCoreTestApp.Droid
{
    public class ACPCoreExtensionService : IACPCoreExtensionService
    {
        TaskCompletionSource<string> stringOutput;
        private static CountdownEvent latch = null;
        private static string callbackString = "";

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
            var data = new Dictionary<string, Java.Lang.Object>();
            data.Add("testEvent", true);
            Event sdkEvent = new Event.Builder("eventName", "eventType", "eventSource").SetEventData(data).Build();
            stringOutput.SetResult(ACPCore.DispatchEvent(sdkEvent, new ErrorCallback()).ToString());
            return stringOutput;
        }

        public TaskCompletionSource<string> DispatchEventWithResponseCallback()
        {
            stringOutput = new TaskCompletionSource<string>();
            var data = new Dictionary<string, Java.Lang.Object>();
            data.Add("testEvent", true);
            Event sdkEvent = new Event.Builder("eventName", "eventType", "eventSource").SetEventData(data).Build();
            stringOutput.SetResult(ACPCore.DispatchEventWithResponseCallback(sdkEvent, new StringCallback(), new ErrorCallback()).ToString());
            return stringOutput;
        }

        public TaskCompletionSource<string> DispatchResponseEvent()
        {
            stringOutput = new TaskCompletionSource<string>();
            var data = new Dictionary<string, Java.Lang.Object>();
            data.Add("testDispatchResponseEvent", "true");
            Event requestEvent = new Event.Builder("eventName", "eventType", "eventSource").SetEventData(data).Build();
            Event responseEvent = new Event.Builder("eventName", "eventType", "eventSource").SetEventData(data).Build();
            stringOutput.SetResult(ACPCore.DispatchResponseEvent(responseEvent, requestEvent, new ErrorCallback()).ToString());
            return stringOutput;
        }

        public TaskCompletionSource<string> DownloadRules()
        {
            // TODO: this method is not implemented on Android
            stringOutput = new TaskCompletionSource<string>();
            stringOutput.SetResult("");
            return stringOutput;
        }

        public TaskCompletionSource<string> GetPrivacyStatus()
        {
            latch = new CountdownEvent(1);
            stringOutput = new TaskCompletionSource<string>();
            ACPCore.GetPrivacyStatus(new StringCallback());
            latch.Wait(1000);
            stringOutput.SetResult(callbackString);
            return stringOutput;
        }

        public TaskCompletionSource<string> GetSDKIdentities()
        {
            latch = new CountdownEvent(1);
            stringOutput = new TaskCompletionSource<string>();
            ACPCore.GetSdkIdentities(new StringCallback());
            latch.Wait(1000);
            stringOutput.SetResult(callbackString);
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
            ACPCore.LogLevel = LoggingMode.Verbose;
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> SetPrivacyStatus()
        {
            stringOutput = new TaskCompletionSource<string>();
            ACPCore.SetPrivacyStatus(MobilePrivacyStatus.OptIn);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> TrackAction()
        {
            stringOutput = new TaskCompletionSource<string>();
            var data = new Dictionary<string, string>();
            data.Add("key", "value");
            ACPCore.TrackAction("action", data);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> TrackState()
        {
            stringOutput = new TaskCompletionSource<string>();
            var data = new Dictionary<string, string>();
            data.Add("key", "value");
            ACPCore.TrackState("state", data);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> UpdateConfig()
        {
            stringOutput = new TaskCompletionSource<string>();
            var config = new Dictionary<string, Java.Lang.Object>();
            config.Add("someConfigKey", "configValue");
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
            latch = new CountdownEvent(1);
            stringOutput = new TaskCompletionSource<string>();
            ACPIdentity.AppendVisitorInfoForURL("https://example.com", new StringCallback());
            latch.Wait(1000);
            stringOutput.SetResult(callbackString);
            return stringOutput;
        }

        public TaskCompletionSource<string> GetExperienceCloudId()
        {
            latch = new CountdownEvent(1);
            stringOutput = new TaskCompletionSource<string>();
            ACPIdentity.GetExperienceCloudId(new StringCallback());
            latch.Wait(1000);
            stringOutput.SetResult(callbackString);
            return stringOutput;
        }

        public TaskCompletionSource<string> GetIdentifiers()
        {
            latch = new CountdownEvent(1);
            stringOutput = new TaskCompletionSource<string>();
            ACPIdentity.GetIdentifiers(new GetIdentifiersCallback());
            latch.Wait(1000);
            stringOutput.SetResult(callbackString);
            return stringOutput;
        }

        public TaskCompletionSource<string> GetUrlVariables()
        {
            latch = new CountdownEvent(1);
            stringOutput = new TaskCompletionSource<string>();
            ACPIdentity.GetUrlVariables(new StringCallback());
            latch.Wait(1000);
            stringOutput.SetResult(callbackString);
            return stringOutput;
        }

        public TaskCompletionSource<string> SyncIdentifier()
        {
            stringOutput = new TaskCompletionSource<string>();
            ACPIdentity.SyncIdentifier("name", "john", VisitorID.AuthenticationState.Authenticated);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> SyncIdentifiers()
        {
            stringOutput = new TaskCompletionSource<string>();
            var ids = new Dictionary<string, string>();
            ids.Add("lastname", "doe");
            ids.Add("age", "38");
            ids.Add("zipcode", "94403");
            ACPIdentity.SyncIdentifiers(ids);
            stringOutput.SetResult("completed");
            return stringOutput;
        }

        public TaskCompletionSource<string> SyncIdentifiersWithAuthState()
        {
            stringOutput = new TaskCompletionSource<string>();
            var ids = new Dictionary<string, string>();
            ids.Add("lastname", "doe");
            ids.Add("age", "38");
            ids.Add("zipcode", "94403");
            ACPIdentity.SyncIdentifiers(ids, VisitorID.AuthenticationState.LoggedOut);
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
        class StringCallback : Java.Lang.Object, IAdobeCallback
        {
            public void Call(Java.Lang.Object stringContent)
            {
                if (stringContent != null)
                {
                    callbackString = stringContent.ToString();
                }
                else
                {
                    callbackString = "null content in string callback";
                }
                if (latch != null)
                {
                    latch.Signal();
                }
            }
        }

        class GetIdentifiersCallback : Java.Lang.Object, IAdobeCallback
        {
            public void Call(Java.Lang.Object retrievedIds)
            {
                System.String visitorIdsString = "[]";
                if (retrievedIds != null)
                {
                    var ids = GetObject<JavaList>(retrievedIds.Handle, JniHandleOwnership.DoNotTransfer);
                    if (ids != null && ids.Count > 0)
                    {
                        visitorIdsString = "";
                        foreach (VisitorID id in ids)
                        {
                            visitorIdsString = visitorIdsString + "[Id: " + id.Id + ", Type: " + id.IdType + ", Origin: " + id.IdOrigin + ", Authentication: " + id.GetAuthenticationState() + "]";
                        }
                    }
                }
                callbackString = "Retrieved visitor ids: " + visitorIdsString;
                if (latch != null)
                {
                    latch.Signal();
                }
            }
        }

        class ErrorCallback : Java.Lang.Object, IExtensionErrorCallback
        {

            public void Call(Java.Lang.Object sdkEvent)
            {
                callbackString = "AEP SDK event data: " + sdkEvent.ToString();
            }

            public void Error(Java.Lang.Object error)
            {
                callbackString = "AEP SDK Error: " + error.ToString();
            }

        }
    }

}