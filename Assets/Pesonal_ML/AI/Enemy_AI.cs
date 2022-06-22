using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    private State_ML _currentStateMl;
    private Animator enemy_Anim;

    [SerializeField] private EnemyVars_ML EnemyVars;
    private ArcCollider _arcCollider;

    public bool turnBack;

    private void Start()
    {
        if (EnemyVars.EnemyType == EnemyType.Ranged)
        {
            EnemyVars.firePoint = GetComponentsInChildren<Transform>()[^1];
        }
      
        EnemyVars.tracerEyes = GetComponentInChildren<TracerEyes>();
        EnemyVars.animator = GetComponent<Animator>();
        EnemyVars.enemyRef = gameObject;
        EnemyVars.ArcCollider = GetComponentInChildren<ArcCollider>();
        enemy_Anim = GetComponent<Animator>();
     
        _currentStateMl = new Idle(EnemyVars);
    }

    private void Update()
    {
        _currentStateMl = _currentStateMl.Process();
    }
    
    
}
