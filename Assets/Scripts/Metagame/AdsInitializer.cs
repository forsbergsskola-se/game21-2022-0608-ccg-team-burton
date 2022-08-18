using System.Collections;
using TMPro;
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

        private bool _showBanner;
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

        #region Reward Settings
        [Header("Reward Type")]
        public bool Multiplier = true;

        public bool InterstitialAd = false;
        public bool RewardAd = false;
        public bool BannerAd = false;
        public TextMeshProUGUI coinText;
        public TextMeshProUGUI totalCoinText;
        private ItemCollector itemCollector;
        private int Coins;
        #endregion

        #region Reward Sums;
        [Header("Reward Sums")]
        public int InterstitialAmount;
        public int RewardCompleteAmount;
        public int RewardSkippedAmount;
        #endregion
        
        #region Reward Multipliers;
        [Header("Reward Multipliers")]
        public float InterstitialMultiplier;
        public float RewardCompleteMultiplier;
        public float RewardSkippedMultiplier;
        #endregion
        
        private void Start()
        {
            itemCollector = FindObjectOfType<ItemCollector>().gameObject.GetComponent<ItemCollector>();
            //if (!Advertisement.isInitialized)
                InitializeAds();

            AssignButtons();

            if (!ShowBannerOnStartup) return;
            LoadBannerAd();
            ShowBannerAd();
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
            if (InterstitialAd)
            {
                ShowInterstitialAdButton.onClick.AddListener(LoadInterstitialAd);
                ShowInterstitialAdButton.interactable = true;
            }

            if (RewardAd)
            {
                ShowRewardAdButton.onClick.AddListener(LoadRewardedAd);
                ShowRewardAdButton.interactable = true;
            }

            if (!BannerAd) return;
            
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
            
            _showBanner = true;
            if (Advertisement.Banner.isLoaded == false)
                StartCoroutine(RepeatShowBanner());
            
            Advertisement.Banner.Show(_bannerID);
        }



        private void HideBannerAd()
        {
            _showBanner = false;
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
        
        
        
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Coins = itemCollector._coinCounter;

            if (placementId == _interstitialID && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
                CalculateReward(placementId, showCompletionState, "watching", InterstitialMultiplier, InterstitialAmount);


            else if (placementId == _rewardID && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
                CalculateReward(placementId, showCompletionState, "completing", RewardCompleteMultiplier, RewardCompleteAmount);


            else if (placementId == _interstitialID && showCompletionState == UnityAdsShowCompletionState.SKIPPED)
                CalculateReward(placementId, showCompletionState, "skipping", RewardSkippedMultiplier, RewardSkippedAmount);

            ToggleBanner(false);
        }

        

        private void CalculateReward(string placementId, UnityAdsShowCompletionState showCompletionState, 
            string message, float completionMultiplier, int completionAmount)
        {
            Debug.Log($"Ad status = {showCompletionState} - Reward Player for {message} {placementId} video");
            if (Multiplier)
                RewardPlayerCalc(completionMultiplier);
            else
                RewardPlayerSum(completionAmount);
        }
        
        
        
        private void ToggleBanner(bool toggle)
        {
            if (_shouldReactivateBanner)
            {
                ShowBannerAd();
                _shouldReactivateBanner = toggle;
            }
            
            if (toggle) HideBannerAd();
        }


        private void RewardPlayerSum(int rewardType)
        {
            Debug.Log($"Coins before Ad {Coins}");
            Debug.Log($"Total Coins before Ad {PlayerPrefsKeys.CurrentCoins.ToString()}");

            Coins += rewardType;
            coinText.text = $"{Coins}";
            totalCoinText.text = $"{Coins}";
            Debug.Log($"Coins after Ad {Coins}");
            Debug.Log($"Total Coins before Ad {PlayerPrefsKeys.CurrentCoins.ToString()}");

            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) + rewardType);
        }
        
        
        
        private void RewardPlayerCalc(float rewardType)
        {
            Debug.Log($"Coins before Ad {Coins}");
            Debug.Log($"Total Coins before Ad {PlayerPrefsKeys.CurrentCoins.ToString()}");
            
            var newCoinValue = Mathf.CeilToInt(Coins * rewardType);
            coinText.text = $"{newCoinValue}";
            totalCoinText.text = $"Total Coins : {newCoinValue}";
            Debug.Log($"Coins after Ad {newCoinValue}");
            var difference = newCoinValue - Coins;
            Debug.Log($"Difference = {difference}");
            
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) + difference);
        }
    }
}