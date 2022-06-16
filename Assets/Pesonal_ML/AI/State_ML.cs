using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public abstract class State_ML 
{
    public STATE Name;
    protected EVENT Stage;
    protected State_ML NextStateMl;
    protected GameObject Npc;
    protected Animator _animator;
    protected EnemyVars_ML EnemyVarsMl;

    public enum STATE
    {
        Idle, Patrol, Pursue, Attack, Rest, Jump
    }

    public enum EVENT
    {
        Enter, Update, Exit
    }

    public State_ML(GameObject npc, Animator animator, EnemyVars_ML enemyVarsMl)
    {
        Stage = EVENT.Enter;
        Npc = npc;
        _animator = animator;
        EnemyVarsMl = enemyVarsMl;
    }

    public virtual void Enter()
    {
        Stage = EVENT.Update;
    }
    
    
    public virtual void Update()
    {
        Stage = EVENT.Update;
    }
    public virtual void Exit()
    {
        Stage = EVENT.Exit;
    }
    
    public State_ML Process()
    {
        switch (Stage)
        {
            case EVENT.Enter:
                Enter();
                break;
            case EVENT.Update:
                Update();
                break;
            case EVENT.Exit:
                Exit();
                return NextStateMl;
        }
        return this;
    }
}


public class Idle : State_ML
{
    private float time;
    
    public Idle(GameObject npc, Animator animator, EnemyVars_ML enemyVarsMl)
        : base( npc, animator, enemyVarsMl)
    {
        Name = STATE.Idle;
        Npc = npc;
    }
    
    public override void Update()
    {
        time += Time.deltaTime * Random.Range(0, 11);
        if (time > 10)
        {
            NextStateMl = new Patrol(Npc, _animator, EnemyVarsMl);
            Stage = EVENT.Exit;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _animator.SetBool("ExitIdleState", true);
        
    }
}

public class Patrol : State_ML
{
    private int currentIndex = 0;
    private bool incDec = true;
    public Patrol(GameObject npc, Animator animator, EnemyVars_ML enemyVarsMl)
        : base( npc, animator, enemyVarsMl)
    {
        Name = STATE.Patrol;
        Npc = npc;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        
        if (EnemyVarsMl._eyes.PlayerSeen)
        {
            Stage = EVENT.Exit;
            NextStateMl = new Pursue(Npc, _animator, EnemyVarsMl);
        }

        else if (!EnemyVarsMl._eyes.GroundSeen)
        {
            Stage = EVENT.Exit;
            NextStateMl = new Jump(Npc, _animator, EnemyVarsMl);
        }

        else
        {
            SimpleMove();
        }
    }
    
    
    private void SimpleMove()
    {
        Npc.transform.position += Npc.transform.right * (Time.deltaTime * EnemyVarsMl.GetMoveSpeed);
        
    }
    
    private void RotateSimple()
    {
        Npc.transform.Rotate(Vector3.up, 180);
    }
    
}

public class Pursue : State_ML
{
    public Pursue(GameObject npc,Animator animator, EnemyVars_ML enemyVarsMl)
        : base(npc, animator, enemyVarsMl)
    {
        Name = STATE.Pursue;
    }

    public override void Update()
    {
       
        
        if (Vector3.Distance(EnemyVarsMl._eyes.PlayerTrans.position, Npc.gameObject.transform.position) < EnemyVarsMl.GetAttackDistance)
        {
            NextStateMl = new Attack(Npc, _animator, EnemyVarsMl);
            Stage = EVENT.Exit;
        }
        else
        {
            SimpleMove();
        }
        
    }
    
    private void SimpleMove()
    {
        Npc.transform.position += Npc.transform.right * (Time.deltaTime * EnemyVarsMl.GetMoveSpeed);
        
    }
}

public class Attack : State_ML
{
    private float attackDelay;
    public Attack(GameObject npc, Animator animator, EnemyVars_ML enemyVarsMl)
        : base(npc, animator, enemyVarsMl)
    {
        Name = STATE.Attack;
    }


    public override void Enter()
    {
        base.Enter();
        _animator.SetBool("EnterCombat", true);
    }

    public override void Update()
    {
        if (!EnemyVarsMl._eyes.PlayerSeen)
        {
            
        }
        
        
        if (attackDelay >= EnemyVarsMl.GetAttackInterval)
        {
            _animator.SetInteger("Attack", 0);
            
            attackDelay -= EnemyVarsMl.GetAttackInterval;
        }
        
        attackDelay += 0.5f * Time.deltaTime;
    }
    
}
public class Jump : State_ML
{
    private bool makeJump;
    private float maxAngle = 90;
    private List<Vector2> arcPoints = new List<Vector2>();
    private Vector2 currentPoint;
    private Vector2 currentPlayerPos;
    private float lerpValue;
    private int pointCount;
    private bool start;

    public Jump(GameObject npc, Animator animator, EnemyVars_ML enemyVarsMl) 
        : base(npc, animator, enemyVarsMl)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        _animator.SetBool("ExitIdleState", false);
    }
    
    public override void Update()
    {
        if (!EnemyVarsMl.ArcCollider.TileSpotted && !start)
        {
            EnemyVarsMl.ArcCollider.CalculatePoints();
        }
        
        else if (EnemyVarsMl.ArcCollider.GetAngle > maxAngle && !start)
        {
            BackToPatrol();
        }
        else if (EnemyVarsMl.ArcCollider.SameTileSpotted)
        {
            Debug.Log("same tile");
            BackToPatrol();
        }
        
        else if(EnemyVarsMl.ArcCollider.TileSpotted && !EnemyVarsMl.ArcCollider.SameTileSpotted)
        {
            //SetArc();
            MadeJump();
           // makeJump = true;
           // pointCount++;
           // start = true;
           // currentPoint = arcPoints[pointCount];
           // currentPlayerPos = Npc.transform.position;
           // Npc.GetComponent<Rigidbody2D>().simulated = false;

        }
       

        if (makeJump && start) 
        {
            var currentPos = Vector2.Lerp(currentPlayerPos, currentPoint, lerpValue);
            lerpValue += 6f * Time.deltaTime;

            if (lerpValue > 0.95f)
            {
                lerpValue = 0;
                
                pointCount++;
                if (pointCount < arcPoints.Count)
                {
                    currentPoint = arcPoints[pointCount];
                    currentPlayerPos  = Npc.transform.position;
                }

                else
                {
                    MadeJump();
                }
            }
            
            Npc.transform.position = new Vector3(currentPos.x, currentPos.y);
        
        }
        
    }

    public void MadeJump()
    {
        SetArc();
        var forward = Npc.gameObject.transform.right;
        var impulse2 = EnemyVarsMl.ArcCollider.GetImpulse();
        var diff = EnemyVarsMl.ArcCollider.TileHeightDifference;
        if (diff < 0)
        {
            diff = 2;
        }
        
        else
        {
            diff *= 7;
            diff = Mathf.Clamp(diff, 0, 7);
        }
        
        var impulse = new Vector2(forward.x  * (EnemyVarsMl.ArcCollider.GetLengthDifference() * 0.75f), diff);
        Debug.Log(EnemyVarsMl.ArcCollider.GetLengthDifference());
        
       // Debug.Log(arcPoints[0]);
       // Debug.Log(arcPoints[^1]);
       Npc.GetComponent<Rigidbody2D>().AddForce(impulse , ForceMode2D.Impulse); 
       // Npc.GetComponent<Rigidbody2D>().AddForce(new Vector2(forward.x * 3f, 5f), ForceMode2D.Impulse);
       // makeJump = true;
        EnemyVarsMl.ArcCollider.ResetCollider();
        
        NextStateMl = new Idle(Npc, _animator, EnemyVarsMl);
        Stage = EVENT.Exit;
    }

    private void SetArc()
    {
        var worldPos = Npc.transform.position;
        
        for (int i = 0; i < EnemyVarsMl.ArcCollider.GetNumberArcPoints() / 2; i++)
        {
            var  aPoint = EnemyVarsMl.ArcCollider.GetPoint(i);
            arcPoints.Add(new Vector2(worldPos.x + aPoint.x, worldPos.y + aPoint.y));
        }
    }
    
    private void BackToPatrol()
    {
        Npc.transform.Rotate(Vector3.up, 180);
        EnemyVarsMl.ArcCollider.ResetCollider();
        NextStateMl = new Idle(Npc, _animator, EnemyVarsMl);
        Stage = EVENT.Exit;
    }
}