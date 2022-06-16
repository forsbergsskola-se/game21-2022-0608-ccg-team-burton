using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    private State_ML _currentStateMl;
    private Animator enemy_Anim;

    [SerializeField] private EnemyVars_ML EnemyVars;
    private ArcCollider _arcCollider;

    private void Start()
    {
        EnemyVars._eyes = GetComponentInChildren<Enemy_Eyes>();
        Debug.Log(EnemyVars._eyes.Height);
        
        EnemyVars.ArcCollider = GetComponentInChildren<ArcCollider>();
        enemy_Anim = GetComponent<Animator>();
     
        _currentStateMl = new Idle(gameObject, enemy_Anim, EnemyVars);
    }

    private void Update()
    {
        _currentStateMl = _currentStateMl.Process();
    }
    
    
}
