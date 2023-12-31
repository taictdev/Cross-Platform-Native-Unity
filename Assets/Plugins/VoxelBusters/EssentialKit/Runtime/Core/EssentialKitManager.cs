﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    public class EssentialKitManager : PrivateSingletonBehaviour<EssentialKitManager>
    {
        #region Static methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnLoad()
        {
#pragma warning disable 
            var     singleton   = GetSingleton();
#pragma warning restore 
        }

        #endregion

        #region Unity methods

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();

            // Create required systems
            CallbackDispatcher.Initialize();

            // Set environment variables
            var     settings    = EssentialKitSettings.Instance;
            DebugLogger.SetLogLevel(settings.ApplicationSettings.LogLevel,
                                    CoreLibraryDomain.Default,
                                    CoreLibraryDomain.NativePlugins,
                                    EssentialKitDomain.Default);

            // Initialize required features
            AddressBook.Initialize(settings.AddressBookSettings);
            BillingServices.Initialize(settings.BillingServicesSettings);
            CloudServices.Initialize(settings.CloudServicesSettings);
            DeepLinkServices.Initialize(settings.DeepLinkServicesSettings);
            GameServices.Initialize(settings.GameServicesSettings);
            MediaServices.Initialize(settings.MediaServicesSettings);
            NetworkServices.Initialize(settings.NetworkServicesSettings);
            NotificationServices.Initialize(settings.NotificationServicesSettings);
            NativeUI.Initialize(settings.NativeUISettings, overrideAvailability: settings.IsFeatureUsed(NativeFeatureType.kNativeUI));
            SharingServices.Initialize(settings.SharingServicesSettings);
            Utilities.Initialize();            
            WebView.Initialize(settings.WebViewSettings);
            if (settings.ApplicationSettings.RateMyAppSettings.IsEnabled)
            {
                if (null == FindObjectOfType<RateMyApp>())
                {
                    ActivateRateMyAppService();
                }
            }
        }

        #endregion

        #region Private methods

        private static void ActivateRateMyAppService()
        {
            var     prefab      = Resources.Load<GameObject>("RateMyApp");
            Assert.IsPropertyNotNull(prefab, "prefab");

            Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        #endregion
    }
}