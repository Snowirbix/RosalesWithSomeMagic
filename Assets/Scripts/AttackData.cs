using System;
using UnityEngine;

public abstract class AttackData : ScriptableObject
{
    public abstract new Type GetType();
    
    public float cooldown;
    
    public float animationAttackTime;
}
