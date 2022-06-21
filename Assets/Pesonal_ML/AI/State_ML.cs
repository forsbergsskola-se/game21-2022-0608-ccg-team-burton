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
        Debug.Log("Idle state");
        Name = STATE.Idle;
    }
    
    public override void Update()
    {
        time += Time.deltaTime * Random.Range(0, 11);
        if (time > 10)
        {
            if ((int) EnemyVarsMl.GetEnemyType < 2)
            {
                NextStateMl = new Patrol(EnemyVarsMl);
            }
            else
            {
                NextStateMl = new Sentry(EnemyVarsMl);
            }

            Stage = EVENT.Exit;
        }
    }

    public override void Exit()
    {
        base.Exit();
        EnemyVarsMl.animator.SetBool(Animator.StringToHash("ExitIdleState"), true);

    }
}


public class Sentry : State_ML
{
    public Sentry(EnemyVars_ML enemyVarsMl)
        : base(enemyVarsMl)
    {
        
    }
    
   
    public override void Update()
    {
       
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
        Debug.Log("Patrol state");
        Name = STATE.Patrol;
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
           
            NextStateMl = new Pursue(EnemyVarsMl);
        }

        else if (!EnemyVarsMl._eyes.GroundSeen)
        {
            Stage = EVENT.Exit;
            Debug.Log("Ground not seen");
           
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
        Debug.Log("Pursue state");
        Name = STATE.Pursue;
    }

    public override void Update()
    {
        var distance = Vector3.Distance(EnemyVarsMl._eyes.PlayerTrans.position, EnemyVarsMl.enemyRef.gameObject.transform.position);
        if (EnemyVarsMl.attackZone.playerInZone)
        {
            NextStateMl = new Attack(EnemyVarsMl);
            Stage = EVENT.Exit;
        }
        
        else if (distance > EnemyVarsMl.GetAttackDistance + 6)
        {
            NextStateMl = new Patrol(EnemyVarsMl);
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
        Debug.Log("Attack state");
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
                TurnAround();
            }
            else
            {
                Stage = EVENT.Exit;
                NextStateMl = new Idle(EnemyVarsMl);
            }
        }
        
        if (attackDelay >= EnemyVarsMl.GetAttackInterval && EnemyVarsMl._eyes.PlayerSeen)
        { 
            EnemyVarsMl.animator.SetTrigger(Animator.StringToHash("MakeAttack"));

            if (EnemyVarsMl.GetEnemyType == EnemyType.Ranged)
            {
                EffectsPool.RequestEffectStatic(EffectType.FireBall, EnemyVarsMl.firePoint.position, EnemyVarsMl.enemyRef.transform.right);
            }

            attackDelay -= EnemyVarsMl.GetAttackInterval;
        }
        
        attackDelay += 1f * Time.deltaTime;
    }

    private void TurnAround()
    {
        EnemyVarsMl.enemyRef.transform.Rotate(Vector3.up, 180);
        Stage = EVENT.Exit;
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
    private float maxAngle = 90;
    private bool start;
    private float jumpDelay = 2;
    private Rigidbody2D body;

    public Jump(EnemyVars_ML enemyVarsMl) 
        : base(enemyVarsMl)
    {
        Debug.Log("Jump state");
        Name = STATE.Jump;
        body = EnemyVarsMl.enemyRef.GetComponent<Rigidbody2D>();
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
        
        else if (EnemyVarsMl.ArcCollider.TileHeightDifference > 4)
        {
            BackToPatrol();
        }
        
        else if(EnemyVarsMl.ArcCollider.TileSpotted && !EnemyVarsMl.ArcCollider.SameTileSpotted)
        {
            jumpDelay -= Time.deltaTime * 2f;
            
            
            if (jumpDelay < 0)
            {
                MakeJump();
            }
        }
    }

    public void MakeJump()
    {
        EnemyVarsMl.animator.SetTrigger(Animator.StringToHash("Jump"));
        var forward = EnemyVarsMl.enemyRef.gameObject.transform.right;
        var impulse2 = EnemyVarsMl.ArcCollider.GetImpulse();
        var diff = EnemyVarsMl.ArcCollider.TileHeightDifference;
        if (diff < 0)
        {
            diff = 6;
        }
        
        else
        {
            diff *= 100;
            diff = Mathf.Clamp(diff, 0, 7);
        }
        
        var impulse = new Vector2(forward.x  * (EnemyVarsMl.ArcCollider.GetLengthDifference() * 0.75f), diff);

        body.AddForce(impulse , ForceMode2D.Impulse); 
    
        EnemyVarsMl.ArcCollider.ResetCollider();
        NextStateMl = new Idle(EnemyVarsMl);
        Stage = EVENT.Exit;
    }
    
    private void BackToPatrol()
    {
        EnemyVarsMl.enemyRef.transform.Rotate(Vector3.up, 180);
        EnemyVarsMl.ArcCollider.ResetCollider();
        NextStateMl = new Idle(EnemyVarsMl);
        Stage = EVENT.Exit;
    }
}