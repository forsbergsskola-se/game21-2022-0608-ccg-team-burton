using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class ArcCollider : MonoBehaviour
{
    private EdgeCollider2D collider;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float velocity = 9.8f;
    [SerializeField] private float angle = 60f;
    public bool TileSpotted { get; private set; }
    public bool SameTileSpotted { get; private set; }
    public bool announceTileSpotted;
    private float currentDeg;

    public GameObject NextTile { get; private set; }
    
    public float TileHeightDifference { get; private set; }

    public float GetAngle => angle;

    public bool TestForJump;

    void Start()
    {
        currentDeg = angle;
        collider = GetComponent<EdgeCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Grounded"))
        {
            TileSpotted = true;

            if (col.gameObject != NextTile)
            {
                if(NextTile != null)
                    TileHeightDifference = col.gameObject.transform.position.y - NextTile.transform.position.y;

                else
                {
                    TileHeightDifference = col.gameObject.transform.position.y;
                }
                
                NextTile = col.gameObject;
                SameTileSpotted = false;
            }
            else
            {
                SameTileSpotted = true;
            }
            
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Grounded"))
        {
            TileSpotted = false;
        }
    }

    public Vector3 GetNextTilePosition()
        => NextTile.transform.position;



    public float GetLengthDifference()
    {
       return collider.points[^2].x ;
    }
    
    public Vector2 GetImpulse()
    {
        var length = collider.points.Length;
        if (length < 1) 
            return new Vector2(0, 0);
        
        var some = collider.points[length / 2];
 
        return  some;
    }

    public Vector2 GetPoint(int pointIndex)
    {
        return collider.points.Length > pointIndex 
            ? collider.points[pointIndex] : Vector2.zero;
    }
    
    public List<Vector2> GetArc()
    {
        return collider.points.ToList();
    }

    public int GetNumberArcPoints()
    {
        return collider.points.Length;
    }

    public void ResetCollider()
    {
        angle = 60;
        collider.Reset();
        collider.isTrigger = true;
        collider.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
    
    public void CalculatePoints()
    {
        var something = 2 * velocity * Mathf.Sin(Mathf.Deg2Rad * angle) / gravity;
        var points = FRange(0, something, 0.1f);
        var vecs = new List<Vector2>();
        
        foreach (var p in points)
        {
            var posX = velocity * Mathf.Cos(Mathf.Deg2Rad * angle) * p;
            var posY = velocity * Mathf.Sin(Mathf.Deg2Rad * angle) * p - (0.5f * gravity * p * p);
            vecs.Add(new Vector2(posX, posY));
        }
        vecs.Add(new Vector2(vecs[^1].x, vecs[^1].y - 5));
        
        
        collider.points = vecs.ToArray();

        angle+= Time.deltaTime * 30f;
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
    
}
