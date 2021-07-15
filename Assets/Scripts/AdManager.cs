using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
//using Firebase;

public class AdManager : MonoBehaviour, IUnityAdsListener {

    public static string InterstitialAd = "Interstitial_Android";
    public static string rewardedVideoAd = "Rewarded_Android";

    public static bool isAdOpened;
    public bool isRewardedShowed;
    //private FirebaseApp app;

    void Awake() {
        /*
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {

                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        */
    }

    private void Start() {
        //GameAnalytics.Initialize();
        if (!Advertisement.isInitialized) {
            Advertisement.AddListener(this);
            Advertisement.Initialize("4216553", true);
        } else {
        }
    }

    public void PlayInterstitialAd() {
        if (!Advertisement.IsReady(InterstitialAd)) {

            return;
        }
        Advertisement.Show(InterstitialAd);
        isAdOpened = true;
    }

    public void PlayRewardedVideoAd() {
        if (!Advertisement.IsReady(rewardedVideoAd)) { return; }
        Advertisement.Show(rewardedVideoAd);
        isAdOpened = true;
    }
    public void OnUnityAdsReady(string placementId) {}
    public void OnUnityAdsDidError(string message) {}
    public void OnUnityAdsDidStart(string placementId) {}
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        isAdOpened = false;
        switch (showResult) {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                isRewardedShowed = true;
                break;
            case ShowResult.Finished:
                break;
        }
    }
}