using System;
using System.Collections;
using GamePlay.Entities.Player;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
 //PRIVATE VARIABLES
 [SerializeField] Combat _combat;
 private float _finalAttackDelay;
 [SerializeField] float _attackDelaySec = 2f;
 CommandContainer _commandContainer;
 private SoundMananger _soundMananger;
 bool _allowAttack = true;

 void Awake(){
  _commandContainer = FindObjectOfType<CommandContainer>();

 }

 private void Start()
 {
  _finalAttackDelay = _attackDelaySec / (1+(PlayerPrefs.GetFloat("buequipment.head.attributevalue")/100));
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
  yield return new WaitForSeconds(_finalAttackDelay);
  _allowAttack = true;
 }
}
