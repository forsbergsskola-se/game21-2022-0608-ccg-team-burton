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
    [SerializeField] private int startPool = 20;
    [SerializeField] private int maxPool = 30;
    private List<GameObject> _effects = new List<GameObject>();
    public static event Action<EffectType, Vector2> OnRequestEffect; 

    private void Start()
    {
        OnRequestEffect += RequestEffect;
        for (var i = 0; i < startPool; i++)
        {
           var effect = Instantiate(effectPrototype);
           effect.AddComponent<ProjectileHandler>();
           effect.SetActive(false);
           _effects.Add(effect);
        }
    }

    private void OnDisable()
    {
        OnRequestEffect -= RequestEffect;
    }

    public static void RequestEffectStatic(EffectType type, Vector2 location)
    {
        OnRequestEffect?.Invoke(type, location);
    }
    
    
    public void RequestEffect(EffectType type, Vector2 location)
    {
        Debug.Log("effect requested");
        GameObject effect = null;
        
        foreach (var t in _effects)
        {
            if (t.activeInHierarchy) continue;
            
            effect = t;
            break;
        }

        if (effect == null && _effects.Count < maxPool)
        {
            effect = Instantiate(effectPrototype);
            _effects.Add(effect);
        }

        var theType = GetEffectType(type);

        if (effect == null) return;
        
        effect.SetActive(true);
        effect.GetComponent<IEffects>().EngageEffect();
        effect.transform.position = location - new Vector2(0, 0.3f);

        //  temp.GetComponent<Animator>().SetTrigger(Animator.StringToHash(input));
    }

    public string GetEffectType(EffectType type)
    {
        var output = "";

        switch (type)
        {
            case EffectType.Blast:
                output = "Blast";
                break;
            case EffectType.Explosion:
                output = "Explosion";
                break;
            case EffectType.Dash:
                output = "Dash";
                break;
            case EffectType.FireBall:
                output = "Fireball";
                break;
        }

        return output;
    }
    
}
