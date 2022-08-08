using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : BaseNode
{
    public List<BaseNode> children = new ();
    
    public override BaseNode Clone()
    {
        var node = Instantiate(this);
        node.children = children.ConvertAll(c => c.Clone());
        return node;
    }
}
