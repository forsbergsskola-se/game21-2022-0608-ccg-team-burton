using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public abstract class State_ML 
{
    public STATE Name;
    protected EVENT Stage;
    protected State_ML NextStateMl;
    protected GameObject Npc;
    public GameObject PatrolNodes;
    protected Enemy_Eyes Detector;
    protected Animator _animator;
    protected EnemyVars_ML EnemyVarsMl;

    public enum STATE
    {
        Idle, Patrol, Pursue, Attack, Rest,
    }

    public enum EVENT
    {
        Enter, Update, Exit
    }

    public State_ML(GameObject npc, Enemy_Eyes detector, Animator animator, EnemyVars_ML enemyVarsMl)
    {
        Stage = EVENT.Enter;
        Npc = npc;
        Detector = detector;
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
    
    public Idle(GameObject npc, Enemy_Eyes detector, Animator animator, EnemyVars_ML enemyVarsMl)
        : base( npc, detector, animator, enemyVarsMl)
    {
        Name = STATE.Idle;
        Npc = npc;
    }
    
    public override void Update()
    {
        time += Time.deltaTime * Random.Range(0, 11);
        if (time > 15)
        {
            NextStateMl = new Patrol(Npc, Detector, _animator, EnemyVarsMl);
            Stage = EVENT.Exit;
        }
    }
  
}

public class Patrol : State_ML
{
    private int currentIndex = 0;
    private bool incDec = true;
    public Patrol(GameObject npc, Enemy_Eyes detector, Animator animator, EnemyVars_ML enemyVarsMl)
        : base( npc,  detector, animator, enemyVarsMl)
    {
        Name = STATE.Patrol;
        Npc = npc;
        Detector = detector;
    }

    public override void Enter()
    {
        _animator.SetBool("ExitIdleState", true);
        base.Enter();
    }

    public override void Update()
    {
        SimpleMove();

        if (Detector.PlayerSeen)
        {
            Stage = EVENT.Exit;
            NextStateMl = new Pursue(Npc, Detector, _animator, EnemyVarsMl);
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
    public Pursue(GameObject npc, Enemy_Eyes detector, Animator animator, EnemyVars_ML enemyVarsMl)
        : base(npc, detector, animator, enemyVarsMl)
    {
        Name = STATE.Pursue;
        Detector = detector;
    }

    public override void Update()
    {
        base.Update();
        Stage = EVENT.Update;

        if (Vector3.Distance(Detector.PlayerTrans.position, Npc.gameObject.transform.position) < EnemyVarsMl.GetAttackDistance)
        {
            NextStateMl = new Attack(Npc, Detector, _animator, EnemyVarsMl);    
        }
    }
}

public class Attack : State_ML
{
    private float attackDelay;
    public Attack(GameObject npc, Enemy_Eyes detector, Animator animator, EnemyVars_ML enemyVarsMl)
        : base(npc, detector, animator, enemyVarsMl)
    {
        Name = STATE.Attack;
    }


    public override void Enter()
    {
        base.Enter();
        _animator.SetBool("EnterCombatMode", true);
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
