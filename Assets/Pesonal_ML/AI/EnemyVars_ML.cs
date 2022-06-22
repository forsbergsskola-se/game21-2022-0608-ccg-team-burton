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
    public EnemyType EnemyType;
    
    [HideInInspector] public ArcCollider ArcCollider;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject enemyRef;
    [HideInInspector] public Transform firePoint;
    [HideInInspector] public TracerEyes tracerEyes;

    public float GetAttackDistance => AttackDistance;
    public float GetPursueDistance => PursueDistance;
    public float GetMoveSpeed => MoveSpeed;
    public float GetAttackInterval => AttackInterval;
    public EnemyType GetEnemyType => EnemyType;
}

[Serializable]
public abstract class EnemyVarsBase
{
    [SerializeField] protected float AttackDistance;
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected float AttackInterval;
    [SerializeField] protected EnemyType EnemyType;
    
}

[Serializable]
public class EnemyVarsTurret : EnemyVarsBase, IEnemyVars
{
    public float GetAttackDistance()
        => AttackDistance;

    public float GetMoveSpeed()
        => MoveSpeed;

    public float GetAttackInterval()
        => AttackInterval;

    public EnemyType GetEnemyType()
        => EnemyType;
}

public interface IEnemyVars
{
    public float GetAttackDistance();
    public float GetMoveSpeed();
    public float GetAttackInterval();

    public EnemyType GetEnemyType();
    
    

}