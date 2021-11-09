using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GamerWolf.Super_BoyZ;


public class AdController : MonoBehaviour{
    public static AdController current;

    private readonly string interstitialId = "ca-app-pub-3940256099942544/1033173712";
    private readonly string rewardedId = "ca-app-pub-3940256099942544/5224354917";


    private readonly string appId = "ca-app-pub-7290372165583145~6134101927";

    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    // private int npa;

    
    private void Awake(){
        DontDestroyOnLoad(this);
        if (current == null){
            current = this;
        }
        else {
            Destroy(gameObject);

        }
        MobileAds.Initialize(initStatus => {
            Debug.Log("Init Stauts " + initStatus);
        });
    }



    private void Start(){
        InitializeAd();
        // if (!PlayerPrefs.HasKey("npa")){
        //     TACanvasController.CallOnAgreeEvent();
        // }
        // else{
        // }
        // InitializeSdk();
        
    }

    // public void InitializeSdk(){
    //     // npa = PlayerPrefs.GetInt("npa");
    //     // var requestConfiguration = new RequestConfiguration.Builder().SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.False).build();

    //     // MobileAds.SetRequestConfiguration(requestConfiguration);

    //     // MobileAds.Initialize(initStatus => {
    //     //     var map = initStatus.getAdapterStatusMap();
    //     //     foreach (var keyValuePair in map){
    //     //         var className = keyValuePair.Key;
    //     //         var status = keyValuePair.Value;
    //     //         switch (status.InitializationState){
    //     //             case AdapterState.NotReady:
    //     //                 Debug.Log("Adapter: " + className + " not ready.");
    //     //                 break;
    //     //             case AdapterState.Ready:
    //     //                 Debug.Log("Adapter: " + className + " is initialized.");
    //     //                 break;
    //     //         }
    //     //     }

    //     // });
        
        
        
    // }

    private void InitializeAd(){
        interstitialAd?.Destroy();
        interstitialAd = new InterstitialAd(interstitialId);
        interstitialAd.OnAdClosed += (sender, args) => RequestInterstitial();

        RequestInterstitial();


        rewardedAd = new RewardedAd(rewardedId);
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        RequestRewardedAd();
    }

    

    private void RequestInterstitial(){
        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(request);
    }

    private void RequestRewardedAd(){
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }
    
    public void ShowInterstitialAd(){
        if (interstitialAd.IsLoaded()){
            interstitialAd.Show();
        }
        else{
            RequestInterstitial();
            if (interstitialAd.IsLoaded()){
                interstitialAd.Show();
            }
            else {
                Debug.Log("Interstitial is not loaded");
            }
        }
    }

    public bool IsRewardedAdLoaded(){
        return rewardedAd.IsLoaded();
    }

    public void ShowRewardedAd(){
        if (rewardedAd.IsLoaded()){
            rewardedAd.Show();
            
            GameHandler.current.SetIsAdsPlaying(true);
        }else{
            RequestRewardedAd();
            if (rewardedAd.IsLoaded()){
                rewardedAd.Show();
                GameHandler.current.SetIsAdsPlaying(true);
            }
            else{
                GameHandler.current.SetIsAdsPlaying(false);
                Debug.Log("RewardedAd is not loaded");
            }
        }
    }

    #region RewardedHandle

    public void HandleRewardedAdLoaded(object sender, EventArgs args){
        if(GameHandler.current != null){
            GameHandler.current.SetCanRewardedShowAd(true);
        }
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args){
        RequestRewardedAd();
        GameHandler.current.SetIsAdsPlaying(false);
        GameHandler.current.SetGameOver();
        GameHandler.current.SetCanRewardedShowAd(false);
        if (!rewardedAd.IsLoaded()){
            RequestRewardedAd();
        }
        else{
            GameHandler.current.SetCanRewardedShowAd(true);
        }
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args){
        // AudioManager.i.PauseMusic(SoundType.Main_Sound);
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args){
        if(!rewardedAd.IsLoaded()){
            RequestRewardedAd();
        }
        GameHandler.current.SetIsAdsPlaying(false);
        GameHandler.current.SetGameOver();
        
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args){
        if (!rewardedAd.IsLoaded()){
            RequestRewardedAd();
        }
    }

    private void HandleUserEarnedReward(object sender, Reward args){
        GameHandler.current.SetIsAdsPlaying(false);
        GameHandler.current.SetCanRewardedShowAd(false);
        GameHandler.current.SetIsRevived(true);
        Debug.Log("Ads Reward Completed");
        RequestRewardedAd();
        
    }

    #endregion
    
    
    
}