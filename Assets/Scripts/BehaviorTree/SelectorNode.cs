using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : ConnectorNode
{
    public override Node GetActiveNode()
    {
        foreach (Node node in childNodes)
        {
            if (node.TestDecorator())
                return node;
        }
        return null;
    }
}
