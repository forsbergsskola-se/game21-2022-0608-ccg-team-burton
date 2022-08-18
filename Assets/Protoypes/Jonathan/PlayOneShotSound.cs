using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class PlayOneShotSound : MonoBehaviour
{
    public FMODUnity.EventReference SoundFile;

    public SoundMananger _soundMananger;
    private FMOD.Studio.EventInstance _sound;
    private bool initiated;

    private void Awake()
    {
        _soundMananger = FindObjectOfType<SoundMananger>();
    }

    public void PlaySound()
    {
        
        if (_soundMananger != null)
        { 
            if (!initiated)
            {
                _sound = FMODUnity.RuntimeManager.CreateInstance(SoundFile);
                initiated = true;
            }
          
            _soundMananger.PlaySound(_sound);
        }else
        {
            Debug.Log("NO SOUNDMANAGER PRESENT");
        }
    }
    
    public void PlaySound(EventReference SoundFile)
    {
      

        if (_soundMananger != null)
        {  
            if (!initiated)
            {
                _sound = FMODUnity.RuntimeManager.CreateInstance(SoundFile);
                initiated = true;
            }
           
            _soundMananger.PlaySound(_sound);
            
        }
        else
        {
            Debug.Log("NO SOUNDMANAGER PRESENT");
        }
    }

    public void PlayStackingSound()
    {

        if (_soundMananger != null)
        {
            _sound = FMODUnity.RuntimeManager.CreateInstance(SoundFile);

            _soundMananger.PlayStackingSound(_sound); 
        }else
        {
            Debug.Log("NO SOUNDMANAGER PRESENT");
        }
    }
    
    public void PlayStackingSound(EventReference SoundFile)
    {

        if (_soundMananger != null)
        {
            _sound = FMODUnity.RuntimeManager.CreateInstance(SoundFile);

            _soundMananger.PlayStackingSound(_sound);
            
        }
        else
        {
            Debug.Log("NO SOUNDMANAGER PRESENT");

        }
    }
}
