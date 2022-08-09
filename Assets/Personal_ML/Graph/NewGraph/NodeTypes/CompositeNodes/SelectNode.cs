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

    public override void OnStart()
    {
        SetPossibleNodes();
        if(agent.currentCommand == CurrentCommand.None)
            agent.currentCommand = CurrentCommand.OutOfCommands;

        currentCommand = agent.commandQueue.Peek();
        
        choiceMade = true;
    }
    
    private void OnObjectSeen(TraceType obj)
    {
        if (currentCommand == CurrentCommand.None)
        {
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

        }

    }

    public override void OnExit()
    {
       
    }
    
    public override State OnUpdate()
    {
        if (agent.currentCommand == CurrentCommand.None) return State.Update;
        
        var child = ownedNodes[agent.currentCommand];

        switch (child.Update())
        {
            case State.Failure:
                return State.Failure;
            
            case State.Update:
                return State.Update;

            case State.Success:
                //currentCommand = agent.commandQueue.Peek();
                Debug.Log(agent.currentCommand);
                break;
        }

        return State.Update;
    }
}
