using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour
{
    [SerializeField]
    private float _knockbackForce;

    public float KnockbackForce
    {
        get => _knockbackForce;
        set => _knockbackForce = value;
    }

    [SerializeField]
    private Rigidbody2D _rb2d;
    
    public void DoKnockback(Vector3 hitposition)
    {
        Debug.Log("Knockback");
        var knockbackDir = (transform.position - hitposition).normalized;
        
        _rb2d.AddForce(knockbackDir * KnockbackForce, ForceMode2D.Impulse);

    }
}
