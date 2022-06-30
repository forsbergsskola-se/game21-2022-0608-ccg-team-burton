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
    Pursue,
    Stop
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

    private float traceInterval = 0.4f;
    private float timeSinceTrace;


    private List<HitResultValues> hitValuesList = new();
    
    public bool WallSeen { get; private set;}
    public bool GroundSeen { get; private set;}
    public bool PlayerSeen { get; private set;}
    public bool PlatformSeen { get; private set; }
    public bool PlayerBehind { get; private set; }
    
    private bool UnderAttack;
    
    private bool PlayerHit;
    public bool PlayerForgotten { get; private set; }

    private bool JumpableWallSeen;
    
    private bool WallOnTopSeen;

    private bool PlayerKnown;
    
    public Actions actions { get; private set; }

    private Health _playerHealth;
    
    public Vector2 EstimatedJumpForce { get; private set; }
    
    public bool PlayerInAttackRange { get; private set;}
    
    public Transform PlayerTrans;

    private List<HitResults> hitResultList = new();
    
    private Health _health;

    private void Awake()
    {
        _health = GetComponentInParent<Health>();
        _health.OnHealthChanged += RegisterAttack;

        PlayerForgotten = true;
        multiMask = 1 << 6 | 1 << 8;
    }

    private void OnDisable()
    {
        _health.OnHealthChanged -= RegisterAttack;
    }

    private void RegisterAttack(int currentHealth)
    {
        if (!PlayerSeen)
        {
            UnderAttack = true;
        }
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

    public int GetPlayerHealth()
        => _playerHealth.CurrentHealth;

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
       
       SetResults();
    }

    private void SetResults()
    {
        if (UnderAttack)
        {
            actions = Actions.TurnAround;
            
            UnderAttack = false;
        }
        
        if (PlayerSeen)
        {
            PlayerInAttackRange = hitResultList[1].theHit.distance < 1;
            
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
                    PlayerBehind = false;
                }
            }
        }
       
        if (GroundSeen)
        {
            
        }

        if (!GroundSeen)
        {
            var jumpForce = CheckForJumps(8);

            if (jumpForce.x != 0)
            {
                EstimatedJumpForce =  jumpForce;
                actions = Actions.PlatformJump;
            }
            
            else
            {
                actions = Actions.TurnAround;
            }
        }
        
        if (PlatformSeen)
        {
            
        }
        
        if (WallSeen)
        {
            if (hitResultList[1].theHit.distance < 1.5f && GroundSeen)
            {
                actions = Actions.TurnAround;
            }
        }

        UnderAttack = false;
    }

    private Vector2 CheckForJumps(int numberTraces)
    {
        var pos = transform.position;
        var dir = transform.right;
        var traceDistance = 7;

        FillHitResults(numberTraces, dir, pos, traceDistance, new Vector2(), new Vector2(0, 1));
        var filter2 = hitResultList.Where(x => x.theHit).ToList();
        
        if (filter2.Count == numberTraces - 1)
        {
            return Vector2.zero;
        }
        
        FillHitResults(numberTraces, new Vector2(0, -1), pos  + new Vector3(dir.x * 2, 5), traceDistance, new Vector2(), dir);
        
        var filter = hitResultList.Where(x => x.theHit).ToList();

        var theDistance = new Vector2();
        
        if (filter.Count > 3)
        {
            var yForce = Mathf.Clamp(theDistance.y, 1, 12);
          
            theDistance = filter[1].theHit.point -= (Vector2) pos;
            return new Vector2(Mathf.Abs(theDistance.x) * dir.x, yForce * 6);
        }
        
        return Vector2.zero;
    }

    private void FillHitResults(int numberTraces, Vector2 dir, Vector2 pos, float traceDistance, Vector2 dirMod = new Vector2(), Vector2 posMod = new Vector2())
    {
        hitResultList.Clear();
        var noHits = true;
        
        for (int i = 0; i < numberTraces; i++)
        {
            var traceHit = DoSingleTrace(dir, pos, traceDistance, out var hit);

            if (hit)
            {
                noHits = false;
            }
            
            HitResults results = new HitResults()
            {
                theHitType = traceHit,
                theHit = hit
            };
            hitResultList.Add(results);
            
            pos += posMod;
            dir += dirMod;
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
        
        if (layer is 6)
        {
            Debug.DrawRay(pos, dir *hit.distance, Color.green, traceInterval);
            
            return TraceType.Ground | TraceType.Wall;
        }
        else if(layer == 8)
        {
            Debug.DrawRay(pos, dir *hit.distance, Color.blue, traceInterval);
            
            return TraceType.Player;
        }

        return TraceType.None;
    }

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

                Debug.Log("Player spotted");
            }
            
            else if (hitObject.layer == 6)
            {

                if (hitObject.transform.localScale.y > 2)
                {
                    hitValuesList[^1].type = TraceType.Wall;
                    Debug.Log("Wall seen");
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
