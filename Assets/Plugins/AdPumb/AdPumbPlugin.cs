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
        public AndroidJavaObject build(){
            #if UNITY_ANDROID
            return PlacementBuilderObject.Call<AndroidJavaObject>("build");
            #endif
            return null;
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