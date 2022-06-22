using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TracerEyes : MonoBehaviour
{
    [SerializeField] private Transform attackRange;
    private int multiMask;
    private float TraceLength = 2f;
    public GameObject StandingOn { get; private set; }
    
    private float traceInterval = 0.4f;
    private float timeSinceTrace;
    
    public bool WallSeen { get; private set;}
    public bool GroundSeen { get; private set;}
    public bool PlayerSeen { get; private set;}
    
    public bool PlayerInAttackRange { get; private set;}
    
    public Transform PlayerTrans;
    
    private void Awake()
    {
        multiMask = 1 << 6 | 1 << 8;
    }
    

    void Update()
    {
        
        timeSinceTrace += Time.deltaTime;

        if (timeSinceTrace >= traceInterval)
        {
            timeSinceTrace -= traceInterval;
            var right = transform.right;
            
            CheckForGround(new Vector2(right.x, -0.8f));
            
            CheckForWalls(right);
            
            TraceCube();
            
        }
    }

    private void DrawBoxRuntime(Vector2 size, Vector2 origin)
    {
        size /= 2;
        var point1 = origin - size;
        var point2 = origin + new Vector2(-size.x,  size.y);
        var point3 = origin + size;
        var point4 = origin + new Vector2(size.x, -size.y);
        
        Debug.DrawLine(point1, point4, Color.red, traceInterval);
        
        Debug.DrawLine(point2, point3 , Color.red, traceInterval);
        
        Debug.DrawLine(point4,point3, Color.red, traceInterval);
        
        Debug.DrawLine(point1,  point2, Color.red, traceInterval);
    }
    
    private void OnDrawGizmosSelected()
    {
      //  var trans = gameObject.transform;
      //  Gizmos.DrawWireCube(trans.position + new Vector3(3.5f ,0) * trans.forward.x, new Vector3(7,3));   
    }

    private RaycastHit2D DoARayTrace(Vector2 dir, bool drawTrace )
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, TraceLength, multiMask);

        return hit;
    }
    
    private void CheckForWalls(Vector2 dir)
    {
        var trans = transform;
        var hit = Physics2D.Raycast(trans.position, dir, 0.9f, multiMask);
        
        if (!hit)
        {
            WallSeen = false;
            return;
        }
        
        if (hit.collider.gameObject.layer == 6)
        {
            Debug.DrawRay(transform.position, dir *0.9f, Color.blue, traceInterval);
            WallSeen = true;
        }
    }

    private void TraceCube()
    {
        var trans = transform;  
        var result =  Physics2D.BoxCastAll(trans.position + new Vector3(3.5f * trans.forward.x, 0), new Vector2(7, 2), 0, trans.forward);
        DrawBoxRuntime(new Vector2(7, 2), trans.position + new Vector3(7 * trans.forward.x, 0));
        
        foreach (var r in result)
        {
            if (r.collider.gameObject.layer == 8)
            {
                PlayerSeen = true;
                if(PlayerTrans == default)
                    PlayerTrans = r.collider.transform;

                if (Vector2.Distance(r.collider.gameObject.transform.position, attackRange.position) < 1f)
                {
                    PlayerInAttackRange = true;
                }
                else
                {
                    PlayerInAttackRange = false;
                }
                
                Debug.Log("Player spotted");
            }
        }
    }
    
    private void CheckForGround(Vector2 dir)
    {
        var trans = transform;
        var hit = Physics2D.Raycast(trans.position, dir, TraceLength, multiMask);
        
        if (!hit)
        {
            GroundSeen = false;
            return;
        }
        
        if (hit.collider.gameObject.layer == 6)
        {
            Debug.DrawRay(trans.position, dir *TraceLength, Color.blue, traceInterval);
            GroundSeen = true;

            if (StandingOn == hit.collider.gameObject) return; 

            StandingOn = hit.collider.gameObject;
        }
    }
    
}
