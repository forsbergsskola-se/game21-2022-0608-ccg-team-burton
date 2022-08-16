using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotSound : MonoBehaviour
{
    [SerializeField] private SoundMananger _soundMananger;
    public FMODUnity.EventReference SoundFile;
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
}
