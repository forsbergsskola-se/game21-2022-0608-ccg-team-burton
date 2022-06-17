using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public abstract class State_ML 
{
    public STATE Name;
    protected EVENT Stage;
    protected State_ML NextStateMl;
    protected EnemyVars_ML EnemyVarsMl;

    public enum STATE
    {
        Idle, Patrol, Pursue, Attack, Rest, Jump
    }

    public enum EVENT
    {
        Enter, Update, Exit
    }

    public State_ML(EnemyVars_ML enemyVarsMl)
    {
        Stage = EVENT.Enter;
    
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
    
    public Idle(EnemyVars_ML enemyVarsMl)
        : base(enemyVarsMl)
    {
        Name = STATE.Idle;
    }
    
    public override void Update()
    {
        time += Time.deltaTime * Random.Range(0, 11);
        if (time > 10)
        {
            NextStateMl = new Patrol(EnemyVarsMl);
            Stage = EVENT.Exit;
        }
    }

    public override void Exit()
    {
        base.Exit();
      //  _animator.SetBool("ExitIdleState", true);
        
    }
}

public class Patrol : State_ML
{
    private int currentIndex = 0;
    private bool incDec = true;
    private float delay = 1;
    public Patrol(EnemyVars_ML enemyVarsMl)
        : base(enemyVarsMl)
    {
        Name = STATE.Patrol;
    }

    public override void Enter()
    {
        base.Enter();
        EnemyVarsMl.animator.SetBool(Animator.StringToHash("ExitIdleState"), true);
    }

    public override void Update()
    {
        
        if (EnemyVarsMl._eyes.PlayerSeen)
        {
            Stage = EVENT.Exit;
            NextStateMl = new Pursue(EnemyVarsMl);
        }

        else if (!EnemyVarsMl._eyes.GroundSeen)
        {
            Stage = EVENT.Exit;
            NextStateMl = new Jump(EnemyVarsMl);
        }

        if (delay < 0)
        {
            SimpleMove();
        }

        delay -= Time.deltaTime * 2f;
    }
    
    
    private void SimpleMove()
    {
        EnemyVarsMl.enemyRef.transform.position += EnemyVarsMl.enemyRef.transform.right * (Time.deltaTime * EnemyVarsMl.GetMoveSpeed);
        
    }
}

public class Pursue : State_ML
{
    public Pursue(EnemyVars_ML enemyVarsMl)
        : base(enemyVarsMl)
    {
        Name = STATE.Pursue;
    }

    public override void Update()
    {
        if (Vector3.Distance(EnemyVarsMl._eyes.PlayerTrans.position, EnemyVarsMl.enemyRef.gameObject.transform.position) < EnemyVarsMl.GetAttackDistance)
        {
            NextStateMl = new Attack(EnemyVarsMl);
            Stage = EVENT.Exit;
        }
        else
        {
            SimpleMove();
        }
    }
    
    private void SimpleMove()
    {
        EnemyVarsMl.enemyRef.transform.position += EnemyVarsMl.enemyRef.transform.right * (Time.deltaTime * EnemyVarsMl.GetMoveSpeed);
    }
}

public class Attack : State_ML
{
    private float attackDelay;
    public Attack(EnemyVars_ML enemyVarsMl)
        : base(enemyVarsMl)
    {
        Name = STATE.Attack;
    }
    
    public override void Enter()
    {
        base.Enter();
        EnemyVarsMl.animator.SetBool(Animator.StringToHash("EnterCombat"), true);
    }

    public override void Update()
    {

        if (!EnemyVarsMl._eyes.PlayerSeen)
        {
          
            var dotProd = Vector3.Dot(EnemyVarsMl.enemyRef.transform.right, EnemyVarsMl._eyes.PlayerTrans.position);

            if (dotProd < 1)
            {
             //   TurnAround();
            }
        }
        
        
        if (attackDelay >= EnemyVarsMl.GetAttackInterval && EnemyVarsMl._eyes.PlayerSeen)
        { 
            EnemyVarsMl.animator.SetTrigger(Animator.StringToHash("MakeAttack"));

            attackDelay -= EnemyVarsMl.GetAttackInterval;
        }

        else
        {
            
        //    EnemyVarsMl.animator.SetInteger("Attack", 3);
        }
        
        attackDelay += 0.5f * Time.deltaTime;
    }

    private void TurnAround()
    {
        EnemyVarsMl.enemyRef.transform.Rotate(Vector3.up, 180);
        NextStateMl = new Patrol(EnemyVarsMl);
    }
    
    public override void Exit()
    {
        base.Exit();
        EnemyVarsMl.animator.SetBool(Animator.StringToHash("EnterCombat"), false);
        
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
    private float jumpDelay = 2;

    public Jump(EnemyVars_ML enemyVarsMl) 
        : base(enemyVarsMl)
    {
        Name = STATE.Jump;
    }
    
    public override void Enter()
    {
        base.Enter();
        EnemyVarsMl.animator.SetBool(Animator.StringToHash("ExitIdleState"), false);
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
            BackToPatrol();
        }
        
        else if(EnemyVarsMl.ArcCollider.TileSpotted && !EnemyVarsMl.ArcCollider.SameTileSpotted)
        {
            jumpDelay -= Time.deltaTime * 2f;
            
            //SetArc();

            if (jumpDelay < 0)
            {
                MakeJump();
            }
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
                    currentPlayerPos  = EnemyVarsMl.enemyRef.transform.position;
                }

                else
                {
                    MakeJump();
                }
            }
            
            EnemyVarsMl.enemyRef.transform.position = new Vector3(currentPos.x, currentPos.y);
        
        }
        
    }

    public void MakeJump()
    {
        EnemyVarsMl.animator.SetTrigger(Animator.StringToHash("Jump"));
        SetArc();
        var forward = EnemyVarsMl.enemyRef.gameObject.transform.right;
        var impulse2 = EnemyVarsMl.ArcCollider.GetImpulse();
        var diff = EnemyVarsMl.ArcCollider.TileHeightDifference;
        if (diff < 0)
        {
            diff = 4;
        }
        
        else
        {
            diff *= 7;
            diff = Mathf.Clamp(diff, 0, 7);
        }
        
        var impulse = new Vector2(forward.x  * (EnemyVarsMl.ArcCollider.GetLengthDifference() * 0.75f), diff);

        EnemyVarsMl.enemyRef.GetComponent<Rigidbody2D>().AddForce(impulse , ForceMode2D.Impulse); 
    
        EnemyVarsMl.ArcCollider.ResetCollider();
        NextStateMl = new Idle(EnemyVarsMl);
        Stage = EVENT.Exit;
    }

    private void SetArc()
    {
        var worldPos = EnemyVarsMl.enemyRef.transform.position;
        
        for (int i = 0; i < EnemyVarsMl.ArcCollider.GetNumberArcPoints() / 2; i++)
        {
            var  aPoint = EnemyVarsMl.ArcCollider.GetPoint(i);
            arcPoints.Add(new Vector2(worldPos.x + aPoint.x, worldPos.y + aPoint.y));
        }
    }
    
    private void BackToPatrol()
    {
        EnemyVarsMl.enemyRef.transform.Rotate(Vector3.up, 180);
        EnemyVarsMl.ArcCollider.ResetCollider();
        NextStateMl = new Idle(EnemyVarsMl);
        Stage = EVENT.Exit;
    }
}