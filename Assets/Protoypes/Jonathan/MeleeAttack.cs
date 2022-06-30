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

 bool _allowAttack = true;

 void Awake(){
  _commandContainer = FindObjectOfType<CommandContainer>();

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

  _allowAttack = false;
  yield return new WaitForSeconds(_attackDelaySec);
  _allowAttack = true;
 }
}
