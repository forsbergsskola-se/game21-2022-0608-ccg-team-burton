using UnityEngine;
using UnityEngine.Advertisements;

namespace Metagame
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        private const string _androidID = "4876959";
        private const string _iOSId = "4876958";
        [SerializeField] private bool _testMode = true;
        [SerializeField] private string _adTargetSystemId;
        //public string myPlacementId = "rewardedVideo";


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
            Advertisement.Show("Interstitial Android");
        }

        public void PlayRewardedAd()
        {
            Debug.Log("Play rewarded Ad");
            Advertisement.Show("Rewarded Android");
        }

        
        
        public void OnInitializationComplete()
        { 
            Debug.Log("Unity Ads initialization complete.");
        }

        
        
        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}
