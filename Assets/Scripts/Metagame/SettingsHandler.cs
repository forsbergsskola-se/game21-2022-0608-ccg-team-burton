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
            slider.value = PlayerPrefs.GetFloat("MusicVolume");
            slider.value = PlayerPrefs.GetFloat("SFXVolume");
            VcaController.setVolume(PlayerPrefs.GetFloat("MusicVolume"));
            VcaController.setVolume(PlayerPrefs.GetFloat("SFXVolume"));
        }

        public void SetMusicVolume(float volume){
            VcaController.setVolume(volume);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
        
        public void SetSFXVolume(float volume){
            VcaController.setVolume(volume);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }
        

        public void ToggleVibration(){
            
        }
    }
}
