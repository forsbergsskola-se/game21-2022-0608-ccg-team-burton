using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
        public bool ShowBannerOnStartup;

        private bool ShowBanner;
        private bool _shouldReactivateBanner;
        private bool OnlyOnce = true;

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

        #region Rewards
        [Header("Reward Type")]
        public bool Multiplier = true;
        public GameObject CoinTextAsset;
        private TMP_Text CoinText;
        private int Coins;
        #endregion

        #region Reward Sums;
        [Header("Reward Sums")]
        public int InterstitialAmount;
        public int RewardFullAmount;
        public int RewardSkippedAmount;
        #endregion
        
        #region Reward Multipliers;
        [Header("Reward Multipliers")]
        public float InterstitialMultiplier;
        public float RewardFullMultiplier;
        public float RewardSkippedMultiplier;
        #endregion
        
        private void Awake()
        {
            if (!Advertisement.isInitialized)
                InitializeAds();

            AssignButtons();

            if (ShowBannerOnStartup)
            {
                LoadBannerAd();
                ShowBannerAd();
            }

            CoinText = CoinTextAsset.GameObject().GetComponent<TMP_Text>();
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
            ShowInterstitialAdButton.onClick.AddListener(LoadInterstitialAd);
            ShowInterstitialAdButton.interactable = true;
            
            ShowRewardAdButton.onClick.AddListener(LoadRewardedAd);
            ShowRewardAdButton.interactable = true;
            
            ShowBannerAdButton.onClick.AddListener(ShowBannerAd);
            ShowBannerAdButton.interactable = true;
            
            HideBannerAdButton.onClick.AddListener(HideBannerAd);
            HideBannerAdButton.interactable = true;
        }

        

        private void LoadInterstitialAd()
        {
            Debug.Log("Play interstitial Ad");
            Advertisement.Load(_interstitialID, this);
        }

        
        
        private void LoadRewardedAd()
        {
            Debug.Log("Play rewarded Ad");
            Advertisement.Load(_rewardID, this);
        }


        
        private void LoadBannerAd()
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Load(_bannerID, new BannerLoadOptions {loadCallback = OnBannerLoaded, errorCallback = OnBannerLoadError});
        }


        
        private void ShowBannerAd()
        {
            if (ShowBannerOnStartup == false && OnlyOnce)
            {
                LoadBannerAd();
                OnlyOnce = false;
            }
            
            ShowBanner = true;
            if (Advertisement.Banner.isLoaded == false)
                StartCoroutine(RepeatShowBanner());
            
            Advertisement.Banner.Show(_bannerID);
        }



        private void HideBannerAd()
        {
            ShowBanner = false;
            Advertisement.Banner.Hide(false);
        }

        
        
        private IEnumerator RepeatShowBanner()
        {
            yield return new WaitForSeconds(1);
            ShowBannerAd();
        }


        private void OnBannerLoaded()
            => Advertisement.Banner.Show(_bannerID, new BannerOptions{});
        
        
        
        
        private static void OnBannerLoadError(string message)
            => Debug.Log($"Error loading Banner Ad - {message}");

        

        
        /// <summary>
        /// Initialization Logic
        /// </summary>
        
        public void OnInitializationComplete() 
            => Debug.Log("Unity Ads initialization complete.");
        
        //TODO: Can add banner ad here
        
        

        
        
        public void OnInitializationFailed(UnityAdsInitializationError error, string message) 
            => Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");



        /// <summary>
        /// Ads OnLoad Logic
        /// </summary>
        
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("Ads are ready");
            Advertisement.Show(placementId, this);
        }

        
        
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) 
            => Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");



        /// <summary>
        /// Ads OnShow Logic
        /// </summary>

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) 
            => Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");



        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"Started Ad Unit {placementId}");
            
            ToggleBanner(true);
        }



        public void OnUnityAdsShowClick(string placementId) 
            => Debug.Log($"Clicked on {placementId}");
        
        
        
        //TODO: Add the reward logic based on Completion State
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Coins = Convert.ToInt32(CoinText.text);

            if (placementId == _interstitialID && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log($"Ad status = {showCompletionState} - Reward Player for watching full {placementId} video");
                
                if (Multiplier)
                    RewardPlayerCalc(InterstitialMultiplier);
                else
                    RewardPlayerSum(InterstitialAmount);
            }

            else if (placementId == _rewardID && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log($"Ad status = {showCompletionState} - Reward Player for watching {placementId} video");
                
                if (Multiplier)
                    RewardPlayerCalc(RewardFullMultiplier);
                else
                    RewardPlayerSum(RewardFullAmount);
            }

            else if (placementId == _interstitialID && showCompletionState == UnityAdsShowCompletionState.SKIPPED)
            {
                Debug.Log($"Ad status = {showCompletionState} - Reward Player for skipping {placementId} video");
                
                if (Multiplier)
                    RewardPlayerCalc(RewardSkippedMultiplier);
                else
                    RewardPlayerSum(RewardSkippedAmount);
            }
            
            ToggleBanner(false);
        }
        
        private void ToggleBanner(bool toggle)
        {
            if (_shouldReactivateBanner)
            {
                ShowBannerAd();
                _shouldReactivateBanner = toggle;
            }
            
            if (toggle) HideBannerAd();
            Time.timeScale = 1;
        }


        private void RewardPlayerSum(int rewardType)
        {
            Coins += rewardType;
            CoinText.text = $"{Coins}";
        }
        
        private void RewardPlayerCalc(float rewardType)
        {
            Coins = Mathf.CeilToInt(Coins * rewardType);
            CoinText.text = $"{Coins}";
        }
    }
}