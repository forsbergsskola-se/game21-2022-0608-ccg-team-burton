using System;
using UnityEngine;
using UnityEngine.UI;

namespace Metagame
{
    public class SettingsHandler : MonoBehaviour
    {
        FMOD.Studio.VCA VcaController;

        [SerializeField] string VcaName;
        [SerializeField] Slider slider;

        void Start(){
            VcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);
            slider = GetComponent<Slider>();
            LoadMusicVolume();
            LoadSFXVolume();
            VcaController.setVolume(PlayerPrefs.GetFloat("MusicVolume"));
            VcaController.setVolume(PlayerPrefs.GetFloat("SFXVolume"));
        }

        public void SetMusicVolume(float volume){
            VcaController.setVolume(volume);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        public void LoadMusicVolume(){
            if (gameObject.name.Contains("Music")){
                slider.value = PlayerPrefs.GetFloat("MusicVolume");
            }
        }
        public void LoadSFXVolume(){
            if (gameObject.name.Contains("SFX")){
                slider.value = PlayerPrefs.GetFloat("SFXVolume");
            }
        }
        
        
        public void SetSFXVolume(float volume){
            VcaController.setVolume(volume);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }
        

        public void ToggleVibration(){
            
        }
    }
}
