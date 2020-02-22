using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DisableNonLocal : NetworkBehaviour
{
    public List<Component> componentsToDisable;

    private void Start()
    {
        if (isLocalPlayer)
            return;
        
        foreach (Component comp in componentsToDisable)
        {
            if (comp is Behaviour)
            {
                (comp as Behaviour).enabled = false;
            }
        }
    }
}
