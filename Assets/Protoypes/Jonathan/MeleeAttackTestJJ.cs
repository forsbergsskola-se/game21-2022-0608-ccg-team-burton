using System;
using System.Collections;
using GamePlay.Entities.Player;
using UnityEngine;

public class MeleeAttackTestJJ : MonoBehaviour
{
 [SerializeField] Combat _combat;
 [SerializeField] float _attackDelaySec = 2f;

 SlowMotionEffect slowMotionEffect;
 CommandContainer _commandContainer;
 bool _allowAttack = true;

 void Awake(){
  slowMotionEffect = GetComponent<SlowMotionEffect>();
  _commandContainer = FindObjectOfType<CommandContainer>();
 }

 private void Update()
 {
  if (_commandContainer.AttackDownCommand && _allowAttack)
    StartCoroutine(CallAttack());
 }


 IEnumerator CallAttack()
 {
  _combat.MeleeAttack();
  slowMotionEffect.StartSlowMotion();
  
  _allowAttack = false;
  yield return new WaitForSeconds(_attackDelaySec);
  _allowAttack = true;
  slowMotionEffect.StopSlowMotion();
 }
}
