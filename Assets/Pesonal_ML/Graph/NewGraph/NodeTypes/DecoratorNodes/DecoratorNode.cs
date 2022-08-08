using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : BaseNode
{
    public BaseNode child;
    
    public override BaseNode Clone()
    {
        var node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
