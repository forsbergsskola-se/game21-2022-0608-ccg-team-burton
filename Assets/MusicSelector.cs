using FMODUnity;
using UnityEngine;

public class MusicSelector : MonoBehaviour
{
    public EventReference SoundFile;
    SoundMananger _soundMananger => FindObjectOfType<SoundMananger>();

    void Start(){
        _soundMananger.StartMusic(SoundFile);
    }

    void ReleaseAll(){
    }
}