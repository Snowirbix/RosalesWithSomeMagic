using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : ConnectorNode
{
    protected int idx = 0;

    public override Node GetActiveNode()
    {
        return childNodes[idx];
    }
}
