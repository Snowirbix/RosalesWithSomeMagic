using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class VisualNode : VisualElement
{
    public new class UxmlFactory : UxmlFactory<VisualNode, UxmlTraits> { }

    // Unity factory soup
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        //UxmlStringAttributeDescription  m_String =  new UxmlStringAttributeDescription  { name = "string-attr", defaultValue = "default_value" };
 
        // Unity wtf soup
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }
 
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var ate = ve as VisualNode;
            ate.Clear();

            //ate.stringAttr = m_String.GetValueFromBag(bag, cc);
            //ate.Add(new TextField("String") { value = ate.stringAttr });
        }
    }

    //public string   stringAttr  { get; set; }

    [Serializable]
    public struct Data
    {
        public int id;
        public int parentId;
        public string  name;
        public Vector2 position;
    }

    protected Data data;
    protected VisualElement bot;
    protected NodeLinkImage link;
    protected bool firstDraw = true;

    public VisualNode () : base()
    {
        data = new Data() {
            id = -1,
            parentId = -1,
            name = "root",
            position = new Vector2(50, 50)
        };

        this.style.left = new StyleLength(new Length(50f, LengthUnit.Pixel));
        this.style.top  = new StyleLength(new Length(50f, LengthUnit.Pixel));

        Initialize();
    }
    
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
        AddToClassList("BT_Node");

        RegisterCallback<MouseDownEvent> (OnMouseDown);
        RegisterCallback<MouseMoveEvent> (OnMouseMove);
        RegisterCallback<MouseUpEvent>   (OnMouseUp);
        RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        RegisterCallback<KeyDownEvent>   (OnKeyDown);

        // Unity manipulator soup
        this.AddManipulator(new ContextualMenuManipulator(null));
        RegisterCallback<ContextualMenuPopulateEvent>(OnContextMenu);

        RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        if (firstDraw)
        {
            firstDraw = false;
            // Unity Query doesn't work here
            bot = Children().First(x => x.name == "BottomLink");
            bot.RegisterCallback<MouseDownEvent>(OnMouseDownBottom);
            bot.RegisterCallback<MouseMoveEvent>(OnMouseMoveBottom);
            bot.RegisterCallback<MouseUpEvent>  (OnMouseUpBottom);

            link = bot.Children().First(x => x.name == "Link") as NodeLinkImage;
            link.visible = false;
        }
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        switch ((MouseButton)evt.button)
        {
            case MouseButton.LeftMouse:
                this.CaptureMouse();
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
                this.ReleaseMouse();
            break;
        }
    }

    private void OnMouseLeave(MouseLeaveEvent evt)
    {
        if (this.HasMouseCapture())
            this.ReleaseMouse();
    }

    private void OnContextMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Delete", (DropdownMenuAction action) =>
        {
            RemoveFromHierarchy();
        });
    }

    private void OnKeyDown (KeyDownEvent evt)
    {
        if (evt.keyCode == KeyCode.Escape)
        {
            MouseCaptureController.ReleaseMouse();
            Debug.Log("Force Release Mouse");
        }
    }

    private void OnMouseDownBottom(MouseDownEvent evt)
    {
        switch ((MouseButton)evt.button)
        {
            case MouseButton.LeftMouse:
                evt.StopImmediatePropagation();
                link.style.left = evt.localMousePosition.x;
                link.style.top = evt.localMousePosition.y;
                link.visible = true;
                bot.CaptureMouse();
            break;
        }
    }
    
    private void OnMouseMoveBottom(MouseMoveEvent evt)
    {
        switch ((MouseButton)evt.button)
        {
            case MouseButton.LeftMouse:
                //evt.StopImmediatePropagation();
                if (bot.HasMouseCapture())
                {
                    link.style.width  = link.style.width. value.value + evt.mouseDelta.x;
                    link.style.height = link.style.height.value.value + evt.mouseDelta.y;
                }
            break;
        }
    }
    
    private void OnMouseUpBottom(MouseUpEvent evt)
    {
        switch ((MouseButton)evt.button)
        {
            case MouseButton.LeftMouse:
                bot.ReleaseMouse();
            break;
        }
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
