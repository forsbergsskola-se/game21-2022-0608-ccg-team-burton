using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour, IEffects
{
    private Transform objTrans;
    public Vector2 travelVector;
    private bool startMoving;
    private float timeAlive;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        objTrans = gameObject.transform;
    }

    
    // Update is called once per frame
    public void EngageEffect()
    {
        Debug.Log("Effect engaged");
        anim.SetBool(Animator.StringToHash("FireballPlus"), true);
      //  anim.SetTrigger(Animator.StringToHash("Explosion"));
        startMoving = true;
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            other.gameObject.GetComponent<IDamageable>().ModifyHealth(-3);
        }
    }


    void Update()
    {
        if(startMoving)
            objTrans.position += objTrans.right * (Time.deltaTime * 2);

        timeAlive += Time.deltaTime;

        if (timeAlive > 8)
        {
            timeAlive -= 8;
            anim.SetBool(Animator.StringToHash("FireballPlus"), false);
            gameObject.SetActive(false);
        }
    }
}
