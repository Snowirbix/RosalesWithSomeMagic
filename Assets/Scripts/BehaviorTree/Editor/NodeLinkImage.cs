﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeLinkImage : Image
{
    public new class UxmlFactory : UxmlFactory<NodeLinkImage, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }
 
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var ate = ve as NodeLinkImage;
        }
    }

    public static VectorImage image;

    public NodeLinkImage () : base()
    {
        if (!image)
            throw new MissingReferenceException("Vector image is not set");

        this.vectorImage = image;
        this.scaleMode = ScaleMode.StretchToFill;
        this.AddToClassList("BT_Link");
    }

    protected override void ExecuteDefaultAction (EventBase evt)
    {
        base.ExecuteDefaultAction(evt);

        switch ((EventType)evt.eventTypeId)
        {
            case EventType.MouseMove:
                if (this.HasMouseCapture())
                    Debug.Log("mouse move");
            break;
            case EventType.MouseUp:
                //Debug.Log(evt.target == this);
                //this.ReleaseMouse();
                //Debug.Log("Release mouse");
            break;
        }
    }
}
