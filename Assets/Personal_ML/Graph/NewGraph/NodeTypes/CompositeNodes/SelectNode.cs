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
      
    }
    
    private void CheckOptions()
    {
        Debug.Log("checking options");
        if (!agent.enemyEyes.compoundActions.HasFlag(CompoundActions.GroundSeen))
        {
            var ground = agent.grid
                .GetCurrentGround(agent.enemyTransform.position +
                                  new Vector3(agent.enemyTransform.right.x * 9,0), 2);
            
            if (ground == null)
            {
               GetTarget(true);
            }
            else
            {
                currentCommand = CurrentCommand.Jump;
            }
        }

        else
        {
            GetTarget(false);
        }

        _choiceMade = true;
        Debug.Log(choiceMade);
    }
    
    private void GetTarget(bool atEnd)
    {
        agent.keepWalking = false;
        var ground = agent.grid.GetCurrentGround(agent.enemyTransform.position, 4);

        var dir = agent.enemyTransform.right.x > 0;

        if (dir)
        {
            agent.currentDestination = atEnd ? ground.start : ground.end;
        }
        else
        {
            agent.currentDestination = atEnd ? ground.end : ground.start;
        }
        
        currentCommand = CurrentCommand.MoveToPosition;
    }
    
    private void Save()
    {
        var eyeComp = agent.enemyEyes.compoundActions;
        
        if (eyeComp.HasFlag(CompoundActions.EnemyDead))
        {
            choiceMade = false;
            return;
        }
        
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
        _currentChoice = ownedNodes[currentCommand];

        switch (_currentChoice.Update())
        {
            case State.Failure:
                return State.Failure;
            
            case State.Update:
                return State.Update;

            case State.Success:
                choiceMade = false;
                CheckOptions();
                Debug.Log("exit a node");
                break;
        }

        return State.Update;
    }
}
