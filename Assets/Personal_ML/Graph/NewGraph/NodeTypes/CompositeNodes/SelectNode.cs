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
    }
    
    private void OnObjectSeen(TraceType obj)
    {
        if (currentCommand == CurrentCommand.None)
        {
        }
    }
    
    private void CheckOptions()
    {
        _plusMinus = agent.enemyTransform.right.x > 0;
        _current = agent.grid.GetSquareFromPoint(agent.enemyTransform.position);

        if (!agent.enemyEyes.GroundSeen)
        {
            agent.commandQueue.Enqueue(CurrentCommand.Jump);
            _choiceMade = true;
            currentCommand = agent.commandQueue.Dequeue();
            Debug.Log("jumping time");
            return;
        }
        
        if (_plusMinus)
        {
            if (_current.options.HasFlag(TileOptions.OpenPlus))
            {
                agent.commandQueue.Enqueue(CurrentCommand.MoveToPosition);
                agent.currentDestination = new Vector3(_current.max.x, agent.enemyTransform.position.y);
                currentCommand = agent.commandQueue.Dequeue();
                _choiceMade = true;
                return;
            }
        }
        else
        {
            agent.commandQueue.Enqueue(CurrentCommand.None);
        }
        
       // agent.commandQueue.Enqueue(CurrentCommand.None);
      //  currentCommand = agent.commandQueue.Dequeue();
      //  Debug.Log(currentCommand);
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
