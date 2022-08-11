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
    EnemyAttacked = 16
}

[Flags]
public enum CompoundActions
{
    None = 0,
    CanJump = 1,
    CantJump = 2,
    Walk = 4,
    Rotate = 8
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
    
    private int _groundMask;
    private int _boxMask;

    private float traceInterval = 0.4f;
    private float timeSinceTrace;
    
    public bool WallSeen { get; private set;}
    public bool GroundSeen { get; private set;}
    
    public Vector2 EstimatedJumpForce { get; private set; }

    public Vector2 PlayerPos { get; private set; }
    
    public bool QuitNode { get; private set; }
    
    private bool UnderAttack;
    [HideInInspector] public Transform PlayerTrans;
  
    private Health _playerHealth;
    private Health _enemyHealth;
    private bool _somethingHit;

    public PlayerEncounter playerEncounter;
    private bool _enemyHit;

    private List<RaycastHit2D> _pointsList = new();
    public CompoundActions compoundActions;

    public event Action CheckForJump;
    
    private void Awake()
    {
        GetComponentInParent<BehaviourTreeRunner>().CheckForJump += JumpCheck;
        GetComponentInParent<Health>().OnHealthChanged += RegisterAttack;

        _groundMask = 1 << 6 | 1 << 10; 
        _boxMask = 1 << 8 | 1 << 13 | 1 << 7 ;
        GroundSeen = true;
    }


    private void JumpCheck(Action<CompoundActions> callback)
    {
        
        callback.Invoke(CompoundActions.CanJump);
    }
    
    private void OnDisable()
    {
        GetComponentInParent<Health>().OnHealthChanged -= RegisterAttack;
        GetComponentInParent<BehaviourTreeRunner>().CheckForJump -= JumpCheck;
    }

    private void RegisterAttack(int currentHealth)
    {
        playerEncounter |= PlayerEncounter.EnemyAttacked;
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
        GroundSeen = TraceForGround(new Vector3(0, -1), 2);
        WallSeen = TraceForGround(new Vector3(0, 0), 8);

        if (!GroundSeen)
        {
            DetermineJump();    
        }

        //  TraceBox();
    }
    
    private bool TraceForGround(Vector3 dirMod, float traceDist)
    {
        var pos = transform.position;
        var dir = transform.right + dirMod;
        
        var hit = Physics2D.Raycast(pos, dir, traceDist, _groundMask);

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

    private void DetermineJump()
    {
        Debug.Log("checking jump");
        
        var result = Physics2D.BoxCastAll(transform.position, 
            traceSize, 0, transform.up, 8, _boxMask);
        _somethingHit = false;
        
        foreach (var h in result)
        {
            _somethingHit = true;
            var layer = h.collider.transform.gameObject.layer;

            switch (layer)
            {
                case 7:
                    break;
                case 8:
                    break;
                case 13:
                    if (_pointsList.SingleOrDefault(x => x.point == h.point) != default)
                    {
                        _pointsList.Add(h);
                    }
                    break;
            }
        }
        
        _pointsList = _pointsList
            .OrderBy(x => x.distance).ToList();

        var mag = _pointsList[0].point - _pointsList[^1].point;
        
        if (mag.x > 5)
        {
            compoundActions = CompoundActions.CantJump;
        }
    }

    private void TraceBox()
    {
        var result = Physics2D.BoxCastAll(transform.position, 
            traceSize, 0, transform.up, 8, _boxMask);
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
                case 13:
                    if (_pointsList.SingleOrDefault(x => x.point == h.point) != default)
                    {
                        _pointsList.Add(h);
                    }
                    break;
            }
        }

        
        if (!GroundSeen)
        {
            if (_pointsList.Count > 1)
            {
                SetPointEncounter();
            }
            else
            {
                compoundActions |= CompoundActions.CantJump;
            }
        }
    
        
        if (!playerNoticed)
        {
            playerEncounter = PlayerEncounter.None;
        }
    }

    private void SetPointEncounter()
    {
        _pointsList = _pointsList
            .OrderBy(x => x.distance).ToList();

        var mag = _pointsList[0].point - _pointsList[^1].point;
        
        if (mag.x > 5)
        {
            compoundActions = CompoundActions.CantJump;
        }
        else
        {
            
        }
        
        foreach (var p in _pointsList)
        {
          //  Debug.Log($"{plus++}: {p.point}, {p.distance}");
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
            
            if (distance < 2)
            {
                playerEncounter |= PlayerEncounter.PlayerInAttackRange;
            }
        }
        
        else
        {
            if (playerEncounter.HasFlag(PlayerEncounter.EnemyAttacked))
            {
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
