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
            switchPitch();
        }
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



    public void EquipItem()
    {
        Debug.Log("Equip item");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Objects/Equip item");
    }

    public void PickUpCurrency()
    {
        Debug.Log("PickUpCurrency");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Pick up currency");
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
        Debug.Log("Win");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Info-messages/Win");
    }

    public void SelectButton()
    {
        Debug.Log("SelectButton");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Ui/Select button");
    }

    public void PlayerIdle()
    {
        Debug.Log("PlayerIdle");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Player/Player idle");
    }

    public void LootCrateDestroy()
    {
        Debug.Log("LootCrateDestroy");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Objects/Loot crate get's destroyed");
    }

    public void AttackLootCrate()
    {
        Debug.Log("AttackLootCrate");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Objects/Attacking loot crate");
    }

    public void TakeDmg()
    {
        Debug.Log("TakeDmg");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Combat/Taking damage");
    }

    public void Dies()
    {
        Debug.Log("Dies");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Combat/Dies");
    }

    public void Attack1()
    {
        Debug.Log("Attack1");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Combat/Attack type 1");
    }

    public void Attack2()
    {
        Debug.Log("Attack2");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Combat/Attack type 2");
    }

    public void Attack3()
    {
        Debug.Log("Attack3");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Combat/Attack type 3");
    }

    public void OpenCrate()
    {
        Debug.Log("OpenCrate");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Meta Game/Lootbox/Open Gacha crate Type 1");
    }

    public void OpenCrate2()
    {
        Debug.Log("OpenCrate2");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Meta Game/Lootbox/Open Gacha crate Type 2");
    }
}  




