using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Melee,
    Ranged,
    Turret
}

[Serializable]
public class EnemyVars_ML
{
    [SerializeField] private float AttackDistance;
    [SerializeField] private float PursueDistance;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float AttackInterval;
    [SerializeField] EnemyType EnemyType;
    
    [HideInInspector] public Enemy_Eyes _eyes;
    [HideInInspector] public ArcCollider ArcCollider;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject enemyRef;

    public float GetAttackDistance => AttackDistance;
    public float GetPursueDistance => PursueDistance;
    public float GetMoveSpeed => MoveSpeed;
    public float GetAttackInterval => AttackInterval;
    public EnemyType GetEnemyType => EnemyType;
}
