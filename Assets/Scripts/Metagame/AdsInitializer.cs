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

        private const string _iOS = "iOS";
        private const string _android = "Android"; 
        
        [SerializeField] private bool _testMode = true;
        [SerializeField] private string _placementID;
        
        [SerializeField] private string _rewardID = "Rewarded_";
        [SerializeField] private string _interstitialID = "Interstitial_";
        [SerializeField] private string _bannerID = "Banner_";
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
            InitializeAds();

            if (ShowBanner)
                ShowBannerAd();
            
            AssignButtons();
        }



        private void InitializeAds()
        {
            // Get the Ad Unit ID for the current platform:
            _placementID = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSId
                : _androidID;
            
            Advertisement.Initialize(_placementID, _testMode, this);

            AssignAdIds(_placementID == _iOSId ? _iOS : _android);
        }

        

        private void AssignAdIds(string platform)
        {
            _rewardID += platform;
            _interstitialID += platform;
            _bannerID += platform;
        }

        

        private void AssignButtons()
        {
            ShowInterstitialAdButton.onClick.AddListener(PlayInterstitialAd);
            ShowInterstitialAdButton.interactable = true;
            
            ShowRewardAdButton.onClick.AddListener(PlayRewardedAd);
            ShowRewardAdButton.interactable = true;
            
            ShowBannerAdButton.onClick.AddListener(ShowBannerAd);
            ShowBannerAdButton.interactable = true;
            
            HideBannerAdButton.onClick.AddListener(HideBannerAd);
            HideBannerAdButton.interactable = true;
        }

        

        private void PlayInterstitialAd()
        {
            Debug.Log("Play interstitial Ad");
            Advertisement.Load(_interstitialID, this);
            Advertisement.Show(_interstitialID);
        }

        private void PlayRewardedAd()
        {
            Debug.Log("Play rewarded Ad");
            Advertisement.Load(_rewardID, this);
            Advertisement.Show(_rewardID);
        }


        private void ShowBannerAd()
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Load(_bannerID);

            if (Advertisement.Banner.isLoaded == false)
                StartCoroutine(RepeatShowBanner());
            
            Advertisement.Banner.Show(_bannerID);
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
            Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
        }





        /// <summary>
        /// Ads OnShow Logic
        /// </summary>

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        }




        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"Started Ad Unit {placementId}");
        }

        
        
        
        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"Clicked on {placementId}");
        }
        
        
        

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId == _rewardID && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
                Debug.Log($"{placementId} status = {showCompletionState} - Reward Player for watching full reward video");

            else if (placementId == _rewardID && showCompletionState == UnityAdsShowCompletionState.SKIPPED)
                Debug.Log($"{placementId} status = {showCompletionState} - Reward Player for skipping reward video");
        }
    }
}
