using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerExtension
{
    public static int ToLayerMask (this int i)
    {
        return 1 << i;
    }
}
