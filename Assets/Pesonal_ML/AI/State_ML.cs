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

    public virtual void PlayerDies()
    {
        
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
    public Idle(GameObject npc, GameObject patrolNodes, Enemy_Eyes detector)
        : base( npc, detector)
    {
        Name = STATE.Idle;
    }
    
  
}

public class Patrol : State_ML
{
    private int currentIndex = 0;
    private bool incDec = true;
    public Patrol(GameObject npc, Enemy_Eyes detector)
        : base( npc,  detector)
    {
        Name = STATE.Patrol;
    }
    
    public override void Update()
    {
        CheckForDistance();
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
    public Pursue(GameObject npc, Enemy_Eyes detector)
        : base(npc, detector)
    {
        Name = STATE.Pursue;
    }

    public override void Update()
    {
        base.Update();
        Stage = EVENT.Update;
    }
}

public class Attack : State_ML
{
 //   private Shoot_Enemy shooter;

    public Attack(GameObject npc, Enemy_Eyes detector)
        : base(npc, detector)
    {
        Name = STATE.Attack;
    }

    public override void PlayerDies()
    {
        NextStateMl = new Patrol(Npc, Detector);
        Stage = EVENT.Exit;
    }

    public override void Update()
    {
        if (Random.Range(0, 100) > 90)
        {
            NextStateMl = new Patrol(Npc, Detector);
            Stage = EVENT.Exit;
        }
        
    }
}
