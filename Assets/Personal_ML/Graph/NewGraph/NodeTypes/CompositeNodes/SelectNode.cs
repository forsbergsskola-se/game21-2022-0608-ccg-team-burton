using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NewGraph.NodeTypes.ActionNodes;
using UnityEditor;
using UnityEngine;

public class SelectNode : CompositeNode
{
    [HideInInspector] public bool choiceMade;
    [HideInInspector] public CurrentCommand currentCommand;
    [HideInInspector] public STATE nextState;
    private Dictionary<CurrentCommand, BaseNode> ownedNodes = new();
    
    private CubeFacts _current;
    private bool _plusMinus;

    private bool _choiceMade;

    public override void OnStart()
    {
        SetPossibleNodes();
        CheckOptions();
        choiceMade = true;
        currentCommand = CurrentCommand.MoveToPosition;
    }
    
    private void CheckOptions()
    {
        var comp = CompoundActions.None;
        
        
        agent.CheckForJump?.Invoke(x =>
        {
            comp = x;
        });
        
        
        Debug.Log("Checking options");
        agent.keepWalking = true;
        if (!agent.enemyEyes.GroundSeen)
        {
            Debug.Log(agent.enemyEyes.compoundActions);
            if (agent.enemyEyes.compoundActions.HasFlag(CompoundActions.CantJump))
            {
                currentCommand = CurrentCommand.MoveToPosition;
                choiceMade = true;
                return;
            }

            currentCommand = CurrentCommand.Jump;
            choiceMade = true;
            return;
        }
        
        agent.currentDestination = agent.enemyEyes.PlayerPos;
        var playerEncounter = agent.enemyEyes.playerEncounter;

        if (playerEncounter.HasFlag(PlayerEncounter.PlayerNoticed))
        {
            if (playerEncounter.HasFlag(PlayerEncounter.PlayerInFront))
            {
                if (playerEncounter.HasFlag(PlayerEncounter.PlayerInAttackRange))
                {
                    currentCommand = CurrentCommand.Attack;
                    return;
                }

                agent.keepWalking = false;
                currentCommand = CurrentCommand.MoveToPosition;
                return;
            }
            
            if (playerEncounter.HasFlag(PlayerEncounter.PlayerBehind))
            {
                agent.keepWalking = false;
                currentCommand = CurrentCommand.MoveToPosition;
                return;
            }
        }
        
        currentCommand = CurrentCommand.MoveToPosition;
     //   Debug.Log(currentCommand);
    }

    private void SetPossibleNodes()
    {
        foreach (var n in children)
        {
            var traveler = n as TravelNode2D;

            if (traveler)
            {
                ownedNodes.Add(CurrentCommand.MoveToPosition, traveler);
                continue;
            }

            var check = n as GridCheckerNode;

            if (check)
            {
                ownedNodes.Add(CurrentCommand.OutOfCommands, check);
            }

            var jump = n as JumpNode;

            if (jump)
            {
                ownedNodes.Add(CurrentCommand.Jump, jump);
            }

            var attack = n as AttackNode;

            if (attack)
            {
                ownedNodes.Add(CurrentCommand.Attack, attack);
            }
        }
    }

    public override void OnExit()
    {
       
    }
    
    public override State OnUpdate()
    {
        CheckOptions();
        if (currentCommand == CurrentCommand.None || !choiceMade) return State.Update;
        
        var child = ownedNodes[currentCommand];

        switch (child.Update())
        {
            case State.Failure:
                return State.Failure;
            
            case State.Update:
                return State.Update;

            case State.Success:
                choiceMade = false;
                break;
        }

        return State.Update;
    }
}
