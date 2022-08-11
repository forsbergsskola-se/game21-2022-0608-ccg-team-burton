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
        }

        public void SetVolume(float volume){
            VcaController.setVolume(volume);
        }

        public void ToggleVibration(){
            
        }
    }
}
