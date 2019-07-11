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

    public float cooldown = 1f;

    public int damage = 10;

    public float animationAttackTime = 0.30f;

    public float castingTime = 0.20f;

    public GameObject projectilePrefab;
}
