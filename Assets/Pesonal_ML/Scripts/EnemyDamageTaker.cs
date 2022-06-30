using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class EnemyDamageTaker : MonoBehaviour
{
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
      var health =  GetComponent<Health>();
      health.OnHealthChanged += TakeDamage;

    }

    private void OnDisable()
    {
        var health =  GetComponent<Health>();
        health.OnHealthChanged -= TakeDamage;
    }

    private void TakeDamage(int currentHealth)
    {
        Debug.Log("Damage taken");
        if (currentHealth > 0)
        {
            _animator.SetTrigger(Animator.StringToHash("Enemy_Hit"));
        }
        else
        {
            StartCoroutine(DoDeath());
        }
        
    }

    IEnumerator DoDeath()
    {
        _animator.SetTrigger(Animator.StringToHash("Rat_Dead"));
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

}
