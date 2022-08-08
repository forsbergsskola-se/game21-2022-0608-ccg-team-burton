using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeView : Node
{
    public Action<NodeView> OnNodeSelected;
    
    public BaseNode Node;

    public Port input;
    public Port output;
    
    public NodeView(BaseNode node) : base("Assets/Pesonal_ML/Graph/NodeView.uxml")
    {
        Node = node;
        title = node.name;

        viewDataKey = Node.guid;
        
        style.left = Node.position.x;
        style.top = Node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
        SetupClasses();
        
        var descriptionLabel = this.Q<Label>("description");
        descriptionLabel.bindingPath = "description";
        descriptionLabel.Bind(new SerializedObject(node));

        // var styleSheet = (StyleSheet) EditorGUIUtility.Load("NodeStyle.uss");
    }

    private void SetupClasses()
    {
        switch (Node)
        {
            case ActionNode:
                AddToClassList("action");
                break;
            case CompositeNode:
                AddToClassList("composite");
                break;
            case DecoratorNode:
                AddToClassList("decorator");
                break;
            case RootNode:
                AddToClassList("root");
                break;
        }    
    }

    private void CreateInputPorts()
    {
      
        switch (Node)
        {
            case ActionNode:
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                break;
            case CompositeNode:
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                break;
            case DecoratorNode:
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                break;
        }
        
        if (input != default)
        {
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        switch (Node)
        {
            case ActionNode:
                break;
            case CompositeNode:
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
                break;
            case DecoratorNode:
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                break;
            case RootNode:
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                break;
        }
        
        
        if (output != default)
        {
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);
        }
    }

    public sealed override string title
    {
        get { return base.title; }
        set { base.title = value; }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        
        Undo.RecordObject(Node, "Behavior tree (set position)");
        
        Node.position.x = newPos.xMin;
        Node.position.y = newPos.yMin;
        
        EditorUtility.SetDirty(Node);
    }

    public override void OnSelected()
    {
        base.OnSelected();

        OnNodeSelected?.Invoke(this);

    }

    private void SortChildren()
    {
        var composite = Node as CompositeNode;
        
    }

    public void UpdateState()
    {
        if (!Application.isPlaying) return;
        
        RemoveFromClassList("update");
        RemoveFromClassList("failure");
        RemoveFromClassList("success");
        
        switch (Node.state)
        {
            case BaseNode.State.Failure:
                AddToClassList("failure");
                break;
            case BaseNode.State.Update:
                if (Node.started)
                {
                    AddToClassList("update");
                }
                break;
            case BaseNode.State.Success:
                AddToClassList("success");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
