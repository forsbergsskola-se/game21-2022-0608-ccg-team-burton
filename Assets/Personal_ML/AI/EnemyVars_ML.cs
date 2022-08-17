using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;


[Serializable]
public class EnemyVars_ML
{
    [SerializeField] private float AttackDistance;
    [SerializeField] private float PursueDistance;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float AttackInterval;

    
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject enemyRef;
    [HideInInspector] public Transform firePoint;
    [HideInInspector] public TracerEyes tracerEyes;
    public IDamageable enemyHealth;

    public float GetAttackDistance => AttackDistance;
    public float GetPursueDistance => PursueDistance;
    public float GetMoveSpeed => MoveSpeed;
    public float GetAttackInterval => AttackInterval;
}

[Serializable]
public abstract class EnemyVarsBase
{
    [SerializeField] protected float AttackDistance;
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected float AttackInterval;

}

[Serializable]
public class EnemyVarsTurret : EnemyVarsBase
{
    public float GetAttackDistance()
        => AttackDistance;

    public float GetMoveSpeed()
        => MoveSpeed;

    public float GetAttackInterval()
        => AttackInterval;
}

