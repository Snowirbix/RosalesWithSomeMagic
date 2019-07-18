using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaAttackData", menuName = "ScriptableObjects/AreaAttackData", order = 2)]
public class AreaAttackData : AttackData
{
    public override Type GetType ()
    {
        return typeof(AreaAttack);
    }
    
    [Range(1f, 15f)]
    public float range = 10f;

    public int damage = 10;

    public float castingTime = 0.20f;

    public GameObject castSpellFX;

    public GameObject castSpellHandFX;

    public GameObject fireSpellFX;

    public GameObject spellHitFx;
}
