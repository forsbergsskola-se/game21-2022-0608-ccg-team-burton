using System;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    Idle, Patrol, Pursue, Attack, Rest, Jump
}

public enum CurrentCommand
{
    None,
    MoveToPosition,
    MoveToAttack,
    OutOfCommands,
    Jump,
    Attack,
    Idle
}

[Serializable]
public class Instruction
{
    public Vector3 finalDestination;
}

[Serializable]
public class AiAgent
{
    public Transform enemyTransform;
    public Vector3 currentDestination;
    
    public Animator anim;
    public LevelGrid grid;

    public TracerEyes enemyEyes;
    
    public Rigidbody2D body;
    
    public float moveSpeed;
    public float attackInterval = 0.3f;
    
    public CompoundActions compoundAction;

    public float turnDistance = 1f;

    public Transform attackPointTrans;

    public int damageAmount = 1;

    public float attackRange = 2;

    public AssetPool assetPool;

    public float projectileLifespan;
    
    public AiAgent()
    {
        currentDestination = new Vector3(0, 0, 0);
    }
}