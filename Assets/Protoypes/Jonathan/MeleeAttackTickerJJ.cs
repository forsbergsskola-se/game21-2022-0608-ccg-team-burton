using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTickerJJ : MonoBehaviour
{
 [SerializeField]
 private Combat _combat;

 private bool _doAttack = true;
 
 
 private void Update()
 {
  if (_doAttack)
  {
   StartCoroutine(CallAttack());
   
  }
 }


 IEnumerator CallAttack()
 {
  _combat.MeeleAttack();
  _doAttack = false;
  yield return new WaitForSeconds(2f);
  _doAttack = true;
 }
 
 
 
}
