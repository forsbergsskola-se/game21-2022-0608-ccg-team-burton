using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArcCollider : MonoBehaviour
{
    private EdgeCollider2D collider;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float velocity = 9.8f;
    [SerializeField] private float angle = 30f;

    void Start()
    {
        collider = GetComponent<EdgeCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("ATile"))
        {
            
        }
    }

    private void CalculatePoints()
    {
        var something = 2 * velocity * Mathf.Sin(Mathf.Deg2Rad * angle) / gravity;
        var points = FRange(0, something, 0.05f);
        var vecs = new List<Vector2>();
        
        foreach (var p in points)
        {
            var posX = velocity * Mathf.Cos(Mathf.Deg2Rad * angle) * p;
            var posY = velocity * Mathf.Sin(Mathf.Deg2Rad * angle) * p - (0.5f * gravity * p * p);
            vecs.Add(new Vector2(posX, posY));
        }
      
        collider.points = vecs.ToArray();
    }

    private List<float> FRange(float start, float final, float increment)
    {
        var numbers = new List<float>();
        while (start < final)
        {
            numbers.Add(start);
            start += increment;
        }

        return numbers;
    }

    private void Update()
    {
        CalculatePoints();
        angle += 1f * Time.deltaTime;

    }
}
