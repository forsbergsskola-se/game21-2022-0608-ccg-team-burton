using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTestJJ : MonoBehaviour
{
 [SerializeField]
 private Combat _combat;

 private bool _allowAttack = true;
 
 
 private void Update()
 {
  if (Input.GetKeyDown(KeyCode.E) && _allowAttack)
  {
   StartCoroutine(CallAttack());
   
  }
 }


 IEnumerator CallAttack()
 {
  _combat.MeeleAttack();
  _allowAttack = false;
  yield return new WaitForSeconds(2f);
  _allowAttack = true;
 }
 
 
 
}
