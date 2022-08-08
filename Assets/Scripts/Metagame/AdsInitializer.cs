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

        private void Awake()
        {
            // Get the Ad Unit ID for the current platform:
            _adTargetSystemId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSId
                : _androidID;

            Advertisement.Initialize(_adTargetSystemId, _testMode, this);
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
