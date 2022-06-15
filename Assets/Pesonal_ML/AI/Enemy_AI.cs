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
   
    [SerializeField] private float moveSpeed = 5.0f;
    private Enemy_Eyes _detector;

    private void Start()
    {
        _detector = GetComponentInChildren<Enemy_Eyes>();
        _currentStateMl = new Patrol(gameObject,  _detector);
    }

    private void Update()
    {
        _currentStateMl = _currentStateMl.Process();
    }

    public void GroundGone()
    {
        gameObject.transform.Rotate(new Vector3(0,0,1), 180);
    }
    
    public void PlayerSpotted()
    {
        _currentStateMl = new Pursue(gameObject, _detector);
    }
    
    
}
