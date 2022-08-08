using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [Header("Input values")]
    [SerializeField] private Transform enemyTrans;
    [SerializeField] private Vector2 cubeSize;
    [SerializeField] private Vector2 numberCubes;
    [Range(0, 1),SerializeField] private float delayUpdate;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (var i = 0; i < numberCubes.y; i++)
        {
            for (var j = 0; j < numberCubes.x; j++)
            {
                var next = transform.position + new Vector3(j * cubeSize.x ,i * cubeSize.y);
                Gizmos.DrawWireCube(next, new Vector3(cubeSize.x ,cubeSize.y));
                
            }
        }
    }
    #endif
}
