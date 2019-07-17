using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAttackData", menuName = "ScriptableObjects/ProjectileAttackData", order = 1)]
public class ProjectileAttackData : AttackData
{
    public override Type GetType ()
    {
        return typeof(ProjectileAttack);
    }

    [Range(1f, 15f)]
    public float range = 10f;

    [Range(5f, 20f)]
    public float speed = 12f;

    public int damage = 10;

    public float castingTime = 0.20f;

    public GameObject castSpellFX;

    public GameObject castSpellHandFX;

    public GameObject projectilePrefab;

    public GameObject fireSpellFX;

    public GameObject spellHitFx;
}
