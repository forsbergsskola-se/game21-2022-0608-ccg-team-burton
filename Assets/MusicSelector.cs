using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelector : MonoBehaviour
{
    private SoundMananger _soundMananger => FindObjectOfType<SoundMananger>();
    public FMODUnity.EventReference SoundFile;

    void Start()
    {
        _soundMananger.StartMusic(SoundFile);
    }
}
