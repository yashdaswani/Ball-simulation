using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    public static AdsManager instance;
     
    [SerializeField] string BannerID;
    [SerializeField] string InterstitialID;
    [SerializeField] string RewardedID;
    [SerializeField] string RewardedIDAuto;
    [SerializeField] string GameID = "4488017";
    [SerializeField] bool TestAds;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {

    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {

    }

    bool eligibleForAds()
    {
        if(PlayerPrefs.GetInt(FirebaseSetup.instance.CurrentLeaderboardStatName) > 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        if (placementId == RewardedID)
        {
            //Give user the reweard here
            FirebaseSetup.instance.LogStartingAdClickEvent("1");
        }
        else if (placementId == InterstitialID)
        {
            FirebaseSetup.instance.LogStartingAdClickEvent("2");
            //Do something
        }
        else if (placementId == BannerID)
        {
            FirebaseSetup.instance.LogStartingAdClickEvent("3");
            //Do something
        }
        else if (placementId == RewardedIDAuto)
        {
            FirebaseSetup.instance.LogStartingAdClickEvent("4");
        }
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(placementId == RewardedID)
        {
            //Give user the reweard here
            HeaderManager.instance.AddGems(100);
            FirebaseSetup.instance.LogWatchedAdEvent("1");
        }
        else if(placementId == InterstitialID)
        {
            FirebaseSetup.instance.LogWatchedAdEvent("2");
            //Do something
        }
        else if(placementId == BannerID)
        {
            FirebaseSetup.instance.LogWatchedAdEvent("3");
            //Do something
        }
        else if(placementId == RewardedIDAuto)
        {
            FirebaseSetup.instance.LogWatchedAdEvent("4");
        }
        LoadAds();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {

    }

    public void OnUnityAdsShowStart(string placementId)
    {
        if (placementId == RewardedID)
        {
            //Give user the reweard here
            FirebaseSetup.instance.LogStartingAdEvent("1");
        }
        else if (placementId == InterstitialID)
        {
            FirebaseSetup.instance.LogStartingAdEvent("2");
            //Do something
        }
        else if (placementId == BannerID)
        {
            FirebaseSetup.instance.LogStartingAdEvent("3");
            //Do something
        }
        else if (placementId == RewardedIDAuto)
        {
            FirebaseSetup.instance.LogStartingAdEvent("4");
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        if(Application.platform != RuntimePlatform.IPhonePlayer)
        {
            BannerID = "BannerAd";
            InterstitialID = "InterstitialAd";
            RewardedID = "RewardedAd";
            RewardedIDAuto = "RewardedAuto";
            GameID = "4488017";
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            BannerID = "Banner_iOS";
            InterstitialID = "Interstitial_iOS";
            RewardedID = "Rewarded_iOS";
            RewardedIDAuto = "RewardedAuto_iOS";
            GameID = "4488016";
        }
        InitialiseAds();
    }
    void InitialiseAds()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(GameID, TestAds, this);
        }
    }
    public void LoadAds()
    {
        Advertisement.Load(RewardedID, this);
        Advertisement.Load(InterstitialID, this);
        Advertisement.Load(BannerID, this);
        Advertisement.Load(RewardedIDAuto, this);
    }

    public void ShowBannerAd()
    {
        Advertisement.Show(BannerID, this);
    }
    int rewardedadscount;
    public void ShowAutoRewardedAd()
    {
        rewardedadscount++;
        if(eligibleForAds() && rewardedadscount % 2 == 0)
        {
            Advertisement.Show(RewardedIDAuto, this);
        }
    }
    public void ShowRewardedAd()
    {
        //This gives reward
        Advertisement.Show(RewardedID, this);
    }
    int interAdCount = 0;
    public void ShowInterstital()
    {
        interAdCount++;
        if(interAdCount%2==0)
        {
            Advertisement.Show(InterstitialID, this);
        }
    }

    public void OnInitializationComplete()
    {
        LoadAds();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("failed to initialise ads");
    }
}
