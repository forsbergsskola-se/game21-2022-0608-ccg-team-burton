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

    public bool quitNode;

    public TracerEyes enemyEyes;
    
    public Rigidbody2D body;
    
    public float moveSpeed;
    public float attackInterval = 0.3f;
    
    public Action<Action<CompoundActions>> CheckForJump;
    
    public CompoundActions compoundAction;
    
    public GameObject projectile;

    public float turnDistance = 1.5f;

    public Transform attackPointPos;
    
    public AiAgent()
    {
        currentDestination = new Vector3(0, 0, 0);
    }
}