using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    private State_ML _currentStateMl;
    private Animator enemy_Anim;

    [SerializeField] private EnemyVars_ML EnemyVars;
    
    private Enemy_Eyes _detector;
    private ArcCollider _arcCollider;

    private void Start()
    {
        EnemyVars._eyes = GetComponentInChildren<Enemy_Eyes>();
        EnemyVars.ArcCollider = GetComponentInChildren<ArcCollider>();
        enemy_Anim = GetComponent<Animator>();
        _detector = GetComponentInChildren<Enemy_Eyes>();
        _detector = GetComponentInChildren<Enemy_Eyes>();
        _currentStateMl = new Idle(gameObject, _detector, enemy_Anim, EnemyVars);
    }

    private void Update()
    {
        _currentStateMl = _currentStateMl.Process();
    }

    public void GroundGone()
    {
        gameObject.transform.Rotate(Vector3.up, 180);
     //   gameObject.GetComponent<SpriteRenderer>().flipX = currentFace;
    }
    
    public void PlayerSpotted()
    {
        _currentStateMl = new Pursue(gameObject, _detector, enemy_Anim, EnemyVars);
    }
    
    
}
