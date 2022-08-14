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
public enum CompoundActions
{
    None = 1 << 0,
    CanJump = 1 << 1,
    WalkToTarget = 1 << 2,
    KeepWalking = 1 << 3,
    Rotate = 1 << 4,
    GroundSeen = 1 << 5,
    WallSeen = 1 << 6,
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

public enum EnemyType
{
    Melee,
    Cannon
}

public class TracerEyes : MonoBehaviour
{
    [Header("Eye values")]
    [SerializeField] public float pursueDistance;
    [SerializeField] public Vector2 traceSize;
    [SerializeField, Range(0.2f, 1.5f)]private float traceInterval = 0.4f;
    [SerializeField] private EnemyType enemyType;
    private int _groundMask;
    private int _boxMask;

    [HideInInspector] public LevelGrid grid;

    private float timeSinceTrace;
    
    public Vector2 EstimatedJumpForce { get; private set; }

    public Vector2 PlayerPos { get; private set; }
    
    
    private bool UnderAttack;

    private Health _playerHealth;
    private Health _enemyHealth;
    private bool _somethingHit;

    private bool _enemyHit;

    private List<RaycastHit2D> _pointsList = new();
    [HideInInspector] public Transform PlayerTrans;
    [HideInInspector] public CompoundActions compoundActions;

    public float distanceToWall;

    private void Awake()
    {
        compoundActions |= CompoundActions.GroundSeen;
    }

    private void Start()
    {
        
        GetComponentInParent<BehaviourTreeRunner>().CheckForJump += JumpCheck;
        GetComponentInParent<Health>().OnHealthChanged += RegisterAttack;

        _groundMask = 1 << 6 | 1 << 10 | 1 << 7; 
        _boxMask = 1 << 8 | 1 << 13 | 1 << 7;
    }

    private void OnDisable()
    {
        GetComponentInParent<Health>().OnHealthChanged -= RegisterAttack;
        GetComponentInParent<BehaviourTreeRunner>().CheckForJump -= JumpCheck;
    }

    private void RegisterAttack(int currentHealth)
    {
        compoundActions |= CompoundActions.EnemyAttacked;

        if (currentHealth <= 0)
        {
            compoundActions = CompoundActions.EnemyDead;
        }
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
        if (enemyType == EnemyType.Melee)
        {
            var groundTrace = TraceForGround(new Vector3(0, -0.45f), 2);
            var wallTrace = TraceForGround(new Vector3(0, 0), 8);
                
            if (groundTrace) compoundActions |= CompoundActions.GroundSeen;
            else compoundActions &= ~CompoundActions.GroundSeen;
            
            if (wallTrace) compoundActions |= CompoundActions.WallSeen;
            else compoundActions &= ~CompoundActions.WallSeen;
            
            if (compoundActions.HasFlag(CompoundActions.WallSeen))
            {
                if (distanceToWall < 0.9f)
                {
                    compoundActions |= CompoundActions.WallInTurnRange;
                }
            }
            else
            {
                compoundActions &= ~CompoundActions.WallInTurnRange;
            }
            
            TraceBox();
           // Debug.Log(compoundActions);
        }
        
        else if (enemyType == EnemyType.Cannon)
        {
            CannonTrace();
        }
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

    private void CannonTrace()
    {
        var newMask = 1 << 8;
        var result = Physics2D.BoxCastAll(transform.position, 
            traceSize, 0, transform.up, traceSize.y, newMask);
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
                    playerNoticed = true;
                    break;
            }
        }

        if (playerNoticed)
        {
            compoundActions |= CompoundActions.PlayerNoticed;
        }
        else
        {
            compoundActions  &= ~CompoundActions.PlayerNoticed;
        }

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
            compoundActions  &= ~CompoundActions.PlayerInFront;
            compoundActions  &= ~CompoundActions.PlayerInAttackRange;
            compoundActions  &= ~CompoundActions.PlayerBehind;
            compoundActions  &= ~CompoundActions.PlayerNoticed;
        }
    }
    

    private void SetPlayerEncounter(RaycastHit2D hit)
    {
        var seeing = CheckIfLookingAtTarget(hit.point);
        PlayerPos = hit.collider.transform.position;
        var distance = Vector2.Distance(PlayerPos, transform.position);
        var awareOfPlayer = false;
        
        if (seeing)
        {
            compoundActions |= CompoundActions.PlayerNoticed;
            compoundActions |= CompoundActions.PlayerInFront;
            compoundActions |= CompoundActions.AwareOfPlayer;
            
            compoundActions  &= ~CompoundActions.PlayerBehind;

            if (distance < 2)
            {
                compoundActions |= CompoundActions.PlayerInAttackRange;
            }
            else
            {
                compoundActions  &= ~CompoundActions.PlayerInAttackRange;
            }
        }
        
        else
        {
            if (compoundActions.HasFlag(CompoundActions.EnemyAttacked))
            {
                compoundActions  &= ~CompoundActions.PlayerInFront;
                
                compoundActions |= CompoundActions.AwareOfPlayer;
                compoundActions |= CompoundActions.PlayerBehind;
                compoundActions |= CompoundActions.PlayerNoticed;
            }
                    
            if (compoundActions.HasFlag(CompoundActions.PlayerNoticed))
            {
                compoundActions |= CompoundActions.PlayerBehind;
            }
        }
      // Debug.Log(compoundActions);
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
