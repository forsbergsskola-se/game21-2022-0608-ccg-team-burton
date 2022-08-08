using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionNodeFunction
{
    Travel,
    Interact
}

public abstract class ActionNode : BaseNode
{
    public ActionNodeFunction stateType;
}
