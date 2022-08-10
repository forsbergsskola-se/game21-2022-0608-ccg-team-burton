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
    PlayerInAttackRange = 8
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
    public bool PlayerInAttackRange { get; private set;}
    
    public Vector2 PlayerPos { get; private set; }
    
    public bool QuitNode { get; private set; }
    
    private bool UnderAttack;
    [HideInInspector] public Transform PlayerTrans;
    private List<HitResults> hitResultList = new();
    
    private Health _playerHealth;
    private Health _enemyHealth;

    private bool JumpReady;

    private bool _somethingHit;

    public PlayerEncounter playerEncounter;



    private void Awake()
    {

        JumpReady = true;
        _enemyHealth = GetComponentInParent<Health>();
        
        _groundMask = 1 << 6 | 1 << 10; 
        _boxMask = 1 << 8 | 1 << 13;
        GroundSeen = true;
    }
    

    private void RegisterAttack(int currentHealth)
    {
        
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
        TraceBox();
    }

    private void TraceForWalls()
    {
        
    }
    
    private bool TraceForGround(Vector3 dirMod, float traceDist)
    {
        var pos = transform.position;
        var dir = transform.right + dirMod;
        
        var hit = Physics2D.Raycast(pos, dir, traceDist, _groundMask);

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

        return hit;
    }
    
    private bool IsObjectBehind(Vector2 objectPos)
    {
        return Vector2.Dot(transform.TransformDirection(Vector3.right),
            (Vector3)objectPos - transform.position) < 0;
    }
    
    private void TraceBox()
    {
        var result = Physics2D.BoxCastAll(transform.position, 
            traceSize, 0, transform.up, 8, _boxMask);
        var awareOfPlayer = false;
        
        foreach (var h in result)
        {
            if (h.collider.transform.gameObject.layer == 8)
            {
                var seeing = CheckIfLookingAtTarget(h.point);
                PlayerPos = h.collider.transform.position;
                var distance = Vector2.Distance(PlayerPos, transform.position);

                if (seeing)
                {
                    if (distance < 2)
                    {
                        playerEncounter |= PlayerEncounter.PlayerInAttackRange;
                    }
                    else
                    {
                        playerEncounter &= ~PlayerEncounter.PlayerInAttackRange;    
                    }
                    awareOfPlayer = true;
                    if (playerEncounter.HasFlag(PlayerEncounter.None))
                    {
                        playerEncounter |= PlayerEncounter.PlayerNoticed;
                    }

                    playerEncounter &= ~PlayerEncounter.PlayerBehind;
                    playerEncounter |= PlayerEncounter.PlayerInFront;
                }
                else
                {
                    if (playerEncounter.HasFlag(PlayerEncounter.PlayerNoticed))
                    {
                        awareOfPlayer = true;
                        playerEncounter &= ~PlayerEncounter.PlayerInFront;
                        playerEncounter |= PlayerEncounter.PlayerBehind;
                    }
                }
              
            }
        }
        
     //   Debug.Log(playerEncounter);
    }

    private bool CheckIfLookingAtTarget(Vector2 enemyPos)
    {
        var dirFromAtoB = ((Vector2)transform.position - enemyPos).normalized;
        var dotProd = Vector2.Dot(dirFromAtoB, transform.right);
        return dotProd < 0.9f;
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
        Gizmos.color = _somethingHit ? Color.green : Color.red;

        Gizmos.DrawWireCube(transform.position, traceSize);
    }
    #endif
}
