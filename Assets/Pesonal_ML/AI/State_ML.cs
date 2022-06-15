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

    public enum STATE
    {
        Idle, Patrol, Pursue, Attack, Rest,
    }

    public enum EVENT
    {
        Enter, Update, Exit
    }

    public State_ML(GameObject npc, Enemy_Eyes detector)
    {
        Stage = EVENT.Enter;
        Npc = npc;
        Detector = detector;
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
    
    public Idle(GameObject npc, Enemy_Eyes detector, Animator animator)
        : base( npc, detector)
    {
        Name = STATE.Idle;
        _animator = animator;
        Npc = npc;
    }
    
    public override void Update()
    {
        time += Random.Range(0, 11);
        if (time > 1000)
        {
            NextStateMl = new Patrol(Npc, Detector, _animator);
            Stage = EVENT.Exit;
        }
    }
  
}

public class Patrol : State_ML
{
    private int currentIndex = 0;
    private bool incDec = true;
    public Patrol(GameObject npc, Enemy_Eyes detector, Animator animator)
        : base( npc,  detector)
    {
        Name = STATE.Patrol;
        _animator = animator;
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
            NextStateMl = new Pursue(Npc, Detector, _animator);
        }
    }

    private void SimpleMove()
    {
        Npc.transform.position += Npc.transform.right * (Time.deltaTime * 0.5f);
        
    }
    
    private void RotateSimple()
    {
        Npc.transform.Rotate(Vector3.up, 180);
    }
    
    
    private void CheckForDistance()
    {
    }
}

public class Pursue : State_ML
{
    public Pursue(GameObject npc, Enemy_Eyes detector, Animator animator)
        : base(npc, detector)
    {
        Name = STATE.Pursue;
        _animator = animator;
        Detector = detector;
    }

    public override void Update()
    {
        base.Update();
        Stage = EVENT.Update;

        if (Vector3.Distance(Detector.PlayerTrans.position, Npc.gameObject.transform.position) < 2)
        {
            NextStateMl = new Attack(Npc, Detector, _animator);    
        }
    }
}

public class Attack : State_ML
{
    public Attack(GameObject npc, Enemy_Eyes detector, Animator animator)
        : base(npc, detector)
    {
        Name = STATE.Attack;
        _animator = animator;
    }
    
    public override void Update()
    {
        _animator.SetInteger("AttackOne", 0);
    }
}
