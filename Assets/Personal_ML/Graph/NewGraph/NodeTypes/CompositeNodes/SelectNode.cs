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
    [HideInInspector] public CurrentCommand currentCommand;
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
        
        if (comp.HasFlag(CompoundActions.GroundSeen))
        {
            if (comp.HasFlag(CompoundActions.PlayerNoticed))
            {
                if (comp.HasFlag(CompoundActions.PlayerInAttackRange))
                {
                    currentCommand = CurrentCommand.Attack;
                }
                else if(!comp.HasFlag(CompoundActions.PlayerInAttackRange))
                {
                    agent.currentDestination = agent.enemyEyes.PlayerPos;
                    currentCommand = CurrentCommand.MoveToPosition;
                }
            }

           // else if (comp.HasFlag(CompoundActions.WallSeen))
           // {
           //     if (comp.HasFlag(CompoundActions.HigherGroundSeen))
           //     {
           //       //  currentCommand = CurrentCommand.Jump;
           //       //  agent.enemyEyes.compoundActions &= ~CompoundActions.HigherGroundSeen;
           //     }
           //     
           // }
            
            else if (!comp.HasFlag(CompoundActions.PlayerNoticed))
            {
                if (comp.HasFlag(CompoundActions.ArrivedAtTarget))
                {
                    agent.enemyEyes.compoundActions &= ~CompoundActions.ArrivedAtTarget;
                    GetTarget();
                }
                
                GetTarget();
            }
        }
        
        else if (!comp.HasFlag(CompoundActions.GroundSeen))
        {
            var ground = agent.grid
                .GetCurrentGround(agent.enemyTransform.position +
                                  new Vector3(agent.enemyTransform.right.x * 9,0));
            if (!comp.HasFlag(CompoundActions.PlayerNoticed) && !comp.HasFlag(CompoundActions.LowerGroundSeen))
            {
                if (ground == null)
                {
                    GetTarget();
                }
                else
                {
                    currentCommand = CurrentCommand.Jump;
                }
            }
            
            if (!comp.HasFlag(CompoundActions.PlayerNoticed) && comp.HasFlag(CompoundActions.LowerGroundSeen))
            {
                var dir = agent.enemyTransform.right.x > 0;
                agent.currentDestination = dir ? ground.end : ground.start;
            }
        }

        _choiceMade = true;
    }

    private void IfGroundSeen(CompoundActions comp)
    {
        if (comp.HasFlag(CompoundActions.PlayerNoticed))
        {
            if (comp.HasFlag(CompoundActions.PlayerInAttackRange))
            {
                currentCommand = CurrentCommand.Attack;
            }
            else if(!comp.HasFlag(CompoundActions.PlayerInAttackRange))
            {
                agent.currentDestination = agent.enemyEyes.PlayerPos;
                currentCommand = CurrentCommand.MoveToPosition;
            }
        }

        else if (comp.HasFlag(CompoundActions.WallSeen))
        {
            if (comp.HasFlag(CompoundActions.HigherGroundSeen))
            {
                //  currentCommand = CurrentCommand.Jump;
                //  agent.enemyEyes.compoundActions &= ~CompoundActions.HigherGroundSeen;
            }
                
        }
            
        else if (!comp.HasFlag(CompoundActions.PlayerNoticed))
        {
            GetTarget();
        }
    }
    
    
    private void GetTarget()
    {
        var pos = agent.enemyTransform.position;
        var ground = agent.grid.GetCurrentGround(pos);
        var comp = agent.enemyEyes.compoundActions;

        if (comp.HasFlag(CompoundActions.PlayerBehind) || comp.HasFlag(CompoundActions.PlayerNoticed))
        {
            agent.currentDestination = agent.enemyEyes.PlayerPos;
        }

        else if (!comp.HasFlag(CompoundActions.PlayerNoticed) && !comp.HasFlag(CompoundActions.PlayerBehind))
        {
            var startDist = Vector2.Distance(pos, ground.start);
            var endDist = Vector2.Distance(pos, ground.end);
            
            agent.currentDestination = startDist < endDist ? ground.end : ground.start;
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
        if (!_choiceMade) return State.Update;
        if (currentCommand == CurrentCommand.None) return State.Update;
        _currentChoice = ownedNodes[currentCommand];

        switch (_currentChoice.Update())
        {
            case State.Failure:
                return State.Failure;
            
            case State.Update:
                return State.Update;

            case State.Success:
                _choiceMade = false;
                CheckOptions();
                break;
        }

        return State.Update;
    }
}
