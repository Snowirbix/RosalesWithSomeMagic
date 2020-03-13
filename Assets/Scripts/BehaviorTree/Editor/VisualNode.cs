using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VisualNode : Label
{
    [Serializable]
    public struct Data
    {
        public int id;
        public int parentId;
        public string  name;
        public Vector2 position;
    }

    protected Data data;
    
    public VisualNode(int id) : base()
    {
        data = new Data() {
            id = id,
            parentId = -1,
            name = "root",
            position = new Vector2(50, 50)
        };

        this.style.left = new StyleLength(new Length(50f, LengthUnit.Pixel));
        this.style.top  = new StyleLength(new Length(50f, LengthUnit.Pixel));

        Initialize();
    }

    /**
     * Create visual node from saved data
     */
    public VisualNode(Data data)
    {
        Deserialize(data);

        Initialize();
    }

    public void Initialize ()
    {
        this.text = "root";

        this.AddToClassList("BT_Node");

        this.RegisterCallback<MouseDownEvent>(OnMouseDown);
        this.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        this.RegisterCallback<MouseUpEvent  >(OnMouseUp);

        // Unity manipulator soup
        this.AddManipulator(new ContextualMenuManipulator(null));
        this.RegisterCallback<ContextualMenuPopulateEvent>(OnContextMenu);
    }

    private void wtf(ContextualMenuManipulator evt)
    {
        return;
    }

    protected void OnMouseDown(MouseDownEvent evt)
    {
        switch ((MouseButton)evt.button)
        {
            case MouseButton.LeftMouse:
                evt.target.CaptureMouse();
            break;
        }
    }
    
    private void OnMouseMove(MouseMoveEvent evt)
    {
        switch ((MouseButton)evt.button)
        {
            case MouseButton.LeftMouse:
                if (this.HasMouseCapture())
                {
                    this.style.left = this.style.left.value.value + evt.mouseDelta.x;
                    this.style.top  = this.style.top. value.value + evt.mouseDelta.y;
                }
            break;
        }
    }
    
    private void OnMouseUp(MouseUpEvent evt)
    {
        switch ((MouseButton)evt.button)
        {
            case MouseButton.LeftMouse:
                if (this.HasMouseCapture())
                {
                    this.ReleaseMouse();
                }
            break;
        }
    }

    private void OnContextMenu(ContextualMenuPopulateEvent evt)
    {
        //evt.menu.AppendAction("Link")
        evt.menu.AppendAction("Delete", (DropdownMenuAction action) =>
        {
            parent.Remove(this);
        });
    }

    public Data Serialize()
    {
        data.position = new Vector2 {
            x = this.style.left.value.value,
            y = this.style.top. value.value
        };

        return data;
    }

    public void Deserialize(Data data)
    {
        // overwrite data
        this.data = data;

        // apply styles
        this.style.left = new StyleLength(new Length(data.position.x, LengthUnit.Pixel));
        this.style.top  = new StyleLength(new Length(data.position.y, LengthUnit.Pixel));
    }
}
