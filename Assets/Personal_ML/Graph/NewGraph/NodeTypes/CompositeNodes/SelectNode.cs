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
            else if (!comp.HasFlag(CompoundActions.PlayerNoticed))
            {
                GetTarget(Vector2.Distance(agent.currentDestination, agent.attackPointTrans.position) <= agent.turnDistance);
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
                    GetTarget(true);
                }
                else
                {
                    currentCommand = CurrentCommand.Jump;
                }

            }
            
            if (!comp.HasFlag(CompoundActions.PlayerNoticed) && comp.HasFlag(CompoundActions.LowerGroundSeen))
            {
                var dir = agent.enemyTransform.right.x > 0;
                agent.currentDestination = ground.end;
            }
            Debug.Log(comp);
        }

        _choiceMade = true;
    }

    private void CheckForJumps(bool startOrEnd)
    {
        var currPos = agent.enemyTransform.position;
        var currGround = agent.grid.GetCurrentGround(currPos);
        
        var nexGround = agent.grid
            .GetCurrentGround(currPos +
                              new Vector3(agent.enemyTransform.right.x * 9,0));

        var yDist = nexGround.start.y - currGround.end.y;
        var xDist = nexGround.start.x - currGround.end.x;

        if (xDist < 1)
        {
            currentCommand = CurrentCommand.MoveToPosition;
            
            agent.currentDestination = nexGround.end;
        }
    }
    
    private void GetClosestTarget()
    {
        var pos = agent.enemyTransform.position;
        var ground = agent.grid.GetCurrentGround(pos);
        var start = Vector2.Distance(pos, ground.start);
        var end = Vector2.Distance(pos, ground.start);
        
        var dir = agent.enemyTransform.right.x > 0;
        
        if (dir)
        {
            agent.currentDestination =  ground.end;
        }
        else
        {
            agent.currentDestination = ground.start;
        }

        agent.currentDestination = ground.start;
        
        currentCommand = CurrentCommand.MoveToPosition;
    }
    
    private void GetTarget(bool atEnd)
    {
        var ground = agent.grid.GetCurrentGround(agent.enemyTransform.position);
        var dir = agent.enemyTransform.right.x > 0;
        var comp = agent.enemyEyes.compoundActions;

        if (comp.HasFlag(CompoundActions.PlayerBehind))
        {
            agent.currentDestination = agent.enemyEyes.PlayerPos;
        }
        else if (comp.HasFlag(CompoundActions.PlayerNoticed))
        {
            agent.currentDestination = agent.enemyEyes.PlayerPos;
        }
        
        else if (!comp.HasFlag(CompoundActions.PlayerNoticed))
        {
            if (dir)
            {
                agent.currentDestination = atEnd ? ground.start : ground.end;
            }
            else
            {
                agent.currentDestination = atEnd ? ground.end : ground.start;
            }
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
