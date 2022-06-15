using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyVars_ML
{
    [SerializeField] private float AttackDistance;
    [SerializeField] private float PursueDistance;
    [SerializeField] private float MoveSpeed;
    public Enemy_Eyes _eyes;

    public float GetAttackDistance => AttackDistance;
    public float GetPursueDistance => PursueDistance;
    public float GetMoveSpeed => MoveSpeed;
}
