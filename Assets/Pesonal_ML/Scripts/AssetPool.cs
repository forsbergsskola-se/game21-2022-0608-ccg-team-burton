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

public class AssetPool : MonoBehaviour
{
    [SerializeField] private GameObject assetPrototype;
    [SerializeField] private int startPool = 20;
    [SerializeField] private int maxPool = 30;
    private List<GameObject> _objects = new List<GameObject>();
    public static event Action<EffectType, Vector2, Vector2> OnRequestObject; 

    private void Start()
    {
        OnRequestObject += RequestObject;
        for (var i = 0; i < startPool; i++)
        {
           var effect = Instantiate(assetPrototype);
        //   effect.AddComponent<ProjectileHandler>();
           effect.SetActive(false);
           _objects.Add(effect);
        }
    }

    private void OnDisable()
    {
        OnRequestObject -= RequestObject;
    }

    public static void RequestEffectStatic(EffectType type, Vector2 location, Vector2 travelVector)
    {
        OnRequestObject?.Invoke(type, location, travelVector);
    }
    
    
    public void RequestObject(EffectType type, Vector2 location, Vector2 travelVector)
    {
        GameObject effect = null;
        
        foreach (var t in _objects)
        {
            if (t.activeInHierarchy) continue;
            
            effect = t;
            break;
        }

        if (effect == null && _objects.Count < maxPool)
        {
            effect = Instantiate(assetPrototype);
            _objects.Add(effect);
        }

        var theType = GetEffectType(type);

        if (effect == null) return;
        
        effect.SetActive(true);
    //    effect.GetComponent<IEffects>().EngageEffect();
        effect.transform.position = location;
        effect.GetComponent<Bullet_ML>().travelVector = travelVector;

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
