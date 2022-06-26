using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity;
using UnityEditor;
using UnityEngine;

[Flags]
public enum TraceType
{
    Ground,
    Wall,
    Player,
    Platform,
    None
}

public enum Actions
{
    None,
    TurnAround,
    Jump,
    PlatformJump
}

public enum SubType
{
    Wall,
}

[Serializable]
public class HitResults
{
    public RaycastHit2D theHit;
    public TraceType theHitType;
    public int traceCount;
}

public class TracerEyes : MonoBehaviour
{
    [SerializeField] private Transform attackRange;
    public float pursueDistance;
    private int multiMask;
    private float TraceLength = 2f;
    public GameObject StandingOn { get; private set; }
    
    private float traceInterval = 0.4f;
    private float timeSinceTrace;
    
    public bool WallSeen { get; private set;}
    
    public bool WallTurn { get; private set;}
    public bool GroundSeen { get; private set;}
    public bool PlayerSeen { get; private set;}
    
    public bool PlayerNear { get; private set;}
    
    public bool PlatformInJumpDistance { get; private set; }
    
    public Transform PlatformRef { get; private set; }
    
    public Transform WallRef { get; private set; }
    
    public bool PlatformSeen { get; private set; }
    
    public bool PlayerBehind { get; private set; }
    public bool PlayerForgotten { get; private set; }
    
    public Actions actions { get; private set; }

    private Health _playerHealth;
    
    public bool PlayerInAttackRange { get; private set;}
    
    public Transform PlayerTrans;
    
    private void Awake()
    {
        PlayerForgotten = true;
        multiMask = 1 << 6 | 1 << 8;
    }
    

    void Update()
    {
        timeSinceTrace += Time.deltaTime;

        if (timeSinceTrace >= traceInterval)
        {
            timeSinceTrace -= traceInterval;

            DoMultiTrace();
        }
    }

    private void DoMultiTrace()
    {
        var right = transform.right;
       // var pos = transform.position;
       var increment = -0.5f;
       actions = Actions.None;

       var inc = 0.5f;
       List<HitResults> resultList = new List<HitResults>();

       for (int i = 0; i < 4; i++)
       {
           var dir = new Vector2(right.x, increment);
           if (i > 2)
           {
               dir = new Vector2(-right.x, 0);
           }

           var traceHit = DoSingleTrace(dir, transform.position, pursueDistance, out var hit);

           AnalyzeResults(i, traceHit);

           HitResults results = new HitResults()
           {
                traceCount = i,
                theHitType = traceHit,
                theHit = hit
           };
           resultList.Add(results);

           increment += inc;
       }

       if (PlatformSeen && !PlayerSeen)
       {
           PlatformRef = resultList[2].theHit.collider.gameObject.transform;
           
           if (Vector2.Distance(PlatformRef.transform.position, attackRange.position) < 7f)
           {
               PlatformInJumpDistance = true;
           }
           else
           {
               PlatformInJumpDistance = false;
           }
       }

       if (WallSeen)
       {
           if (WallRef != resultList[1].theHit.collider.gameObject.transform)
           {
               WallRef = resultList[1].theHit.collider.gameObject.transform;
           }

           if (resultList[1].theHit.distance < 1 && GroundSeen)
           {
               actions = Actions.TurnAround;
           }
           
       }
       
       if (GroundSeen)
       {
           if (StandingOn != resultList[0].theHit.collider.gameObject)
           {
               StandingOn = resultList[0].theHit.collider.gameObject;
           }
       }
       
       if (PlayerSeen)
       {
           if (Vector2.Distance(resultList[1].theHit.collider.gameObject.transform.position, attackRange.position) < 1f)
           {
               PlayerInAttackRange = true;
           }
           else
           {
               PlayerInAttackRange = false;
           }
           
           if (PlayerTrans == default)
           {
               PlayerTrans = resultList[1].theHit.collider.gameObject.transform; 
               _playerHealth = resultList[1].theHit.collider.gameObject.GetComponent<Health>();
           }
       }
    }

    private void AnalyzeResults(int traceCount, TraceType type)
    {
        switch (type)
        {
            case TraceType.None:
                switch (traceCount)
                {
                    case 0:
                        GroundSeen = false;
                        break;
                    case 1:
                        WallSeen = false;
                        PlayerSeen = false;
                        break;
                    
                    case 2:
                        PlatformSeen = false;
                        break;
                    
                }
                break;
            
            case TraceType.Wall:
            {
                switch (traceCount)
                {
                    case 0:
                        GroundSeen = true;
                        break;
                    
                    case 1:
                        WallSeen = true;
                        break;
                    
                    case 2:
                        PlatformSeen = true;
                        break;
                }
                break;
            }
            
            case TraceType.Player:
            {
                switch (traceCount)
                {
                    case 1:
                        PlayerSeen = true;
                        break;
                    
                    case 3:
                        PlayerBehind = true;
                        break;
                }
                break;
            }
            
        }
    }

    private void CheckPlayer()
    {
        
    }

    private TraceType DoSingleTrace(Vector2 dir, Vector2 pos, float traceDistance, out RaycastHit2D outHit)
    {
        var hit = Physics2D.Raycast(pos, dir, traceDistance, multiMask);
        outHit = hit;

        if (!hit)
        {
            Debug.DrawRay(transform.position, dir *traceDistance, Color.red, traceInterval);
            return TraceType.None;
        }

        var layer = hit.collider.gameObject.layer;
        
        if (layer == 6)
        {
            Debug.DrawRay(transform.position, dir *traceDistance, Color.green, traceInterval);
            
            return TraceType.Ground | TraceType.Wall;
        }
        else if(layer == 8)
        {
            Debug.DrawRay(transform.position, dir *traceDistance, Color.blue, traceInterval);
            
            return TraceType.Player;
        }

        return TraceType.None;
    }

    private void CheckForPlayer(Vector2 dir)
    {
        var trans = transform;
        var hit = Physics2D.Raycast(trans.position, dir, pursueDistance, multiMask);
        
        if (!hit)
        {
            Debug.DrawRay(transform.position, dir *pursueDistance, Color.red, traceInterval);
            PlayerSeen = false;
            return;
        }
        
        if (hit.collider.gameObject.layer == 8)
        {
            Debug.DrawRay(transform.position, dir *pursueDistance, Color.blue, traceInterval);
            PlayerSeen = true;
            
            if (PlayerTrans == default)
            {
                PlayerTrans = hit.collider.transform;
                _playerHealth = hit.collider.gameObject.GetComponent<Health>();
            }

            if (Vector2.Distance(hit.collider.gameObject.transform.position, attackRange.position) < 1f)
            {
                PlayerInAttackRange = true;
            }
            else
            {
                PlayerInAttackRange = false;
            }
                
            Debug.Log("Player spotted");
            return;
        }

        PlayerSeen = false;
    }

    private void CheckForWalls(Vector2 dir)
    {
        var trans = transform;
        var hit = Physics2D.Raycast(trans.position, dir, 0.5f, multiMask);
        
        if (!hit)
        {
            WallSeen = false;
            return;
        }
        
        if (hit.collider.gameObject.layer == 6)
        {
            Debug.DrawRay(transform.position, dir *0.5f, Color.blue, traceInterval);
            WallSeen = true;
        }
    }

    public int GetPlayerHealth()
    => _playerHealth.CurrentHealth;


    private void TraceBox()
    {
        var trans = transform;  
        var result =  Physics2D.BoxCastAll(trans.position + new Vector3((pursueDistance/2) * trans.forward.x, 0), new Vector2(pursueDistance, 2), 0, trans.forward);
        DrawBoxRuntime(new Vector2(pursueDistance, 2), trans.position + new Vector3(pursueDistance * trans.forward.x, 0));
        
        foreach (var r in result)
        {
            if (r.collider.gameObject.layer == 8)
            {
                PlayerSeen = true;
                if (PlayerTrans == default)
                {
                    PlayerTrans = r.collider.transform;
                    _playerHealth = r.collider.gameObject.GetComponent<Health>();
                }

                if (Vector2.Distance(r.collider.gameObject.transform.position, attackRange.position) < 1f)
                {
                    PlayerInAttackRange = true;
                }
                else
                {
                    PlayerInAttackRange = false;
                }
                
                Debug.Log("Player spotted");
                return;
            }
        }

        PlayerSeen = false;
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
}
