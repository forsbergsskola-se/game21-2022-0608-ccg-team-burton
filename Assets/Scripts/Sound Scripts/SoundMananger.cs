using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class SoundMananger : MonoBehaviour
{
    [Header("Music and Ambience")] public EventReference MusicTrack;

    string _currentMusic = "";

    bool isPitched;

    EventInstance MusicTrack_EventInst;

    // Start is called before the first frame update
    void Start(){
        StartMusic(MusicTrack);

        DontDestroyOnLoad(this);

    }

 

    public void SetMusicParam(float val){
        MusicTrack_EventInst.setParameterByName("test", val);
    }

    public void switchPitch(){
        if (isPitched) MusicTrack_EventInst.setParameterByName("test", 0f);
        else MusicTrack_EventInst.setParameterByName("test", 1f);
        isPitched = !isPitched;
    }

    public void StartMusic(EventReference MusicTrack){
        if (_currentMusic != MusicTrack.Guid.ToString()){
            _currentMusic = MusicTrack.Guid.ToString();
            StopMusic();
            MusicTrack_EventInst = RuntimeManager.CreateInstance(MusicTrack);
            MusicTrack_EventInst.start();
        }
    }

    public void StopMusic(){
        MusicTrack_EventInst.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void PlaySound(EventInstance sound){
        PLAYBACK_STATE PbState;
        sound.getPlaybackState(out PbState);
        if (PbState != PLAYBACK_STATE.PLAYING) sound.start();
    }

    public void PlayStackingSound(EventInstance sound){
        sound.start();
    }

    public void StopSound(EventInstance sound){
        sound.stop(STOP_MODE.IMMEDIATE);
    }

    public void ReleaseSound(EventInstance sound){
    }

    public void SubscribeSound(){
    }


    public void NextLevel(){
        Debug.Log("NextLevel");
        RuntimeManager.PlayOneShot("event:/Game Play/Info-Messages/NextLevel");
    }

    public void UpgrSissor(){
        Debug.Log("UpgrSissor");
        RuntimeManager.PlayOneShot("event:/Game Play/Info-Messages/UpgrSissor");
    }

    public void UpgrCeramic(){
        Debug.Log("UpgrCeramic");
        RuntimeManager.PlayOneShot("event:/Game Play/Info-Messages/UpgrCeramic");
    }

    public void SoldierIdle(){
        Debug.Log("SoldierIdle");
        RuntimeManager.PlayOneShot("event:/Game Play/Combat/Enemy/SoldierIdle");
    }

    public void BossWalk(){
        Debug.Log("BossWalk");
        RuntimeManager.PlayOneShot("event:/Game Play/Combat/Enemy/BossWalk");
    }

    public void PickUpDiamond(){
        Debug.Log("PickUpDiamond");
        RuntimeManager.PlayOneShot("event:/UI/PickUpDiamond");
    }

    public void PickUpCeramic(){
        Debug.Log("PickUpCeramic");
        RuntimeManager.PlayOneShot("event:/UI/PickUpCeramic");
    }

    public void LevelFailed(){
        Debug.Log("LevelFailed");
        RuntimeManager.PlayOneShot("event:/Game play/Info-messages/LevelFailed");
    }

    public void SoldierWalk(){
        Debug.Log("SoldierWalk");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/Enemy/SoldierWalk");
    }


    public void BossWhipAttack(){
        Debug.Log("BossWhipAttack");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/Enemy/BossWhipAttack");
    }

    public void EnemyWalk(){
        Debug.Log("EnemyWalk");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/Enemy/EnemyWalk");
    }


    public void RatWalk(){
        Debug.Log("RatWalk");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/Enemy/RatWalk");
    }

    public void RatAttack(){
        Debug.Log("RatAttack");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/Enemy/RatAttack");
    }


    public void EnemyIdle(){
        Debug.Log("EnemyIdle");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/EnemyIdle");
    }


    public void EnemyTakeDamage(){
        Debug.Log("EnemyTakeDamage");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/Enemy/EnemyTakeDamage");
    }


    public void SpoongJump(){
        Debug.Log("SpoongJump");
        RuntimeManager.PlayOneShot("event:/Game play/Objects/SpoongJump");
    }


    public void CanonEnemy(){
        Debug.Log("CanonEnemy");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/Enemy/CanonEnemy");
    }

    public void Walking(){
        Debug.Log("Walking");
        RuntimeManager.PlayOneShot("event:/Game play/Player/Walking");
    }


    public void EquipItem(){
        Debug.Log("EquipItem");
        RuntimeManager.PlayOneShot("event:/Game play/Objects/EquipItem");
    }

    public void PickUpCurrency(){
        Debug.Log("PickUpCurrency");
        RuntimeManager.PlayOneShot("event:/Game play/Objects/PickUpCurrency");
    }

    public void Jump(){
        Debug.Log("Jump");
        RuntimeManager.PlayOneShot("event:/Game play/Player/Jump");
    }

    public void Landing(){
        Debug.Log("Landing");
        RuntimeManager.PlayOneShot("event:/Game play/Player/Landing");
    }

    public void Fall(){
        Debug.Log("Fall");
        RuntimeManager.PlayOneShot("event:/Game play/Player/Fall");
    }

    public void Win(){
        Debug.Log("WinLevel");
        RuntimeManager.PlayOneShot("event:/Game play/Info-messages/WinLevel");
    }

    public void SelectButton(){
        Debug.Log("SelectButton");
        RuntimeManager.PlayOneShot("event:/Ui/SelectButton");
    }

    public void SelectButton2(){
        Debug.Log("SelectButton2");
        RuntimeManager.PlayOneShot("event:/Ui/SelectButton2");
    }

    public void SelectButton3(){
        Debug.Log("SelectButton3");
        RuntimeManager.PlayOneShot("event:/Ui/SelectButton3");
    }

    public void SelectButton4(){
        Debug.Log("SelectButton4");
        RuntimeManager.PlayOneShot("event:/Ui/SelectButton4");
    }

    public void PlayerIdle(){
        Debug.Log("PlayerIdle");
        RuntimeManager.PlayOneShot("event:/Game play/Player/PlayerIdle");
    }

    public void LootCrateDestroy(){
        Debug.Log("LootCrateDestroyd");
        RuntimeManager.PlayOneShot("event:/Game play/Objects/LootCrateGetDestroyed");
    }

    public void AttackLootCrate(){
        Debug.Log("AttackingLootCrate");
        RuntimeManager.PlayOneShot("event:/Game play/Objects/AttackingLootCrate");
    }

    public void TakeDmg(){
        Debug.Log("TakingDamage");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/TakingDamage");
    }

    public void Dies(){
        Debug.Log("DiesFromDamage");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/DiesFromDamage");
    }

    public void Attack1(){
        Debug.Log("AcidAttack");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/AcidAttack");
    }

    public void Attack2(){
        Debug.Log("SissorAcidAttack");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/SissorAcidAttack");
    }

    public void Attack3(){
        Debug.Log("SissorAttack");
        RuntimeManager.PlayOneShot("event:/Game play/Combat/SissorAttack");
    }

    public void OpenCrate(){
        Debug.Log("OpenLootCrate1");
        RuntimeManager.PlayOneShot("event:/Meta Game/Lootbox/OpenLootCrate1");
    }

    public void OpenCrate2(){
        Debug.Log("OpenLootCrate2");
        RuntimeManager.PlayOneShot("event:/Meta Game/Lootbox/OpenLootCrate2");
    }

    public void OpenCrate3(){
        Debug.Log("OpenLootCrate3");
        RuntimeManager.PlayOneShot("event:/Meta Game/Lootbox/OpenLootCrate3");
    }
}