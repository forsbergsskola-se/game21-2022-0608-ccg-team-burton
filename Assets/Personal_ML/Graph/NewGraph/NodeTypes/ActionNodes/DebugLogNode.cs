using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    public string message;
    public override void OnStart()
    {
        Debug.Log($"OnStart {message}");
    }

    public override void OnExit()
    {
        Debug.Log($"OnExit {message}");
    }

    public override State OnUpdate()
    {
        Debug.Log($"OnUpdate {message}");
        return State.Success;
    }
}
