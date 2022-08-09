using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    [HideInInspector] public bool readyToRun;
    
    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        var grid = GameObject.FindWithTag("LevelGrid").GetComponent<LevelGrid>();
        tree = tree.Clone();
        tree.Bind(new AiAgent()
        {
            enemyTransform = gameObject.transform,
            grid = grid,
            anim = GetComponent<Animator>(),
            enemyEyes = GetComponentInChildren<TracerEyes>(),
            body = GetComponent<Rigidbody2D>(),
            keepWalking = true
        });
        readyToRun = true;
    }
    
    void Update()
    {
        if (!readyToRun) return;
        
        tree.Update();
    }
}

