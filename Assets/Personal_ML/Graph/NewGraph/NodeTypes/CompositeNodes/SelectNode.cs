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
        _plusMinus = agent.enemyTransform.right.x > 0;
       // _current = agent.grid.GetSquareFromPoint(agent.enemyTransform.position);
        agent.keepWalking = true;
        if (!agent.enemyEyes.GroundSeen)
        {
            _choiceMade = true;
            currentCommand = CurrentCommand.Jump;
            return;
        }

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
                agent.currentDestination = agent.enemyEyes.PlayerPos;
                currentCommand = CurrentCommand.MoveToPosition;
                return;
            }
            
            if (playerEncounter.HasFlag(PlayerEncounter.PlayerBehind))
            {
                agent.keepWalking = false;
                agent.currentDestination = agent.enemyEyes.PlayerPos;
                currentCommand = CurrentCommand.MoveToPosition;
                return;
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
        if (currentCommand == CurrentCommand.None || !choiceMade) return State.Update;
        
        var child = ownedNodes[currentCommand];

        switch (child.Update())
        {
            case State.Failure:
                return State.Failure;
            
            case State.Update:
                return State.Update;

            case State.Success:
                CheckOptions();
                break;
        }

        return State.Update;
    }
}
