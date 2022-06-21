using System;
using System.Collections;
using UnityEngine;

public class MeleeAttackTestJJ : MonoBehaviour
{
 [SerializeField]
 private Combat _combat;
 [SerializeField] private float _attackDelaySec = 2f;

 SlowMotionEffect slowMotionEffect;
 private bool _allowAttack = true;

 void Awake(){
  slowMotionEffect = GetComponent<SlowMotionEffect>();
 }

 private void Update()
 {
  if (Input.GetKeyDown(KeyCode.E) && _allowAttack)
    StartCoroutine(CallAttack());
 }


 IEnumerator CallAttack()
 {
  _combat.MeleeAttack();
  slowMotionEffect.StartSlowMotion();
  slowMotionEffect.StopSlowMotion();
  _allowAttack = false;
  yield return new WaitForSeconds(_attackDelaySec);
  _allowAttack = true;
 }
}
