using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeElement : Label
{
    public NodeElement() : base()
    {
        this.text = "Node";
        this.AddToClassList("BT_Node");
        this.RegisterCallback<MouseDownEvent>(OnMouseDown);
        this.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        this.RegisterCallback<MouseUpEvent>(OnMouseUp);
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
