using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdPumbPlugin {
    public class RequestConfiguration{
        #if UNITY_ANDROID
        AndroidJavaObject AdPumbConfigurationBuilderObject = new AndroidJavaObject("com.google.android.gms.ads.RequestConfiguration.Builder");
        #endif
        public RequestConfiguration setTestDeviceIds(object[] deviceIds){
            #if UNITY_ANDROID
            if(deviceIds.Length>0){
                AndroidJavaClass ArraysClass  = new AndroidJavaClass("java.util.Arrays");
                AdPumbConfigurationBuilderObject.Call("setTestDeviceIds", new object[] { ArraysClass.CallStatic<AndroidJavaObject>("asList",deviceIds) } );
            }
            #endif
            return this;
        }
        public AndroidJavaObject build(){
            #if UNITY_ANDROID
            return AdPumbConfigurationBuilderObject.Call<AndroidJavaObject>("build");
            #endif
            return null;
        }
    }
    public class AdPumbConfiguration{
        public static string LOG_LEVEL_NONE = "NONE";
        public static string LOG_LEVEL_TRACE = "TRACE";
        #if UNITY_ANDROID
        AndroidJavaObject AdPumbConfigurationObject = null ;
        AndroidJavaObject currentActivity = null ;
        #endif
        // AdPumbConfiguration(){
        //     initApp("com.unity3d.player.UnityPlayer");
        // }
        // AdPumbConfiguration(string mainActivityName){
        //     initApp(mainActivityName);
        // }
        public AdPumbConfiguration setActivity(string mainActivityName){
            #if UNITY_ANDROID
            currentActivity = new AndroidJavaClass(mainActivityName).GetStatic<AndroidJavaObject>("currentActivity");
            AdPumbConfigurationObject = new AndroidJavaObject("com.adpumb.lifecycle.AdPumbConfiguration",new object[]{ currentActivity });
            #endif
            return this;
        }
        public AndroidJavaObject getActivity(){
            #if UNITY_ANDROID
            return currentActivity;
            #endif
            return null;
        }
        public AdPumbConfiguration setDebugMode(bool isDebug){
            #if UNITY_ANDROID
            AdPumbConfigurationObject.Call<AndroidJavaObject>("setDebugMode",new object[]{
                new AndroidJavaObject("java.lang.Boolean",isDebug.ToString().ToLower()) 
            });
            #endif
            return this;
        }
        public AdPumbConfiguration addRequestConfiguration(RequestConfiguration requestConfiguration){ // TODO: proxy class for RequestConfiguration
            #if UNITY_ANDROID
            AdPumbConfigurationObject.Call<AndroidJavaObject>("addRequestConfiguration",new object[]{ requestConfiguration.build() });
            #endif
            return this;
        }
        public AdPumbConfiguration setLogLvl(string logLevel){
            #if UNITY_ANDROID
            AndroidJavaClass logLvlEnum = new    AndroidJavaClass("com.adpumb.lifecycle.AdPumbLogLvl");
            AndroidJavaObject logLvl = logLvlEnum.GetStatic<AndroidJavaObject>(logLevel);
            AdPumbConfigurationObject.Call("setLogLvl",new object[]{ logLvl });
            #endif
            return this;
        }
        public AndroidJavaObject build(){
            #if UNITY_ANDROID
            return AdPumbConfigurationObject;
            #endif
            return null;
        }
    }

    public sealed class AdPumb {
        private static bool isAdInitialized = false;
        private static AndroidJavaObject currentActivity;
        public static AndroidJavaObject getCurrentActivity(){
            return currentActivity;
        }
        public static void register(AdPumbConfiguration adPumbConfiguration){
            if(isAdInitialized){
                return;
            }
            isAdInitialized = true;
            #if UNITY_ANDROID
            AdPumb.currentActivity = adPumbConfiguration.getActivity();
            AndroidJavaClass adPumbClass = new AndroidJavaClass("com.adpumb.lifecycle.Adpumb");
            adPumbClass.CallStatic("register", new object[] { adPumbConfiguration.build() });
            Debug.Log(" adpumb registered ");
            #endif
        }
    }
    public class DisplayManager{
        private static readonly DisplayManager instance = new DisplayManager();
        #if UNITY_ANDROID
        AndroidJavaClass DisplayManagerClass  = new AndroidJavaClass("com.adpumb.ads.display.DisplayManager");
        #endif
        public static DisplayManager Instance {  
            get {  
                return instance;  
            }  
        }
        public DisplayManager(){
            #if UNITY_ANDROID
            // DisplayManagerClass = new AndroidJavaClass("com.adpumb.ads.display.DisplayManager");
            #endif
        }
        public void showAd(AndroidJavaObject PlacementObject){
            #if UNITY_ANDROID
            DisplayManagerClass.CallStatic<AndroidJavaObject>("getInstance").Call("showAd",PlacementObject);
            #endif
        }
    }
    public class AdPlacementBuilder{
        #if UNITY_ANDROID
        AndroidJavaObject PlacementBuilderObject;
        #endif
        public static AdPlacementBuilder Interstitial(){
            return new AdPlacementBuilder("InterstitialPlacementBuilder");
        }
        public static AdPlacementBuilder Rewarded(){
            return new AdPlacementBuilder("RewardedPlacementBuilder");
        }
        public AdPlacementBuilder(string builderClassName ){
            #if UNITY_ANDROID
            PlacementBuilderObject = new AndroidJavaObject("com.adpumb.ads.display."+builderClassName);
            #endif
        }
        public AdPlacementBuilder name(string name){
            #if UNITY_ANDROID
            PlacementBuilderObject = PlacementBuilderObject.Call<AndroidJavaObject>("name", new object[] { name} );
            #endif
            return this;
        }
        public AdPlacementBuilder showLoaderTillAdIsReady(bool showLoader){
            #if UNITY_ANDROID
            PlacementBuilderObject = PlacementBuilderObject.Call<AndroidJavaObject>("showLoaderTillAdIsReady", new object[] { 
                new AndroidJavaObject("java.lang.Boolean",showLoader.ToString().ToLower()) } );
            #endif
            return this;
        }
        public AdPlacementBuilder loaderTimeOutInSeconds(long duration){
            #if UNITY_ANDROID
            PlacementBuilderObject = PlacementBuilderObject.Call<AndroidJavaObject>("loaderTimeOutInSeconds", new object[] { 
                new AndroidJavaObject("java.lang.Long",""+duration) } );
            #endif
            return this;
        }
        public AdPlacementBuilder frequencyCapInSeconds(long duration){
            #if UNITY_ANDROID
            PlacementBuilderObject = PlacementBuilderObject.Call<AndroidJavaObject>("frequencyCapInSeconds", new object[] { 
                new AndroidJavaObject("java.lang.Long",""+duration) } );
            #endif
            return this;
        }
        public AdPlacementBuilder onAdCompletion(AdCompletionDelegate onAdCompletion){
            #if UNITY_ANDROID
            AdPumbPluginAdCompletionCallbackProxy obj = new AdPumbPluginAdCompletionCallbackProxy();
            obj.setCallback(onAdCompletion);
            PlacementBuilderObject = PlacementBuilderObject.Call<AndroidJavaObject>("onAdCompletion", new object[] { obj } );
            #endif
            return this;
        }
        public AdPlacementBuilder loaderUISetting(LoaderSettings loadersettings){
            #if UNITY_ANDROID
            PlacementBuilderObject = PlacementBuilderObject.Call<AndroidJavaObject>("loaderUISetting", new object[] { loadersettings.get() } );
            #endif
            return this;
        }
        public AndroidJavaObject build(){
            #if UNITY_ANDROID
            return PlacementBuilderObject.Call<AndroidJavaObject>("build");
            #endif
            return null;
        }
    }
    public class LoaderSettings{
        AndroidJavaObject LoaderSettingsObject;
        public LoaderSettings(){
            #if UNITY_ANDROID
            LoaderSettingsObject = new AndroidJavaObject("com.adpumb.ads.display.LoaderSettings") ;
            #endif
        }
        public void setLogoResID(){
            #if UNITY_ANDROID
            AndroidJavaObject unityActivity = AdPumb.getCurrentActivity(); ////
            string packageName = unityActivity.Call<string>("getPackageName");
            AndroidJavaObject resource = unityActivity.Call<AndroidJavaObject>("getResources");
            int app_icon = resource.Call<int>("getIdentifier", new object[] { "app_icon", "mipmap", packageName } );
            LoaderSettingsObject.Call("setLogoResID",new object[] { new AndroidJavaObject("java.lang.Integer",""+app_icon)  } );
            #endif
        }
        public AndroidJavaObject get(){
            return LoaderSettingsObject;
        }
    }
    public delegate void AdCompletionDelegate (bool success);
    class AdPumbPluginAdCompletionCallbackProxy : AndroidJavaProxy {
        AdCompletionDelegate deligate;
        public void setCallback( AdCompletionDelegate callback2 ){
            deligate = callback2;
        }
        public AdPumbPluginAdCompletionCallbackProxy() : base("com.adpumb.ads.display.AdCompletion") { }

        public void onAdCompletion(bool success, AndroidJavaObject placementDisplayStatus) {
            Debug.Log(" AdPumbPluginAdCompletionCallbackProxy " );
            deligate(success);
        }
    }
    public interface AdCompletion{
        public void onAdCompletion(bool success);
    }  
}