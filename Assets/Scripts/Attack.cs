using System;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Attack : NetworkBehaviour
{
    public AttackData data;

    protected Vector2 direction;

    protected Vector3 position;

    protected Quaternion rotation;
    
    public abstract void Trigger (Vector3 clickPoint, Vector2 direction, Quaternion rotation);
}