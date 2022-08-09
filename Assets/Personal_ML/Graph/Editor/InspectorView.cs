using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits>
    {
        
    }

    private Editor _editor;
    
    public void UpdateSelection(NodeView nodeView)
    {
       Clear();
       
       Object.DestroyImmediate(_editor);
       _editor = Editor.CreateEditor(nodeView.Node);

       var container = new IMGUIContainer(() =>
       {
           if (_editor.target)
           {
               _editor.OnInspectorGUI();
           }
       });
       Add(container);
    }
}
