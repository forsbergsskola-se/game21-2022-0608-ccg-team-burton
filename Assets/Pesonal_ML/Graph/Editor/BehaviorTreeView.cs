using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class BehaviorTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    
    private BehaviourTree _tree;
    
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits>
    {
        
    }

    public BehaviorTreeView()
    {
        Insert(0, new GridBackground());
        
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Pesonal_ML/Graph/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PopulateView(_tree);
        AssetDatabase.SaveAssets();
    }

    private NodeView FindNodeView(BaseNode node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView(BehaviourTree tree)
    {
        _tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if (!_tree.rootNode)
        {
            _tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(_tree);
            AssetDatabase.SaveAssets();
        }
        
        _tree.nodes.ForEach(CreateNodeView);
        
        _tree.nodes.ForEach(n =>
        {
            var children = _tree.GetChildren(n);
            
            children.ForEach(c =>
            {
                var parentView = FindNodeView(n);
                var childView = FindNodeView(c);

               var edge = parentView.output.ConnectTo(childView.input);
               
               AddElement(edge); 
            });
        });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.Where(p => p.direction != startPort.direction 
                                && p.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        graphViewChange.elementsToRemove?.ForEach(x =>
        {
            if (x is NodeView nodeView)
            {
                _tree.DeleteNode(nodeView.Node);
            }

            if (x is Edge edge)
            {
                var parentView = edge.output.node as NodeView;
                var childView = edge.input.node as NodeView;
                _tree.RemoveChild(parentView.Node, childView.Node);
            }
        });
        
        graphViewChange.edgesToCreate?.ForEach(e =>
        {
            var parentView = e.output.node as NodeView;
            var childView = e.input.node as NodeView;
            _tree.AddChild(parentView.Node, childView.Node);
        });
        
        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var t in types)
            {
                evt.menu.AppendAction($"[{t.BaseType.Name}] {t.Name}", a => CreateNode(t));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var t in types)
            {
                evt.menu.AppendAction($"[{t.BaseType.Name}] {t.Name}", a => CreateNode(t));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var t in types)
            {
                evt.menu.AppendAction($"[{t.BaseType.Name}] {t.Name}", a => CreateNode(t));
            }
        }

    }

    private void CreateNode(System.Type type)
    {
        var node = _tree.CreateNode(type);
        CreateNodeView(node);
    }

    private void CreateNodeView(BaseNode node)
    {
        var nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;

        AddElement(nodeView);
    }


    public void UpdateNodeStates()
    {
        nodes.ForEach(n =>
        {
            var view = n as NodeView;
            view.UpdateState();
        });
    }
}
