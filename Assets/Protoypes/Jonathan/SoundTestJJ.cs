using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTestJJ : MonoBehaviour
{
     private SoundMananger _soundmanager;
     
     public FMODUnity.EventReference TestSound1File;
     private FMOD.Studio.EventInstance _test1Sound;    
     public FMODUnity.EventReference TestSound2File;
     private FMOD.Studio.EventInstance _test2Sound;

     private void Start()
     {
          _soundmanager = FindObjectOfType<SoundMananger>();
          _test1Sound = FMODUnity.RuntimeManager.CreateInstance(TestSound1File);
          _test2Sound = FMODUnity.RuntimeManager.CreateInstance(TestSound2File);

     }

     public void PlaySoundOne()
     {
          _soundmanager.PlaySound(_test1Sound);
     }
     public void PlaySoundTwo()
     {
          _soundmanager.PlaySound(_test2Sound);
     }
}
