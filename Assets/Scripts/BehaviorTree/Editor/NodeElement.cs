using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeElement : Label
{
    public NodeElement(BehaviorTreeConfiguration btConfig) : base()
    {
        this.text = "root";
        this.AddToClassList("BT_Node");
        this.RegisterCallback<MouseDownEvent>(OnMouseDown);
        this.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        this.RegisterCallback<MouseUpEvent>(OnMouseUp);

        btConfig.nodes.Add(new BehaviorTreeConfiguration.NodeConfiguration(btConfig) { name = "root", position = new Vector2(10, 10), parentId = -1 });
    }

    protected void OnMouseDown(MouseDownEvent evt)
    {
        evt.target.CaptureMouse();
    }
    
    private void OnMouseMove(MouseMoveEvent evt)
    {
        if (MouseCaptureController.HasMouseCapture(this))
        {

        }
    }
    
    private void OnMouseUp(MouseUpEvent evt)
    {
        MouseCaptureController.ReleaseMouse();
    }

}
