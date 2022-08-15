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
    [SerializeField] private CapsuleCollider2D _collider2D;

    private int _groundMask;
    private int _boxMask;
    
    private float _timeSinceTrace;
    public Vector2 PlayerPos { get; private set; }
    
    private Health _playerHealth;
    private Health _enemyHealth;
    private bool _somethingHit;

    private List<RaycastHit2D> _pointsList = new();
    [HideInInspector] public CompoundActions compoundActions;
    [HideInInspector] public float distanceToWall;

    private int _maxHealth;
    private void Awake()
    {
        compoundActions |= CompoundActions.GroundSeen;
    }

    private void Start()
    {
        _enemyHealth = GetComponentInParent<Health>();
        _maxHealth = _enemyHealth.CurrentHealth;
        _enemyHealth.OnHealthChanged += RegisterAttack;

        _groundMask = 1 << 6 | 1 << 10; 
        _boxMask = 1 << 8 | 1 << 13 | 1 << 7;
    }

    private void OnDisable()
    {
        _enemyHealth.OnHealthChanged -= RegisterAttack;
    }

    private void RegisterAttack(int currentHealth)
    {
        compoundActions |= CompoundActions.EnemyAttacked;
        
        if (_somethingHit)
        {
        }

        if (currentHealth <= 0)
        {
            compoundActions = CompoundActions.EnemyDead;
        }
    }

    void Update()
    {
        _timeSinceTrace += Time.deltaTime;

        if (_timeSinceTrace >= traceInterval)
        {
            _timeSinceTrace -= traceInterval;
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
    
    private void CannonTrace()
    {
        var newMask = 1 << 8;
        var result = Physics2D.BoxCastAll(transform.position, 
            traceSize, 0, transform.up, traceSize.y / 10, newMask);
        _somethingHit = false;

        _pointsList.Clear();
        
        foreach (var h in result)
        {
            var layer = h.collider.transform.gameObject.layer;
            
            if (layer == 8)
            {
                _somethingHit = true;

                compoundActions |= CompoundActions.PlayerNoticed;
                if (CheckIfLookingAtTarget(h.point))
                {
                    compoundActions |= CompoundActions.PlayerInFront;
                }
                else
                {
                    compoundActions  &= ~CompoundActions.PlayerNoticed;
                    compoundActions  &= ~CompoundActions.PlayerInFront;
                }
            }
        }
        //Debug.Log($"Compound hit: {compoundActions}");

        if (_somethingHit) return;
        
        //Debug.Log($"Compound miss: {compoundActions}");
        
        compoundActions  &= ~CompoundActions.PlayerNoticed;
        compoundActions  &= ~CompoundActions.PlayerInFront;
    }

    private void TraceBox()
    {
        var result = Physics2D.BoxCastAll(transform.position, 
            traceSize, 0, transform.up, traceSize.y / 4, _boxMask);
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
                    Physics2D.IgnoreCollision(_collider2D, h.collider);
                    break;
                case 8:
                    SetPlayerEncounter(h);
                    playerNoticed = true;
                    break;
            }
        }

        if (playerNoticed) return;
        compoundActions  &= ~CompoundActions.PlayerInFront;
        compoundActions  &= ~CompoundActions.PlayerInAttackRange;
        compoundActions  &= ~CompoundActions.PlayerBehind;
        compoundActions  &= ~CompoundActions.PlayerNoticed;
    }
    

    private void SetPlayerEncounter(RaycastHit2D hit)
    {
        var seeing = CheckIfLookingAtTarget(hit.point);
        PlayerPos = hit.collider.transform.position;
        var distance = Vector2.Distance(PlayerPos, transform.position);

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
                compoundActions  &= ~CompoundActions.EnemyAttacked;
                
                compoundActions |= CompoundActions.AwareOfPlayer;
                compoundActions |= CompoundActions.PlayerBehind;
                compoundActions |= CompoundActions.PlayerNoticed;
            }
                    
            if (compoundActions.HasFlag(CompoundActions.PlayerNoticed))
            {
                compoundActions |= CompoundActions.PlayerBehind;
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
