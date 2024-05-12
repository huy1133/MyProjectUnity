using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class adManager : MonoBehaviour
{
    [SerializeField] Button bannerButton;
    [SerializeField] Button rewardAdLoad;
    [SerializeField] Button rewardAdShow;
    [SerializeField] Text coinText;

    public string appId = "ca-app-pub-7644187281063920~2553749138";

    public string bannerAdId = "ca-app-pub-7644187281063920/2697219360";

    public string rewardAdId = "ca-app-pub-7644187281063920/6640419523";

    public BannerView bannerView;

    public RewardedAd rewardedAd;

    public void Start()
    {
        bannerView = null;
        rewardedAd = null;
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            
        });
        bannerButton.onClick.AddListener(bannerLoadAd);
        rewardAdLoad.onClick.AddListener(LoadRewardedAd);
        rewardAdShow.onClick.AddListener(ShowRewardedAd);
    }
    private void Update()
    {
        coinText.text = "coin: " + PlayerPrefs.GetInt("Coin");
    }
    #region bannerAd
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");
        if (bannerView != null)
        {
            DestroyBannerAd();
        }
        bannerView = new BannerView(bannerAdId, AdSize.Leaderboard, AdPosition.Top);
    }
    public void bannerLoadAd()
    {
        // create an instance of a banner view first.
        if (bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        bannerView.LoadAd(adRequest);
    }
    public void DestroyBannerAd()
    {
        bannerView.Destroy();
        bannerView = null;
        Debug.Log("Destroy banner ad");
    }
    #endregion
    #region rewardAd
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardAdId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
            });
    }
    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                PlayerPrefs.SetInt("Coin",PlayerPrefs.GetInt("Coin")+(int)reward.Amount);
                PlayerPrefs.Save();
            });
        }
    }
    #endregion
}
