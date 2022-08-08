using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : BaseNode
{
    public BaseNode child;
    
    public override void OnStart()
    {
       
    }

    public override void OnExit()
    {
        
    }

    public override State OnUpdate()
    {
        return child.Update();
    }

    
    public override BaseNode Clone()
    {
       var node = Instantiate(this);
       node.child = child.Clone();
       return node;
    }
}
