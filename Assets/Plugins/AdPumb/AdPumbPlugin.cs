using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdPumbPlugin {

    public sealed class AdPumb {
        private static bool isAdInitialized = false;
        public static void register(bool isDebug){
            if(isAdInitialized){
                return;
            }
            isAdInitialized = true;
            #if UNITY_ANDROID
            AndroidJavaClass unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaClass adPumbClass = new AndroidJavaClass("com.adpumb.lifecycle.Adpumb");
            adPumbClass.CallStatic("register", new object[] { unityActivity.GetStatic<AndroidJavaObject>("currentActivity") , isDebug });
            #endif
        }
    }
    public class DisplayManager{
        private static readonly DisplayManager instance = new DisplayManager();
        #if UNITY_ANDROID
        AndroidJavaClass DisplayManagerClass ;
        #endif
        public static DisplayManager Instance {  
            get {  
                return instance;  
            }  
        }
        public DisplayManager(){
            #if UNITY_ANDROID
            DisplayManagerClass = new AndroidJavaClass("com.adpumb.ads.display.DisplayManager");
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
        AndroidJavaObject PlacementObject;
        #endif
        public static AdPlacementBuilder Interstitial(){
            return new AdPlacementBuilder("InterstitialPlacementBuilder");
        }
        public AdPlacementBuilder(string builderClassName ){
            #if UNITY_ANDROID
            PlacementObject = new AndroidJavaObject("com.adpumb.ads.display."+builderClassName);
            #endif
        }
        public AdPlacementBuilder name(string name){
            #if UNITY_ANDROID
            PlacementObject = PlacementObject.Call<AndroidJavaObject>("name", new object[] { name} );
            #endif
            return this;
        }
        public AdPlacementBuilder showLoaderTillAdIsReady(bool showLoader){
            #if UNITY_ANDROID
            PlacementObject = PlacementObject.Call<AndroidJavaObject>("showLoaderTillAdIsReady", new object[] { showLoader.ToString().ToLower()} );
            #endif
            return this;
        }
        public AdPlacementBuilder loaderTimeOutInSeconds(long duration){
            #if UNITY_ANDROID
            PlacementObject = PlacementObject.Call<AndroidJavaObject>("loaderTimeOutInSeconds", new object[] { ""+duration } );
            #endif
            return this;
        }
        public AdPlacementBuilder frequencyCapInSeconds(long duration){
            #if UNITY_ANDROID
            PlacementObject = PlacementObject.Call<AndroidJavaObject>("frequencyCapInSeconds", new object[] { ""+duration } );
            #endif
            return this;
        }
        public AndroidJavaObject build(){
            #if UNITY_ANDROID
            return PlacementObject.Call<AndroidJavaObject>("build");
            #endif
            return null;
        }
    }
}