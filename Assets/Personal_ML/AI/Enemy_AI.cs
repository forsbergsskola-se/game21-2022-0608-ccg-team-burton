using System.Collections;
using System.Collections.Generic;
using Entity;
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
        EnemyVars.enemyHealth = GetComponent<IDamageable>();
        EnemyVars.tracerEyes = GetComponentInChildren<TracerEyes>();
        EnemyVars.animator = GetComponent<Animator>();
        enemy_Anim = GetComponent<Animator>();

        _currentStateMl = new Idle(EnemyVars);

        EnemyVars.enemyRef = gameObject;
        EnemyVars.tracerEyes.pursueDistance = EnemyVars.GetPursueDistance;
    }

    private void Update()
    {
        _currentStateMl = _currentStateMl.Process();
    }
    
    
}
