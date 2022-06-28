using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Mono.Cecil;
using UnityEngine;

public class SoundMananger : MonoBehaviour
{
    [Header("Music and Ambience")]
    public FMODUnity.EventReference MusicTrack;

    private FMOD.Studio.EventInstance MusicTrack_EventInst;

    bool isPitched = false;

    // Start is called before the first frame update
    void Start()
    {
        //music
        //MusicTrack1_EventInst = FMODUnity.RuntimeManager.CreateInstance(MusicTrack1_EventRef);
        //MusicTrack1_EventInst.start();
        
        StartMusic(MusicTrack);

        //DoThing();
        
        DontDestroyOnLoad(this);
    }

    public void SetMusicParam(float val)
    {
        MusicTrack_EventInst.setParameterByName("test", val);
    }

    public void switchPitch()
    {
        if (isPitched) MusicTrack_EventInst.setParameterByName("test", 0f);
        else MusicTrack_EventInst.setParameterByName("test", 1f);
        isPitched = !isPitched;
    }

    public void StartMusic(FMODUnity.EventReference MusicTrack){
        StopMusic();
        MusicTrack_EventInst = FMODUnity.RuntimeManager.CreateInstance(MusicTrack);
        MusicTrack_EventInst.start();
    }

    public void StopMusic(){
        MusicTrack_EventInst.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void PlaySound(FMOD.Studio.EventInstance sound)
    {
        FMOD.Studio.PLAYBACK_STATE PbState;
        sound.getPlaybackState(out PbState);
        if (PbState != PLAYBACK_STATE.PLAYING)
        {
            sound.start();
        }
    }

    public void StopSound(FMOD.Studio.EventInstance sound)
    {
        FMOD.Studio.PLAYBACK_STATE PbState;
        sound.getPlaybackState(out PbState);
        if (PbState == PLAYBACK_STATE.PLAYING)
        {
            sound.stop(STOP_MODE.IMMEDIATE);
        }
    }
    


    public void EquipItem()
    {
        Debug.Log("Walking");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Player/Walking");
    }


    public void EquipItem()
    {
        Debug.Log("EquipItem");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Objects/EquipItem");
    }

    public void PickUpCurrency()
    {
        Debug.Log("PickUpCurrency");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Objects/PickUpCurrency");
    }

    public void Jump()
    {
        Debug.Log("Jump");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Player/Jump");
    }

    public void Landing()
    {
        Debug.Log("Landing");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Player/Landing");
    }

    public void Fall()
    {
        Debug.Log("Fall");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Player/Fall");
    }

    public void Win()
    {
        Debug.Log("WinLevel");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Info-messages/WinLevel");
    }

    public void SelectButton()
    {
        Debug.Log("SelectButton");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Ui/SelectButton");
    }

    public void PlayerIdle()
    {
        Debug.Log("PlayerIdle");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Player/PlayerIdle");
    }

    public void LootCrateDestroy()
    {
        Debug.Log("LootCrateDestroyd");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Objects/LootCrateGetDestroyed");
    }

    public void AttackLootCrate()
    {
        Debug.Log("AttackingLootCrate");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Objects/AttackingLootCrate");
    }

    public void TakeDmg()
    {
        Debug.Log("TakingDamage");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Combat/TakingDamage");
    }

    public void Dies()
    {
        Debug.Log("DiesFromDamage");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Combat/DiesFromDamage");
    }

    public void Attack1()
    {
        Debug.Log("AcidAttack");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Combat/AcidAttack");
    }

    public void Attack2()
    {
        Debug.Log("SissorAcidAttack");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Combat/SissorAcidAttack");
    }

    public void Attack3()
    {
        Debug.Log("SissorAttack");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Combat/SissorAttack");
    }

    public void OpenCrate()
    {
        Debug.Log("OpenLootCrate1");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Meta Game/Lootbox/OpenLootCrate1");
    }

    public void OpenCrate2()
    {
        Debug.Log("OpenLootCrate2");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Meta Game/Lootbox/OpenLootCrate2");
    }
}  




