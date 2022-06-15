using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    private Vector3 testVector;
    private bool rotate = true;
    private bool move = false;
    private State currentState;
    [SerializeField] private Transform playerTrans;
    [SerializeField] private float moveSpeed = 5.0f;
    private PlayerDetector _detector;
    
    void Start()
    {
        _detector = GetComponentInChildren<PlayerDetector>();
        currentState = new Patrol(playerTrans, gameObject, GameObject.FindWithTag("Patrol"), _detector);
    }

    void Update()
    {
        currentState = currentState.Process();
    }
}
