using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    private State_ML _currentStateMl;
    private Animator enemy_Anim;
    private bool currentFace = false;

    [SerializeField] private EnemyVars_ML EnemyVars;
    
    private Enemy_Eyes _detector;

    private void Start()
    {
        EnemyVars._eyes = GetComponentInChildren<Enemy_Eyes>();
        
        enemy_Anim = GetComponent<Animator>();
        _detector = GetComponentInChildren<Enemy_Eyes>();
        _currentStateMl = new Idle(gameObject, _detector, enemy_Anim, EnemyVars);
        currentFace = gameObject.GetComponent<SpriteRenderer>().flipX;
    }

    private void Update()
    {
        _currentStateMl = _currentStateMl.Process();
    }

    public void GroundGone()
    {
        currentFace = !currentFace;
        gameObject.transform.Rotate(Vector3.up, 180);
     //   gameObject.GetComponent<SpriteRenderer>().flipX = currentFace;
    }
    
    public void PlayerSpotted()
    {
        _currentStateMl = new Pursue(gameObject, _detector, enemy_Anim, EnemyVars);
    }
    
    
}
