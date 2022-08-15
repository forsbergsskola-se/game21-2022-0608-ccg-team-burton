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
        var comp = agent.enemyEyes.compoundActions;

        if (comp.HasFlag(CompoundActions.EnemyDead))
        {
            agent.body.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        
        if (!comp.HasFlag(CompoundActions.GroundSeen) && !comp.HasFlag(CompoundActions.PlayerNoticed))
        {
            var ground = agent.grid
                .GetCurrentGround(agent.enemyTransform.position +
                                  new Vector3(agent.enemyTransform.right.x * 9,0));
            
            if (ground == null)
            {
                GetTarget(true);
            }
            else
            {
                currentCommand = CurrentCommand.Jump;
            }
        }

        else if (comp.HasFlag(CompoundActions.PlayerNoticed) && comp.HasFlag(CompoundActions.GroundSeen))
        {
            agent.currentDestination = agent.enemyEyes.PlayerPos;
            if (comp.HasFlag(CompoundActions.PlayerInFront))
            {
                if (comp.HasFlag(CompoundActions.PlayerInAttackRange))
                {
                    currentCommand = CurrentCommand.Attack;
                }
                else
                {
                    currentCommand = CurrentCommand.MoveToPosition;
                }
            }
            
            if (comp.HasFlag(CompoundActions.PlayerBehind))
            {
                currentCommand = CurrentCommand.MoveToPosition;
            }
           
        }
        else
        {
            GetTarget(Vector2.Distance(agent.currentDestination, agent.attackPointTrans.position) <= agent.turnDistance);
        }

        _choiceMade = true;
    }
    
    private void GetTarget(bool atEnd)
    {
        var ground = agent.grid.GetCurrentGround(agent.enemyTransform.position);
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
        if (!choiceMade) return State.Update;
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
                break;
        }

        return State.Update;
    }
}
