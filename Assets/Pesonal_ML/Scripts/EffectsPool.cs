using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    FireBall,
    Explosion,
    Blast,
    Dash
}

public class EffectsPool : MonoBehaviour
{
    [SerializeField] private GameObject effectPrototype;
    private List<GameObject> _effects = new List<GameObject>();

    private void Start()
    {
        RequestEffect(EffectType.Explosion, Vector2.zero);
    }

    public void RequestEffect(EffectType type, Vector2 location)
    {
        var input = "";
        
        var temp = Instantiate(effectPrototype, new Vector3(location.x, location.y), Quaternion.identity);
        
        switch (type)
        {
            case EffectType.Blast:
                input = "Blast";
                break;
            case EffectType.Explosion:
                input = "Explosion";
                break;
            case EffectType.Dash:
                input = "Dash";
                break;
            case EffectType.FireBall:
                input = "Fireball";
                break;
        }
        
        temp.GetComponent<Animator>().SetTrigger(Animator.StringToHash(input));
    }
    
}
