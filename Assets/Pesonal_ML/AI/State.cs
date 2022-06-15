using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public abstract class State 
{
    public STATE Name;
    protected EVENT Stage;
    protected State NextState;
    protected GameObject Npc;
    public Transform Player;
    public GameObject PatrolNodes;
    protected PlayerDetector Detector;

    public enum STATE
    {
        Idle, Patrol, Pursue, Attack, Rest,
    }

    public enum EVENT
    {
        Enter, Update, Exit
    }

    public State( Transform player, GameObject npc, GameObject patrolNodes, PlayerDetector detector)
    {
        Player = player;
        Stage = EVENT.Enter;
        Npc = npc;
        Detector = detector;
        PatrolNodes = patrolNodes;
    }

    public virtual void Enter()
    {
    //    HealthUIHandler.OnPlayerDies += PlayerDies;
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
    //    HealthUIHandler.OnPlayerDies -= PlayerDies;
        Stage = EVENT.Exit;
    }
    
    public State Process()
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
                return NextState;
        }
        return this;
    }
}


public class Idle : State
{
    public Idle( Transform player, GameObject npc, GameObject patrolNodes, PlayerDetector detector)
        : base(player, npc,  patrolNodes, detector)
    {
        Name = STATE.Idle;
    }
    
    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();
       
    }
}

public class Patrol : State
{
    private int currentIndex = 0;
//    private WaypointManager nodes;
    private List<SphereCollider> nodes2;
    private bool incDec = true;
    public Patrol( Transform player, GameObject npc, GameObject patrolNodes, PlayerDetector detector)
        : base(player, npc, patrolNodes, detector)
    {
        Name = STATE.Patrol;
        nodes2 = patrolNodes.GetComponentsInChildren<SphereCollider>().ToList();
    }
    
    public override void Update()
    {
        if (Detector.PlayerSpotted)
        {
            NextState = new Attack(Player, Npc, PatrolNodes,Detector);
            Stage = EVENT.Exit;
        }
        else
        {
            CheckForDistance(); 
        }
    }

    private void RotateSimple()
    {
        Npc.transform.Rotate(Vector3.up, 180);
    }
    
    private void RotateEnemy()
    {
        if (!Detector.PlayerSpotted)
        {
            var dot = Vector3.Dot(Npc.transform.forward, nodes2[currentIndex].transform.forward);
            if (dot < 0)
            {
                Npc.transform.Rotate(Vector3.up, 180);
            }
        }
    }
    
    private void CheckForDistance()
    {
        if (Vector3.Distance(Npc.transform.position, nodes2[currentIndex].transform.position) < 1f)
        {
            if (incDec)
            {
                if (currentIndex >= nodes2.Count - 1)
                {
                    currentIndex--;
                    incDec = false;
                    RotateSimple();
                }
                else
                {
                    currentIndex++;
                }
            }
            else
            {
                if (currentIndex <= 0)
                {
                    currentIndex++;
                    incDec = true;
                    RotateSimple();
                }
                else
                {
                    currentIndex--;
                }
            }
        }

        else
        {
            Npc.transform.position = Vector3.MoveTowards(Npc.transform.position, nodes2[currentIndex].transform.position, 1 * Time.deltaTime);
        }
    }
}

public class Pursue : State
{
    public Pursue( Transform player, GameObject npc, GameObject patrolNodes, PlayerDetector detector)
        : base(player, npc,  patrolNodes, detector)
    {
        Name = STATE.Pursue;
    }

    public override void Update()
    {
        base.Update();
        Stage = EVENT.Update;
    }
}

public class Attack : State
{
 //   private Shoot_Enemy shooter;

    public Attack( Transform player, GameObject npc, GameObject patrolNodes, PlayerDetector detector)
        : base(player, npc,patrolNodes, detector)
    {
        Name = STATE.Attack;
      //  shooter = Npc.GetComponentInChildren<Shoot_Enemy>();
    }

    public override void PlayerDies()
    {
        NextState = new Patrol(Player, Npc, PatrolNodes, Detector);
        Stage = EVENT.Exit;
    }

    public override void Update()
    {
        if (Detector.PlayerSpotted)
        {
         //   shooter.Attack();
        }

        if (!Detector.PlayerSpotted)
        {
            if (Random.Range(0, 100) > 90)
            {
                NextState = new Patrol(Player, Npc, PatrolNodes, Detector);
                Stage = EVENT.Exit;
            }
        }
    }
}
