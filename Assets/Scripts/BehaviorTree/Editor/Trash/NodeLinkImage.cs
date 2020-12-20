using System.Collections;
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
}
