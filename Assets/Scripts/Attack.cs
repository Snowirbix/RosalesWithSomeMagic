using System;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Attack : NetworkBehaviour
{
    public AttackData data;

    public Vector2 direction;

    public Vector3 position;

    public Quaternion rotation;
    
    public abstract void Trigger();

}