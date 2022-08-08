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
        choiceMade = false;
        SetPossibleNodes();
        currentCommand = agent.commandQueue.Peek();
        choiceMade = true;
    }
    
    private void OutOfCommands()
    {
        var closestPos = GameObject.FindGameObjectsWithTag("EnemyCommander")
            .OrderBy(x => Vector3.Distance(agent.enemyTransform.position, x.transform.position)).ToArray()[0].transform.position;
        
        agent.TargetQueue.Enqueue(closestPos);
        agent.commandQueue.Enqueue(CurrentCommand.MoveToPosition);
        agent.commandQueue.Enqueue(CurrentCommand.GetInstructions);
    }
    
    private void OnObjectSeen(TraceType obj)
    {
        if (currentCommand == CurrentCommand.None)
        {
        }
    }

    public void SetPossibleNodes()
    {
        foreach (var n in children)
        {
            var traveler = n as TravelNode;

            if (traveler)
            {
                ownedNodes.Add(CurrentCommand.MoveToPosition, traveler);
                continue;
            }
            
        }

    }

    public override void OnExit()
    {
       
    }
    
    public override State OnUpdate()
    {
        if(agent.commandQueue.Count < 1)
            OutOfCommands();
        
        if (!choiceMade) return State.Update;
        
        var child = ownedNodes[currentCommand];

        switch (child.Update())
        {
            case State.Failure:
                return State.Failure;
            
            case State.Update:
                return State.Update;

            case State.Success:
                currentCommand = agent.commandQueue.Peek();
                break;
        }

        return State.Update;
    }
}
