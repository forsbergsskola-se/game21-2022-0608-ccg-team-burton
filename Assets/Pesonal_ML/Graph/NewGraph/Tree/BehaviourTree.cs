using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NewGraph.NodeTypes.CompositeNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu]
public class BehaviourTree : ScriptableObject
{
    public CustomBlackboard blackboard = new();
    public BaseNode rootNode;
    
    public BaseNode.State state = BaseNode.State.Update;

    public List<BaseNode> nodes = new();

    public BaseNode.State Update()
    {
        if (state == BaseNode.State.Update)
        {
            state = rootNode.Update();
        }

        return state;
    }

    public BaseNode CreateNode(Type type)
    {
        var node = CreateInstance(type) as BaseNode;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();

        Undo.RecordObject(this, "Behaviour tree (CreateNode)");
        
        nodes.Add(node);

        if (!Application.isPlaying)
        {
            AssetDatabase.AddObjectToAsset(node, this);
        }
        
        
        Undo.RegisterCreatedObjectUndo(node, "Behaviour tree (CreateNode)");
        AssetDatabase.SaveAssets();
        
        return node;
    }

    public void DeleteNode(BaseNode node)
    {
        Undo.RecordObject(this, "Behaviour tree (DeleteNode)");
        nodes.Remove(node);
        Undo.DestroyObjectImmediate(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(BaseNode parent, BaseNode child)
    {
        var decorator = parent as DecoratorNode;
        if (decorator)
        {
            Undo.RecordObject(decorator, "Behaviour tree (AddChild)");
            decorator.child = child;
            EditorUtility.SetDirty(decorator);
        }

        var root = parent as RootNode;
        if (root)
        {
            Undo.RecordObject(root, "Behaviour tree (AddChild)");
            root.child = child;
            EditorUtility.SetDirty(root);
        }
        
        var composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour tree (AddChild)");
            composite.children.Add(child);
            EditorUtility.SetDirty(composite);
        }

    }
    public void RemoveChild(BaseNode parent, BaseNode child)
    {
        var decorator = parent as DecoratorNode;

        if (decorator)
        {
            Undo.RecordObject(decorator, "Behaviour tree (RemoveChild)");
            decorator.child = null;
            EditorUtility.SetDirty(decorator);
        }

        var root = parent as RootNode;

        if (root)
        {
            Undo.RecordObject(root, "Behaviour tree (RemoveChild)");
            root.child = null;
            EditorUtility.SetDirty(root);
        }
        
        
        var composite = parent as CompositeNode;

        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour tree (RemoveChild)");
            composite.children.Remove(child);
            EditorUtility.SetDirty(composite);
        }
    }
    
    public List<BaseNode> GetChildren(BaseNode targetNode)
    {
        var decorator = targetNode as DecoratorNode;
        var theChildren = new List<BaseNode>();

        if (decorator && decorator.child)
        {
            theChildren.Add(decorator.child);
        }

        var root = targetNode as RootNode;

        if (root && root.child)
        {
            theChildren.Add(root.child);
        }
        
        var composite = targetNode as CompositeNode;

        if (composite)
        {
            return composite.children;
        }

        return theChildren;
    }

    public void Traverse(BaseNode node, Action<BaseNode> visitor)
    {
        if (node)
        {
            visitor?.Invoke(node);
            var children = GetChildren(node);
            children.ForEach(c => Traverse(c, visitor));
        }
    }
    
    public BehaviourTree Clone()
    {
        var tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();
        tree.nodes = new List<BaseNode>();
        Traverse(tree.rootNode, n =>
        {
            tree.nodes.Add(n);
        });
        
        return tree;
    }


    public void Bind(AiAgent agent)
    {
        Traverse(rootNode, n =>
        {
            n.agent = agent;
            n.blackboard = blackboard;
        });
    }
}
