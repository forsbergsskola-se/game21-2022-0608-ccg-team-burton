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
    FindCommander,
    MoveToPosition,
    Interact,
    PickupItem,
    DropOfItem,
    GetInstructions,
    SearchArea,
    Gather,
    Hunt,
    Find
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
    public Instruction currentInstruction;
    public Queue<Vector3> TargetQueue = new();

    public Queue<CurrentCommand> commandQueue = new();

    public Animator anim;
    public bool pathBlocked;

    public bool commanderReached;

    public AiAgent()
    {
        currentDestination = new Vector3(0, 0, 0);
    }
}