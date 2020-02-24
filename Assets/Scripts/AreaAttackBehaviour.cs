using System;
using UnityEngine;
using Mirror;

public class AreaAttackBehaviour : MonoBehaviour
{
    public int id;

    protected GameObject attackOwner;

    protected AreaAttack areaAttack;

    
    public void SetOwner (AreaAttack areaAttack)
    {
        this.areaAttack = areaAttack;
        this.attackOwner = areaAttack.gameObject;
    }
}
