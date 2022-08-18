using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class ThornDamage : MonoBehaviour
{
    [SerializeField]
    private int _damage = 1;

    private float _savedTime = 0f;
    private float _delayTime = 1f;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if ((Time.time - _savedTime) > _delayTime)
            {
                Debug.Log($"{name} dealt damage to {other.gameObject.name}");
                other.gameObject.GetComponent<IDamageable>().ModifyHealth(-_damage);
            }
        }
    }
}
