using UnityEngine;
using UnityEngine.Advertisements;

namespace Metagame
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string _androidID = "4876959";
        private const string _iOSId = "4876958";
        [SerializeField] private bool _testMode = true;
        [SerializeField] private string _adTargetSystemId;
        //public string myPlacementId = "rewardedVideo";
        [SerializeField] private string _androidReward = "Rewarded_Android";
        [SerializeField] private string _androidInterstitial = "Interstitial_Android";
        


        private void Awake()
        {
            // Get the Ad Unit ID for the current platform:
            _adTargetSystemId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSId
                : _androidID;

            Advertisement.Initialize(_androidID, _testMode, this);
        }


        public void PlayAd()
        {
            Debug.Log("Play interstitial Ad");
            Advertisement.Show(_androidInterstitial);
        }

        public void PlayRewardedAd()
        {
            Debug.Log("Play rewarded Ad");
            Advertisement.Show(_androidReward);
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
            throw new System.NotImplementedException();
        }

        
        
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            throw new System.NotImplementedException();
        }

        
        
        
        
        /// <summary>
        /// Ads OnShow Logic
        /// </summary>
        
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            throw new System.NotImplementedException();
        }

        
        
        
        public void OnUnityAdsShowStart(string placementId)
        {
            throw new System.NotImplementedException();
        }

        
        
        
        public void OnUnityAdsShowClick(string placementId)
        {
            throw new System.NotImplementedException();
        }
        
        
        

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId == _androidReward && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log("Reward Player for watching full reward video");
            }
        }
    }
}
