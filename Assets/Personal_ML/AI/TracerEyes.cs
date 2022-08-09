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
    None = 0,
    Ground = 1,
    Wall = 2,
    Player = 4,
    Platform = 8,
    Enemy = 16,
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

[Flags]
public enum PlayerEncounter
{
    None = 0,
    PlayerNoticed = 1,
    PlayerBehind = 2,
    PlayerInFront = 4
}

[Serializable]
public class HitResults
{
    public RaycastHit2D theHit;
    public TraceType theHitType;
}

public class TracerEyes : MonoBehaviour
{
    [Header("Eye values")]
    [SerializeField] public float pursueDistance;
    [SerializeField] public Vector2 traceSize;
    
    private int multiMask;

    private float traceInterval = 0.4f;
    private float timeSinceTrace;

    
    public bool WallSeen { get; private set;}
    public bool GroundSeen { get; private set;}
    public bool PlayerSeen { get; private set;}
    public bool PlatformSeen { get; private set; }
    public bool PlayerBehind { get; private set; }
    public bool PlayerForgotten { get; private set; }
    public Actions Actions { get; private set; }
    public Vector2 EstimatedJumpForce { get; private set; }
    public bool PlayerInAttackRange { get; private set;}
    
    public bool QuitNode { get; private set; }
    
    private bool UnderAttack;
    [HideInInspector] public Transform PlayerTrans;
    private List<HitResults> hitResultList = new();
    
    private Health _playerHealth;
    private Health _enemyHealth;

    private bool JumpReady;

    private void Awake()
    {
        JumpReady = true;
        _enemyHealth = GetComponentInParent<Health>();
        _enemyHealth.OnHealthChanged += RegisterAttack;

        PlayerForgotten = true;
        multiMask = 1 << 6 | 1 << 8;
        GroundSeen = true;
    }

    private void OnDisable()
    {
        _enemyHealth.OnHealthChanged -= RegisterAttack;
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
            //DoMultiTrace();
            TraceForGround();
        }
    }

    public int GetPlayerHealth()
        => _playerHealth.CurrentHealth;
    
    
    private void TraceForGround()
    {
        var pos = transform.position;
        var dir = transform.right + new Vector3(0, -1);
        var traceDist = 2;
        var hit = Physics2D.Raycast(pos, dir, traceDist, multiMask);

        if (hit)
        {
            Debug.DrawRay(pos, dir *hit.distance, Color.green, traceInterval);
            QuitNode = true;
        }
        else
        {
            Debug.DrawRay(pos, dir *traceDist, Color.red, traceInterval);
            QuitNode = false;
        }

        GroundSeen = hit;
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
        Actions = Actions.None;
        
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
            Actions = Actions.TurnAround;
            
            UnderAttack = false;
        }
        
        if (PlayerSeen)
        {
            PlayerInAttackRange = hitResultList[1].theHit.distance < 0.6f;
            
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
                    Actions = Actions.TurnAround;
                    PlayerBehind = false;
                }
            }
        }

        if (!GroundSeen)
        {
            var jumpForce = CheckForJumps(8);

            if (jumpForce.x != 0)
            {
                EstimatedJumpForce =  jumpForce;
                Actions = Actions.PlatformJump;
            }
            
            else
            {
                Actions = Actions.TurnAround;
            }
        }
        
        if (PlatformSeen)
        {
            
        }
        
        if (WallSeen)
        {
            if (hitResultList[1].theHit.distance < 1.5f && GroundSeen)
            {
                Actions = Actions.TurnAround;
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
            var yForce = Mathf.Clamp(theDistance.y, 1, 3);
          
            theDistance = filter[1].theHit.point -= (Vector2) pos;
            return new Vector2(Mathf.Abs(theDistance.x) * dir.x, yForce * 3);
        }
        
        return Vector2.zero;
    }

    private void FillHitResults(int numberTraces, Vector2 dir, Vector2 pos, float traceDistance, Vector2 dirMod = new Vector2(), Vector2 posMod = new Vector2())
    {
        hitResultList.Clear();

        for (int i = 0; i < numberTraces; i++)
        {
            var traceHit = DoSingleTrace(dir, pos, traceDistance, out var hit);
            
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
        else if(layer is 8)
        {
            Debug.DrawRay(pos, dir *hit.distance, Color.blue, traceInterval);
            
            return TraceType.Player;
        }

        return TraceType.None;
    }

    private void TraceBox()
    {
        var sizeY = 7f;
        var sizeX = pursueDistance;
        var boxPlacement = transform.position + new Vector3(0, sizeY / 2 - 1);
        var result = Physics2D.BoxCastAll(boxPlacement , 
            new Vector2(pursueDistance * 2, sizeY), 0, transform.forward, 8, multiMask);
        var playerSeen = false;
        var playerIsHit = false;

        foreach (var h in result)
        {
            Debug.Log(h.collider.transform.name);
            Debug.Log(h.collider.transform.position);
        }
    }

  
    private IEnumerator JumpInProgress()
    {
        JumpReady = false;
        yield return new WaitForSeconds(2f);
        JumpReady = true;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, traceSize);
    }
    #endif
}
