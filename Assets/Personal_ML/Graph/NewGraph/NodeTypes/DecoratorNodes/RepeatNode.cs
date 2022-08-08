using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    
    public override void OnStart()
    {
        
    }

    public override void OnExit()
    {
       
    }

    public override State OnUpdate()
    {
        child.Update();
        return State.Update;
        
    }
}
