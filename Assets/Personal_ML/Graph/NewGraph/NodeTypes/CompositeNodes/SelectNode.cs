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
    private BaseNode _currentChoice;
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
        agent.keepWalking = true;
        var eyeComp = agent.enemyEyes.compoundActions;
        
        if (!eyeComp.HasFlag(CompoundActions.GroundSeen))
        {
            agent.CheckForJump?.Invoke(x =>
            {
                agent.compoundAction = x;
            });
            
            if (eyeComp.HasFlag(CompoundActions.CanJump))
            {
                currentCommand = CurrentCommand.Jump;
            }

            else
            {
                currentCommand = CurrentCommand.MoveToPosition;    
            }
        }

        else if (eyeComp.HasFlag(CompoundActions.PlayerNoticed))
        {
            PlayerStuff();
        }
        
        else
        {
            currentCommand = CurrentCommand.MoveToPosition;
        }
        choiceMade = true;
    }


    private void PlayerStuff()
    {
        agent.currentDestination = agent.enemyEyes.PlayerPos;
        var playerEncounter = agent.enemyEyes.compoundActions;
        var comp = agent.enemyEyes.compoundActions;
        
        if (playerEncounter.HasFlag(CompoundActions.PlayerInFront))
        {
            if (playerEncounter.HasFlag(CompoundActions.PlayerInAttackRange))
            {
                currentCommand = CurrentCommand.Attack;
                return;
            }

            agent.keepWalking = false;
            currentCommand = CurrentCommand.MoveToPosition;
            return;
        }
        
        if (playerEncounter.HasFlag(CompoundActions.PlayerBehind))
        {
            agent.keepWalking = false;
            currentCommand = CurrentCommand.MoveToPosition;
            return;
        }
        
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
        if(!choiceMade) CheckOptions();

        if (currentCommand == CurrentCommand.None || !choiceMade) return State.Update;
        
        _currentChoice = ownedNodes[currentCommand];

        switch (_currentChoice.Update())
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
