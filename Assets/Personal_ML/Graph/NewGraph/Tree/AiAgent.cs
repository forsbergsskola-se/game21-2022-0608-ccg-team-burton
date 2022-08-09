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
    OutOfCommands,
    Jump
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
    public Transform destination;
    public CurrentCommand currentCommand;

    public Vector3 currentDestination;
    public Queue<Vector2> TargetQueue = new();

    public Queue<CurrentCommand> commandQueue = new();
    public Animator anim;
    public LevelGrid grid;

    public bool quitNode;

    public TracerEyes enemyEyes;
    
    public Rigidbody2D body;

    public bool keepWalking;
    
    public AiAgent()
    {
        currentDestination = new Vector3(0, 0, 0);
    }
}