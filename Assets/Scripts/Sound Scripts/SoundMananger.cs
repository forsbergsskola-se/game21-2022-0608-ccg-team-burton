using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMananger : MonoBehaviour
{
    [Header("Music and Ambience")]
    public FMODUnity.EventReference MusicTrack1_EventRef;

    private FMOD.Studio.EventInstance MusicTrack1_EventInst;

    bool isPitched = false;

    // Start is called before the first frame update
    void Start()
    {
        //music
        MusicTrack1_EventInst = FMODUnity.RuntimeManager.CreateInstance(MusicTrack1_EventRef);
        MusicTrack1_EventInst.start();

        //DoThing();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Hello testing");
            //SetMusicParam(0.3f);
            //switchPitch();
        }
    }
    public void DoThing()
    {
        Debug.Log("DoThing");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Pick up currency");
    }

    public void SetMusicParam(float val)
    {
        MusicTrack1_EventInst.setParameterByName("test", val);
    }

    public void switchPitch()
    {
        if (isPitched) MusicTrack1_EventInst.setParameterByName("test", 0f);
        else MusicTrack1_EventInst.setParameterByName("test", 1f);
        isPitched = !isPitched;
    }
 }  




