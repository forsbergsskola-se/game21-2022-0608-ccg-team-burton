using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    public float duration = 1;
    public float startTime;
    
    public override void OnStart()
    {
        startTime = Time.time;
    }

    public override void OnExit()
    {
    }

    public override State OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }

        return State.Update;
    }
}
