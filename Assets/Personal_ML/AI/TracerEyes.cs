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
    PlayerInFront = 4,
    PlayerInAttackRange = 8,
    EnemyAttacked = 16,
    AwareOfPlayer = 32
}

[Flags]
public enum CompoundActions
{
    None = 1 << 0,
    CanJump = 1 << 1,
    WalkToTarget = 1 << 2,
    KeepWalking = 1 << 3,
    Rotate = 1 << 4,
    GroundSeen = 1 << 5,
    WallSeen= 1 << 6,
    MakingJump = 1 << 7,
    PlayerNoticed = 1 << 8,
    PlayerBehind = 1 << 9,
    PlayerInFront = 1 << 10,
    PlayerInAttackRange = 1 << 11,
    EnemyAttacked = 1 << 12,
    AwareOfPlayer = 1 << 13,
    EnemyDead = 1 << 14,
    WallInTurnRange = 1 << 15,
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
    [SerializeField, Range(0.2f, 1.5f)]private float traceInterval = 0.4f;

    private int _groundMask;
    private int _boxMask;

    private float timeSinceTrace;
    
    public bool WallSeen { get; private set;}
    public bool GroundSeen { get; private set;}
    
    public Vector2 EstimatedJumpForce { get; private set; }

    public Vector2 PlayerPos { get; private set; }
    
    
    private bool UnderAttack;
    [HideInInspector] public Transform PlayerTrans;
  
    private Health _playerHealth;
    private Health _enemyHealth;
    private bool _somethingHit;

    public PlayerEncounter playerEncounter;
    private bool _enemyHit;

    private List<RaycastHit2D> _pointsList = new();
    public CompoundActions compoundActions;

    public float distanceToWall;
    
    private void Awake()
    {
        GetComponentInParent<BehaviourTreeRunner>().CheckForJump += JumpCheck;
        GetComponentInParent<Health>().OnHealthChanged += RegisterAttack;

        _groundMask = 1 << 6 | 1 << 10; 
        _boxMask = 1 << 8 | 1 << 13 | 1 << 7;
        GroundSeen = true;
    }
    
    private void OnDisable()
    {
        GetComponentInParent<Health>().OnHealthChanged -= RegisterAttack;
        GetComponentInParent<BehaviourTreeRunner>().CheckForJump -= JumpCheck;
    }

    private void RegisterAttack(int currentHealth)
    {
        playerEncounter |= PlayerEncounter.EnemyAttacked;
        compoundActions |= CompoundActions.EnemyAttacked;
    }

    void Update()
    {
        timeSinceTrace += Time.deltaTime;

        if (timeSinceTrace >= traceInterval)
        {
            timeSinceTrace -= traceInterval;
            DoAllTraces();
        }
    }

    public int GetPlayerHealth()
        => _playerHealth.CurrentHealth;

    private void DoAllTraces()
    {
        if (TraceForGround(new Vector3(0, -1), 2)) 
            compoundActions |= CompoundActions.GroundSeen;
        else compoundActions &= ~CompoundActions.GroundSeen;
        
        if (TraceForGround(new Vector3(0, 0), 8)) 
            compoundActions |= CompoundActions.WallSeen;
        else compoundActions &= ~CompoundActions.WallSeen;


        if (compoundActions.HasFlag(CompoundActions.WallSeen))
        {
            if (distanceToWall < 1)
            {
                compoundActions |= CompoundActions.WallInTurnRange;
            }
        }
        else
        {
            compoundActions &= ~CompoundActions.WallInTurnRange;
        }

        if (!compoundActions.HasFlag(CompoundActions.GroundSeen))
        {
           OtherJumpCheck();
        }
        
        TraceBox();
    }

    private bool TraceForGround(Vector3 dirMod, float traceDist)
    {
        var pos = transform.position;
        var dir = transform.right + dirMod;
        
        var hit = Physics2D.Raycast(pos, dir, traceDist, _groundMask);

        distanceToWall = hit.distance;
        
        if (hit)
        {
            
            Debug.DrawRay(pos, dir *hit.distance, Color.green, traceInterval);
        }
        else
        {
            
            Debug.DrawRay(pos, dir *traceDist, Color.red, traceInterval);
        }

        return hit;
    }

    private void OtherJumpCheck()
    {
        var pos = transform.position;
        var result = Physics2D.BoxCastAll(pos, 
            traceSize, 0, transform.up, traceSize.y / 10, _boxMask);
        _somethingHit = false;
        _pointsList.Clear();
        
        compoundActions &= ~CompoundActions.CanJump;
        compoundActions &= ~CompoundActions.KeepWalking;
        compoundActions &= ~CompoundActions.Rotate;

        foreach (var h in result)
        {
            _somethingHit = true;
            var layer = h.collider.transform.gameObject.layer;

            if (layer == 13)
            {
                if (_pointsList.SingleOrDefault(x => x.point == h.point) == default)
                {
                    _pointsList.Add(h);
                }
            }
        }

        _pointsList = _pointsList
            .OrderBy(x => x.distance).ToList();

        var mag = _pointsList[0].point - _pointsList[^1].point;

        if ( _pointsList.Count < 2 || mag.x > 7)
        {
           
            compoundActions |=  CompoundActions.KeepWalking | CompoundActions.Rotate;
        }
        else
        {
            compoundActions |= CompoundActions.CanJump;
        }
    }
    
    private void JumpCheck(Action<CompoundActions> callback)
    {
        var pos = transform.position;
        var result = Physics2D.BoxCastAll(pos, 
            traceSize, 0, transform.up, traceSize.y / 10, _boxMask);
        _somethingHit = false;
        _pointsList.Clear();
        
        
        var comp = CompoundActions.None;

        foreach (var h in result)
        {
            _somethingHit = true;
            var layer = h.collider.transform.gameObject.layer;

            if (layer == 13)
            {
                if (_pointsList.SingleOrDefault(x => x.point == h.point) == default)
                {
                    _pointsList.Add(h);
                }
            }
        }

        _pointsList = _pointsList
            .OrderBy(x => x.distance).ToList();

        var mag = _pointsList[0].point - _pointsList[^1].point;

        if ( _pointsList.Count < 2 || mag.x > 7)
        {
            comp |=  CompoundActions.KeepWalking | CompoundActions.Rotate;
        }
        else
        {
            comp |= CompoundActions.CanJump;
        }
        
        callback.Invoke(comp);
    }
    
    private void TraceBox()
    {
        var result = Physics2D.BoxCastAll(transform.position, 
            traceSize, 0, transform.up, traceSize.y / 2, _boxMask);
        _somethingHit = false;
        var playerNoticed = false;

        _pointsList.Clear();

        foreach (var h in result)
        {
            _somethingHit = true;
            var layer = h.collider.transform.gameObject.layer;

            switch (layer)
            {
                case 7:
                    break;
                case 8:
                    SetPlayerEncounter(h);
                    playerNoticed = true;
                    break;
            }
        }

        if (!playerNoticed)
        {
            playerEncounter = PlayerEncounter.None;
        }
    }
    

    private void SetPlayerEncounter(RaycastHit2D hit)
    {
        var seeing = CheckIfLookingAtTarget(hit.point);
        PlayerPos = hit.collider.transform.position;
        var distance = Vector2.Distance(PlayerPos, transform.position);
        var awareOfPlayer = false;
        
        playerEncounter &= ~PlayerEncounter.PlayerInFront;
        playerEncounter &= ~PlayerEncounter.PlayerInAttackRange;
        playerEncounter &= ~PlayerEncounter.PlayerBehind;
        playerEncounter &= ~PlayerEncounter.PlayerNoticed;
        
        if (seeing)
        {
            playerEncounter |= PlayerEncounter.PlayerInFront;
            playerEncounter |= PlayerEncounter.PlayerNoticed;
            playerEncounter |= PlayerEncounter.AwareOfPlayer;
            
            if (distance < 2)
            {
                playerEncounter |= PlayerEncounter.PlayerInAttackRange;
            }
        }
        
        else
        {
            if (playerEncounter.HasFlag(PlayerEncounter.EnemyAttacked))
            {
                playerEncounter |= PlayerEncounter.AwareOfPlayer;
                playerEncounter |= PlayerEncounter.PlayerBehind;
                playerEncounter |= PlayerEncounter.PlayerNoticed;
            }
                    
            if (playerEncounter.HasFlag(PlayerEncounter.PlayerNoticed))
            {
                playerEncounter |= PlayerEncounter.PlayerBehind;
            }
        }
    }

    private bool CheckIfLookingAtTarget(Vector2 enemyPos)
    {
        var dirFromAtoB = ((Vector2)transform.position - enemyPos).normalized;
        var dotProd = Vector2.Dot(dirFromAtoB, transform.right);
        return dotProd < 0.9f;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = _somethingHit ? Color.green : Color.red;

        Gizmos.DrawWireCube(transform.position, traceSize);
    }
#endif
}
