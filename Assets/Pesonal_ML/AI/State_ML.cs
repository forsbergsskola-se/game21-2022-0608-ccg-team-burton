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
    private bool _wallSpotted;
    private Bounds _bounds;
    
    
    public Patrol(EnemyVars_ML enemyVarsMl)
        : base(enemyVarsMl)
    {
        Debug.Log("Patrol state");
        Name = STATE.Patrol;
    }

    public override void Enter()
    {
        base.Enter();
       _bounds = EnemyVarsMl.enemyRef.gameObject.GetComponent<BoxCollider2D>().bounds;
     //  Debug.Log($"Center of bounds is: {_bounds.center}");
     //  Debug.Log($"Max of bounds is: {_bounds.max}");

    }

    public override void Update()
    {
        
        if (EnemyVarsMl.tracerEyes.PlayerSeen)
        {
            Stage = EVENT.Exit;
           
            NextStateMl = new Pursue(EnemyVarsMl);
        }

        else if (!EnemyVarsMl.tracerEyes.GroundSeen)
        {
            Stage = EVENT.Exit;
            Debug.Log("Ground not seen");
           
            NextStateMl = new Jump(EnemyVarsMl);
        }
        
        
        else if (EnemyVarsMl.tracerEyes.WallSeen)
        {
            _wallSpotted = true;
            TurnAround();
        }

        if (_wallSpotted)
        {
            
        }
        
        
        if (delay < 0)
        {
            SimpleMove();
        }

        delay -= Time.deltaTime * 2f;
    }
    
    private void TurnAround()
    {
        Stage = EVENT.Exit;
        NextStateMl = new Idle(EnemyVarsMl);
        EnemyVarsMl.enemyRef.transform.Rotate(Vector3.up, 180);
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
        var distance = Vector3.Distance(EnemyVarsMl.tracerEyes.PlayerTrans.position, EnemyVarsMl.enemyRef.gameObject.transform.position);
        if (EnemyVarsMl.tracerEyes.PlayerInAttackRange)
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
        if (!EnemyVarsMl.tracerEyes.PlayerSeen)
        {
            var dotProd = Vector3.Dot(EnemyVarsMl.enemyRef.transform.right, EnemyVarsMl.tracerEyes.PlayerTrans.position);
            
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
        
        if (attackDelay >= EnemyVarsMl.GetAttackInterval && EnemyVarsMl.tracerEyes.PlayerInAttackRange)
        { 
            EnemyVarsMl.animator.SetTrigger(Animator.StringToHash("MakeAttack"));

            if (EnemyVarsMl.GetEnemyType == EnemyType.Ranged)
            {
                AssetPool.RequestEffectStatic(EffectType.FireBall, EnemyVarsMl.firePoint.position, EnemyVarsMl.enemyRef.transform.right);
            }

            attackDelay -= EnemyVarsMl.GetAttackInterval;
        }
        
        if (!EnemyVarsMl.tracerEyes.PlayerInAttackRange)
        {
            Stage = EVENT.Exit;
            NextStateMl = new Pursue(EnemyVarsMl);
        }
        
        attackDelay +=  Time.deltaTime;
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
    private bool tileSpotted;

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
        if (!tileSpotted)
        {
            EnemyVarsMl.ArcCollider.CalculatePoints();
            tileSpotted = EnemyVarsMl.ArcCollider.TileSpotted;
        }

        if (tileSpotted)
        {
            Debug.Log("tile spotted");
            if (EnemyVarsMl.ArcCollider.NextTile == EnemyVarsMl.tracerEyes.StandingOn)
            {
                Debug.Log("same tile");
                BackToPatrol();
            }
            
            else if (EnemyVarsMl.ArcCollider.TileHeightDifference > 4)
            {
                Debug.Log("too high");
                BackToPatrol();
            }
            
            else
            {
                jumpDelay -= Time.deltaTime * 2f;
                MakeJump();    
            }
        }
        
        else if (EnemyVarsMl.ArcCollider.GetAngle > maxAngle)
        {
            Debug.Log("angle too high");
            BackToPatrol();
        }
    }

    public void MakeJump()
    {
        Debug.Log("Making jump");
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
        Debug.Log("Can't jump, back to patrol");
        EnemyVarsMl.enemyRef.transform.Rotate(Vector3.up, 180);
        EnemyVarsMl.ArcCollider.ResetCollider();
        NextStateMl = new Idle(EnemyVarsMl);
        Stage = EVENT.Exit;
    }
}