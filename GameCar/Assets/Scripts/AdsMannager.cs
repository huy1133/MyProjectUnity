using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class AdsMannager : MonoBehaviour
{
    [SerializeField] Button buttonRewardAd;
    [SerializeField] Text textAddkey;
    public string rewardAdId = "ca-app-pub-7644187281063920/6640419523";
    public RewardedAd rewardedAd;
    void Start()
    {
        rewardedAd = null;
        buttonRewardAd.onClick.AddListener(showAds);
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {

        });
        if(rewardedAd == null ) 
        {
            LoadRewardAd();
        }
    }
    public void ShowButtonAds()
    {
        if(rewardedAd != null && rewardedAd.CanShowAd()&& Random.Range(0,20)>10)
        {
            buttonRewardAd.gameObject.SetActive(true);
        }
    }
    #region RewardAd
    void LoadRewardAd()
    {
        var adRequest = new AdRequest();
        RewardedAd.Load(rewardAdId, adRequest,
            (RewardedAd ad, LoadAdError er) =>
            {
                if ( ad == null || er!=null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + er);
                    return;
                }
                rewardedAd = ad;
                RegisterEventHandlers(ad);
            }
        );
    }
    public void showAds()
    {
        if(rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                PlayerPrefs.SetInt("key",PlayerPrefs.GetInt("key") + (int)reward.Amount);
                PlayerPrefs.Save();
            });
        }
    }
    public void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            textAddkey.gameObject.SetActive(true);
        };
    }
    #endregion
}
