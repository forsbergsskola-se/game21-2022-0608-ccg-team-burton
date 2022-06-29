using System;
using System.Collections;
using GamePlay.Entities.Player;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
 [SerializeField] Combat _combat;
 [SerializeField] float _attackDelaySec = 2f;
 CommandContainer _commandContainer;
 private SoundMananger _soundMananger;
 public FMODUnity.EventReference AttackSoundFile;
 private FMOD.Studio.EventInstance _attackSound;
 
 bool _allowAttack = true;

 void Awake(){
  _commandContainer = FindObjectOfType<CommandContainer>();
  _soundMananger = FindObjectOfType<SoundMananger>();
 }

 private void Start()
 {
  _attackSound = FMODUnity.RuntimeManager.CreateInstance(AttackSoundFile);
 }

 private void Update()
 {
  if (!_allowAttack) return;
  
  if (_commandContainer.AttackDownCommand || _commandContainer.AttackMouseCommand)
    StartCoroutine(CallAttack());
 }


 IEnumerator CallAttack()
 {
  _combat.MeleeAttack();
  _soundMananger.PlaySound(_attackSound);
  
  _allowAttack = false;
  yield return new WaitForSeconds(_attackDelaySec);
  _allowAttack = true;
 }
}
