using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackTestJJ : MonoBehaviour
{
 [SerializeField]
 private Combat _combat;

 private bool _allowAttack = true;
 
 
 private void Update()
 {
  if (_allowAttack)
  {
   StartCoroutine(CallAttack());
   
  }
 }


 IEnumerator CallAttack()
 {
  _combat.RangedAttack();
  _allowAttack = false;
  yield return new WaitForSeconds(2f);
  _allowAttack = true;
 }
 
 
 
}
