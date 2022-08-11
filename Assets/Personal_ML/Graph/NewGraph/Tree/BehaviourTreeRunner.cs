using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    [HideInInspector] public bool readyToRun;

    [Header("Enemy Attributes")] 
    [SerializeField, Range(0.2f, 2)]private float attackInterval;
    [SerializeField, Range(1f, 5)]private float baseMoveSpeed;
    [SerializeField, Range(5, 10)]private float pursueMoveSpeed;
    
    public Action<Action<CompoundActions>> CheckForJump;
    
    void Start()
    {
        Setup();
        //CheckForJump?.Invoke();
    }

    private void SomethingSomething(Action<int> action)
    {
       
    }
    
    private void Setup()
    {
       // var grid = GameObject.FindWithTag("LevelGrid").GetComponent<LevelGrid>();
        tree = tree.Clone();
        tree.Bind(new AiAgent()
        {
            enemyTransform = gameObject.transform,
            //grid = grid,
            anim = GetComponent<Animator>(),
            enemyEyes = GetComponentInChildren<TracerEyes>(),
            body = GetComponent<Rigidbody2D>(),
            keepWalking = true,
            attackInterval = attackInterval,
            moveSpeed = baseMoveSpeed,
            CheckForJump = CheckForJump
        });
        readyToRun = true;
    }
    
    void Update()
    {
        if (!readyToRun) return;
        
        tree.Update();
    }
}

