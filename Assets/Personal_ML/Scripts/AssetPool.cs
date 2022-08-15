using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPool : MonoBehaviour
{
    [SerializeField] private GameObject assetPrototype;
    [SerializeField] private int startPool = 20;
    [SerializeField] private int maxPool = 30;
    private List<GameObject> _objects = new List<GameObject>();
    
    private void Start()
    {
        for (var i = 0; i < startPool; i++)
        {
           var effect = Instantiate(assetPrototype);
           effect.SetActive(false);
           _objects.Add(effect);
        }
    }
    
    public void RequestBullet(Vector2 location, Vector2 travelVector, float moveSpeed, int damageAmount, float maxLifespan)
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
        comp.maxLifespan = maxLifespan;
        comp.timeAlive = 0;
    }
}
