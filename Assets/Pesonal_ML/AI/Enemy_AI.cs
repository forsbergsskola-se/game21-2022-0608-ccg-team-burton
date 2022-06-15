using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    private Vector3 testVector;
    private bool rotate = true;
    private bool move = false;
    private State_ML _currentStateMl;
    private Animator enemy_Anim;
    private bool currentFace = false;
   
    [SerializeField] private float moveSpeed = 5.0f;
    private Enemy_Eyes _detector;

    private void Start()
    {
        enemy_Anim = GetComponent<Animator>();
        _detector = GetComponentInChildren<Enemy_Eyes>();
        _currentStateMl = new Patrol(gameObject,  _detector);
        currentFace = gameObject.GetComponent<SpriteRenderer>().flipX;
    }

    private void Update()
    {
        _currentStateMl = _currentStateMl.Process();
    }

    public void GroundGone()
    {
        currentFace = !currentFace;
        gameObject.GetComponent<SpriteRenderer>().flipX = currentFace;
    }
    
    public void PlayerSpotted()
    {
        _currentStateMl = new Pursue(gameObject, _detector);
        enemy_Anim.SetBool("ExitIdleState", true);
    }
    
    
}
