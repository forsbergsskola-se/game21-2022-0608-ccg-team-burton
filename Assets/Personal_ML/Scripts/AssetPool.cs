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
        
    }

    public static void RequestEffectStatic(EffectType type, Vector2 location, Vector2 travelVector)
    {
        OnRequestObject?.Invoke(type, location, travelVector);
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

    public void RequestObject(Vector2 location, Vector2 travelVector, float moveSpeed, int damageAmount)
    {
        GameObject asset = null;
        
        foreach (var t in _objects)
        {
            if (t.activeInHierarchy) continue;
            
            asset = t;
            break;
        }
        
        if (asset == default && _objects.Count < maxPool)
        {
            asset = Instantiate(assetPrototype);
            _objects.Add(asset);
        }
        
        if (asset == default) return;
        
        asset.SetActive(true);
        asset.transform.position = location;
        var comp = asset.GetComponent<Bullet_ML>();
        comp.travelVector = travelVector;
        comp.moveSpeed = moveSpeed;
        comp.damageAmount = damageAmount;
    }
}
