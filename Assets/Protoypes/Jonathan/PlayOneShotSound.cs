using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class PlayOneShotSound : MonoBehaviour
{
    public FMODUnity.EventReference SoundFile;

    private SoundMananger _soundMananger;
    private FMOD.Studio.EventInstance _sound;
    private bool initiated;

    private void Awake()
    {
        _soundMananger = FindObjectOfType<SoundMananger>();
    }

    public void PlaySound()
    {
      
        if (!initiated)
        {
            _sound = FMODUnity.RuntimeManager.CreateInstance(SoundFile);
            initiated = true;
        }

        _soundMananger.PlaySound(_sound);
    }

    public void PlayStackingSound()
    {
        _sound = FMODUnity.RuntimeManager.CreateInstance(SoundFile);
        _soundMananger.PlayStackingSound(_sound);
    }
    
    public void PlayStackingSound(EventReference SoundFile)
    {
        _sound = FMODUnity.RuntimeManager.CreateInstance(SoundFile);
        // _soundMananger.PlayStackingSound(_sound);
    }
}
