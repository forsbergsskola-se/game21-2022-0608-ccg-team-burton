using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using Entity;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[Flags]
public enum TraceType
{
    Ground,
    Wall,
    Player,
    Platform,
    Enemy,
    None
}

public enum TraceStates
{
    GroundSeen,
    WallSeen,
    WallNear,
    PlatformSeen,
    PlatformNear,
    
}

public enum Actions
{
    None,
    TurnAround,
    TestForJump,
    Jump,
    PlatformJump,
    Pursue
}

[Serializable]
public class HitResultValues
{
    public TraceType type;
    public Vector2 position;
    public bool objectWithinRange;
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

    public bool WallInRange;

    private List<HitResultValues> hitValuesList = new();
    
    public bool WallTurn { get; private set;}
    public bool GroundSeen { get; private set;}
    public bool PlayerSeen { get; private set;}
    
    public bool PlayerNear { get; private set;}
    
    public bool PlatformInJumpDistance { get; private set; }
    
    public Transform PlatformRef { get; private set; }
    
    public Transform WallRef { get; private set; }
    
    public bool PlatformSeen { get; private set; }
    
    public bool PlayerBehind { get; private set; }

    private bool PlayerHit;
    public bool PlayerForgotten { get; private set; }

    private bool PlatformInRange;

    private bool JumpableWallSeen;
    
    private bool WallOnTopSeen;

    private bool PlayerKnown;
    
    public Actions actions { get; private set; }

    private Health _playerHealth;
    
    public Vector2 EstimatedJumpForce { get; private set; }
    
    public bool PlayerInAttackRange { get; private set;}
    
    public Transform PlayerTrans;

    private List<HitResults> hitResultList = new();

    private List<float> traceDistances;

    private void Awake()
    {
        traceDistances = new List<float>();


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
           // CheckForGround(new Vector2(transform.right.x, -0.5f));
        //    DoSquareTrace();
        }
    }

    private void DoSquareTrace()
    {
       // DrawBoxRuntime(new Vector2(8, 8), transform.position);
        TraceBox(transform);
    }

    private bool IsObjectBehind(Vector2 objectPos)
    {
        return Vector2.Dot(transform.TransformDirection(Vector3.right),
            (Vector3)objectPos - transform.position) < 0;
    }
    
    private void DoMultiTrace()
    {
        var right = transform.right;
        var pos = transform.position;
       var increment = -0.5f;
       actions = Actions.None;

       hitResultList.Clear();
       var inc = 0.4f;

       for (int i = 0; i < 5; i++)
       {
           var dir = new Vector2(right.x, increment);
           var traceDistance = pursueDistance;
           
           if (i == 0)
           {
               traceDistance = 2.5f;
               dir = new Vector2(right.x, -0.7f);
           }
           else if (i == 1)
           {
               dir = new Vector2(right.x, 0);     
           }
           else
           {
               traceDistance = pursueDistance;
           }
           
           if (i > 3)
           {
               dir = new Vector2(-right.x, 0);
           }

           var traceHit = DoSingleTrace(dir, pos, traceDistance, out var hit);
           
           AnalyzeResults(i, traceHit);

           HitResults results = new HitResults()
           {
               theHitType = traceHit,
               theHit = hit
           };
           hitResultList.Add(results);

           increment += inc;
       }
       
       var traceHit2= DoSingleTrace(right, pos + new Vector3(0, 4), 8, out var hit2);
       AnalyzeResults(5, traceHit2);
       
       HitResults results2 = new HitResults()
       {
           theHitType = traceHit2,
           theHit = hit2
       };
       hitResultList.Add(results2);

       SetResults();
       
    }

    private float CheckGroundDistance()
    {
        var right = transform.right;
        var pos = transform.position + new Vector3(0, 4);
       // var traceHit= DoSingleTrace(right, pos, 8, out var hit);
        var traceHit2= DoSingleTrace(new Vector2(0, -1), pos, 8, out var hit2);
        var traceHit3= DoSingleTrace(new Vector2(0, -1), pos + new Vector3(right.x * 5, 0), 8, out var hit3);

        return hit2.distance - hit3.distance;
    }
  
    private void SetResults()
    {
        if (PlayerSeen)
        {
            SetRangeValues(hitResultList[1].theHit.collider.gameObject.transform.position, 0.4f, TraceType.Player);
            
            if (PlayerTrans == default)
            {
                PlayerTrans = hitResultList[1].theHit.collider.gameObject.transform; 
                _playerHealth = hitResultList[1].theHit.collider.gameObject.GetComponent<Health>();
            }
        }
       
        if (!PlayerSeen)
        {
            if (PlayerTrans != default)
            {
                if (PlayerBehind)
                {
                    actions = Actions.TurnAround;
                }
            }
        }
       
        if (GroundSeen)
        {
            if (StandingOn != hitResultList[0].theHit.collider.gameObject)
            {
                StandingOn = hitResultList[0].theHit.collider.gameObject;
            }
        }
        
        
        if (PlatformSeen)
        {
            if (!GroundSeen)
            {
                if (hitResultList[2].theHit.distance < 7f)
                {
                    PlatformInJumpDistance = true;
                    actions = Actions.PlatformJump;
                }
                else
                {
                    PlatformInJumpDistance = false;
                }
            }

            if (!PlayerSeen && !WallSeen && !PlayerBehind)
            {
                if (hitResultList[2].theHit.distance < 7f)
                {
                    PlatformInJumpDistance = true;
                    actions = Actions.PlatformJump;
                }
                else
                {
                    PlatformInJumpDistance = false;
                }
            }
        }
        
        if (WallSeen)
        {
            if (hitResultList[1].theHit.distance < 1 && GroundSeen)
            {
                actions = Actions.TurnAround;
            }
            
            if (!WallOnTopSeen)
            {
                if (hitResultList[1].theHit.distance < 2f)
                {
                    var dist = CheckGroundDistance();
                    EstimatedJumpForce = new Vector2(3, dist);

                    PlatformInJumpDistance = true;
                    actions = Actions.PlatformJump;
                }
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
                    
                    case 4:
                        PlayerBehind = false;
                        break;
                    
                    case 5:
                        WallOnTopSeen = false;
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
                    
                    case 5:
                        WallOnTopSeen = true;
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
                    
                    case 4:
                        PlayerBehind = true;
                        break;
                }
                break;
            }
            
        }
    }


    private float CompareVectorComponents(Vector2 first, Vector2 second, bool xOrY)
    {
        if (xOrY) return first.x - second.x;

        return first.y - second.y;
    }
    
    private TraceType DoSingleTrace(Vector2 dir, Vector2 pos, float traceDistance, out RaycastHit2D outHit)
    {
        var hit = Physics2D.Raycast(pos, dir, traceDistance, multiMask);
        outHit = hit;
        
        if (!hit)
        {
            Debug.DrawRay(pos, dir *traceDistance, Color.red, traceInterval);
            return TraceType.None;
        }
        
        var layer = hit.collider.gameObject.layer;
        
        if (layer == 6)
        {
            Debug.DrawRay(pos, dir *traceDistance, Color.green, traceInterval);
            
            return TraceType.Ground | TraceType.Wall;
        }
        else if(layer == 8)
        {
            Debug.DrawRay(pos, dir *traceDistance, Color.blue, traceInterval);
            
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


    private void TraceBox(Transform trans)
    {
        var sizeY = 7f;
        var sizeX = pursueDistance;
        var boxPlacement = trans.position + new Vector3(0, sizeY / 2 - 1);
        var result = Physics2D.BoxCastAll(boxPlacement , new Vector2(pursueDistance * 2, sizeY), 0, trans.forward, 8, multiMask);
        DrawBoxRuntime(new Vector2(pursueDistance, sizeY), boxPlacement);
        var playerSeen = false;
        var playerIsHit = false;
        
        hitValuesList.Clear();
        
        foreach (var r in result)
        {
            hitValuesList.Add(new HitResultValues());
            var hitObject = r.collider.gameObject;
            hitValuesList[^1].position = hitObject.transform.position;
     
            TilemapCollider2D coll;
            
            
            if (hitObject.layer == 8)
            {
                hitValuesList[^1].type = TraceType.Player;

                playerIsHit = true;
                if (PlayerTrans == default)
                {
                    PlayerTrans = r.collider.transform;
                    _playerHealth = r.collider.gameObject.GetComponent<Health>();
                }

                SetRangeValues(PlayerTrans.position, 1, TraceType.Player);
                
                Debug.Log("Player spotted");
            }
            
            else if (hitObject.layer == 6)
            {

                if (hitObject.transform.localScale.y > 2)
                {
                    hitValuesList[^1].type = TraceType.Wall;
                    Debug.Log("Wall seen");
                    SetRangeValues(hitObject.transform.position, 0.8f, TraceType.Wall);
                }
                    
                else if (hitObject.transform.localScale.x > 7)
                {
                    hitValuesList[^1].type = TraceType.Ground;
                    Debug.Log("Floor seen");
                }
                
                else
                {
                    PlatformSeen = true;
                    hitValuesList[^1].type = TraceType.Platform;
                    
                    if(StandingOn != hitObject)
                        SetRangeValues(hitObject.transform.position, 7, TraceType.Platform);

                    Debug.Log("Platform seen");
                }
            }
            
            else if (hitObject.layer == 7)
            {
                hitValuesList[^1].type = TraceType.Enemy;
                Debug.Log("other enemy spotted");
            }
        }

        if (playerIsHit)
        {
           playerSeen = !IsObjectBehind(PlayerTrans.position);
        }
        
        PlayerSeen = playerSeen;
        
        ActOnResults();
    }

    private void ActOnResults()
    {
        if (PlayerSeen)
        {
            SetRangeValues(PlayerTrans.position, 1, TraceType.Player);
        }
    }
    
    
    private void SetRangeValues(Vector2 position, float marginDist, TraceType type)
    {
        var dist = Vector2.Distance(position, transform.position);
        
        var closeEn = !(dist >= marginDist);

        switch (type)
        {
            case TraceType.Platform:
                PlatformInRange = closeEn;
                break;
            case TraceType.Wall:
                WallInRange = closeEn;
                break;
            case TraceType.Player:
                PlayerInAttackRange = closeEn;
                break;
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
