using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    [HideInInspector] public bool readyToRun;

    [Header("Enemy Attributes")] 
    [SerializeField, Range(0.5f, 4)]private float attackInterval;
    [SerializeField, Range(1f, 5)]private float baseMoveSpeed;
    [SerializeField, Range(5, 10)]private float pursueMoveSpeed;
    [SerializeField, Range(0.5f, 3f)]private float turnDistance;
    [SerializeField, Range(0, 10)]private int damageAmount;

    [Header("Ranged specific")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private AssetPool assetPool;

    void Start()
    {
        Setup();
    }


    private void Setup()
    {
        var grid = GameObject.FindGameObjectsWithTag("LevelGrid")
            .OrderBy(x => Vector2.Distance(x.transform.position, transform.position))
            .ToArray()[0].GetComponent<LevelGrid>();
        
        tree = tree.Clone();
        tree.Bind(new AiAgent()
        {
            enemyTransform = gameObject.transform,
            anim = GetComponent<Animator>(),
            enemyEyes = GetComponentInChildren<TracerEyes>(),
            body = GetComponent<Rigidbody2D>(),
            attackInterval = attackInterval,
            moveSpeed = baseMoveSpeed,
            grid = grid,
            attackPointTrans = GetComponentsInChildren<Transform>()[^2],
            turnDistance = turnDistance,
            damageAmount = damageAmount,
            assetPool = GameObject.FindWithTag("AssetPool").GetComponent<AssetPool>()
        });
        readyToRun = true;
    }
    
    void Update()
    {
        if (!readyToRun) return;
        
        tree.Update();
    }
}

