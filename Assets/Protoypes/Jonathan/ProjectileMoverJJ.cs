using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoverJJ : MonoBehaviour
{
    
     Vector3 _playerPosition;
     private Vector3 _fireDir;
     [SerializeField]
     private Rigidbody2D _rb2d;
     [SerializeField]
     private float bulletSpeed = 300f;
 
     void Start()
     {
         _playerPosition = GameObject.FindWithTag("Player").transform.position;
         _fireDir = (_playerPosition -transform.position).normalized;
         _rb2d.velocity = _fireDir * (bulletSpeed * Time.deltaTime);

     }

}
