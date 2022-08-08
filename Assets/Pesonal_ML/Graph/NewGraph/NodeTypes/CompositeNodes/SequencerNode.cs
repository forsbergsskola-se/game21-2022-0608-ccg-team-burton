using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerNode : CompositeNode
{
    private int currentChild;
    public override void OnStart()
    {
        currentChild = 0;
    }

    public override void OnExit()
    {
       
    }

    public override State OnUpdate()
    {
        var child = children[currentChild];

        switch (child.Update())
        {
            case State.Failure:
                return State.Failure;
            
            case State.Update:
                return State.Update;

            case State.Success:
                currentChild++;
                break;
        }

        return currentChild == children.Count ? State.Success : State.Update;
    }
}
