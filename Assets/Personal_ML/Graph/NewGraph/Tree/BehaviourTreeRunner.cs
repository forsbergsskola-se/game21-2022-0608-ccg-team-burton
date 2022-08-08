using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    public Transform commanderTrans;
    public bool readyToRun;
    
    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        Queue<CurrentCommand> commands = new Queue<CurrentCommand>();
        commands.Enqueue(CurrentCommand.MoveToPosition);
        commands.Enqueue(CurrentCommand.GetInstructions);
        Queue<Vector3> goals = new Queue<Vector3>();
        goals.Enqueue(commanderTrans.position);
        
        tree = tree.Clone();
        tree.Bind(new AiAgent()
        {
            enemyTransform = gameObject.transform,
            currentDestination = GameObject.Find("EnemyCommander").transform.position,
            commandQueue = commands,
            TargetQueue = goals
        });

        readyToRun = true;
    }
    
    private void OnDisable()
    {
    }

    private void OnObjectSeen(TraceType obj)
    {
        
    }

    public Transform GetTopParent(Transform pTrans)
    {
        if (pTrans.parent)
        {
            pTrans = pTrans.parent;
            GetTopParent(pTrans);
        }
        
        return pTrans;
    }
    
    void Update()
    {
        if (!readyToRun) return;
        
        tree.Update();
    }

    public void MoveToDestination(Vector3 destination)
    {
       
    }

    public void SetNextCommand(CurrentCommand command)
    {
       
    }

    public void GetNextDestination(Action<Instruction> callBack)
    {
        
    }
}

