using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Metagame
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string _androidID = "4876959";
        private const string _iOSId = "4876958";

        #region AdIDs;

        [Header("AD IDs")]
        public bool ShowBanner;
        [SerializeField] private bool _testMode = true;
        [SerializeField] private string _placementID;
        //public string myPlacementId = "rewardedVideo";
        [SerializeField] private string _androidReward = "Rewarded_Android";
        [SerializeField] private string _androidInterstitial = "Interstitial_Android";
        [SerializeField] private string _androidBanner = "Banner_Android";
        #endregion

        #region Buttons;
        
        [Header("Ad Buttons")]
        public Button ShowInterstitialAdButton;
        public Button ShowRewardAdButton;
        public Button ShowBannerAdButton;
        public Button HideBannerAdButton;
        #endregion

        
        private void Awake()
        {
            // Get the Ad Unit ID for the current platform:
            _placementID = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSId
                : _androidID;

            Advertisement.Initialize(_androidID, _testMode, this);
            if (ShowBanner)
                ShowBannerAd();
            
            AssignButtons();
        }

        

        private void AssignButtons()
        {
            ShowInterstitialAdButton.onClick.AddListener(PlayAd);
            ShowInterstitialAdButton.interactable = true;
            
            ShowRewardAdButton.onClick.AddListener(PlayRewardedAd);
            ShowRewardAdButton.interactable = true;
            
            ShowBannerAdButton.onClick.AddListener(ShowBannerAd);
            ShowBannerAdButton.interactable = true;
            
            HideBannerAdButton.onClick.AddListener(HideBannerAd);
            HideBannerAdButton.interactable = true;
        }

        

        private void PlayAd()
        {
            Debug.Log("Play interstitial Ad");
            Advertisement.Show(_androidInterstitial);
        }

        private void PlayRewardedAd()
        {
            Debug.Log("Play rewarded Ad");
            Advertisement.Show(_androidReward);
        }


        private void ShowBannerAd()
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(_androidBanner);
        }


        private void HideBannerAd() => Advertisement.Banner.Hide();

        
        
        private IEnumerator RepeatShowBanner()
        {
            yield return new WaitForSeconds(1);
            ShowBannerAd();
        }
        
        
        
        /// <summary>
        /// Initialization Logic
        /// </summary>
        
        public void OnInitializationComplete()
        { 
            Debug.Log("Unity Ads initialization complete.");
        }

        
        
        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        
        
        
        /// <summary>
        /// Ads OnLoad Logic
        /// </summary>
        
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("Ads are ready");
        }

        
        
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log("ERROR: " + message);
        }

        
        
        
        
        /// <summary>
        /// Ads OnShow Logic
        /// </summary>
        
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log("ERROR: " + message);
        }

        
        
        
        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log("Video Started");
        }

        
        
        
        public void OnUnityAdsShowClick(string placementId)
        {
            throw new System.NotImplementedException();
        }
        
        
        

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId == _androidReward && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
                Debug.Log("Reward Player for watching full reward video");

            else if (placementId == _androidReward && showCompletionState == UnityAdsShowCompletionState.SKIPPED)
                Debug.Log("Reward Player for skipping reward video");
        }
    }
}
