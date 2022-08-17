using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

namespace Metagame
{
    public class SettingsHandler : MonoBehaviour
    {
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;
        [SerializeField] Toggle toggle;

        VCA _musicVcaController;
        VCA _sfxVcaController;

        private void Start(){
            _musicVcaController = RuntimeManager.GetVCA("vca:/Music");
            _sfxVcaController = RuntimeManager.GetVCA("vca:/SFX");
            Debug.Log(musicSlider.gameObject.name);
            LoadMusicVolume();
            LoadSFXVolume();
            if (PlayerPrefs.GetInt("ToggleVibrate") == 1)
                toggle.isOn = true;
            else
                toggle.isOn = false;
            _musicVcaController.setVolume(PlayerPrefs.GetFloat("MusicVolume"));
            _sfxVcaController.setVolume(PlayerPrefs.GetFloat("SFXVolume"));
        }

        public void SetMusicVolume(float volume){
            _musicVcaController.setVolume(volume);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        public void SetSFXVolume(float volume){
            _sfxVcaController.setVolume(volume);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }

        public void LoadMusicVolume(){

            if (musicSlider.gameObject.name.Contains("Music")) musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        public void LoadSFXVolume(){
            if (sfxSlider.gameObject.name.Contains("SFX")) sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }


        public void ToggleVibrate(){
            if (toggle.isOn){
                #if UNITY_ANDROID
                Handheld.Vibrate();
                #endif
                PlayerPrefs.SetInt("ToggleVibrate", 1);
            }
        }
    }
}