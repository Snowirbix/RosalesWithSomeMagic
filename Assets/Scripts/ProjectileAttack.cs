using UnityEngine;
using UnityEngine.Networking;

public class ProjectileAttack : Attack
{
    public ProjectileAttackData data;

    private Vector2 dir2;

    private GameObject AttackOwner;

    private SphereCollider sphereCollider;

    private Vector3 positionStart;

    private Quaternion rotationStart;

    private float castingTimeUsed;

    private bool isTriggered = false;

    public override void SetData (AttackData data)
    {
        this.data = data as ProjectileAttackData;
    }

    public override float GetCooldown ()
    {
        return data.cooldown;
    }

    public override void SetDir (Vector2 lookDir)
    {
        dir2 = lookDir.normalized;
    }

    public override void SetAttackOwner (GameObject Owner)
    {
        AttackOwner = Owner;
    }

    public override float GetAnimationAttackTime ()
    {
        return data.animationAttackTime;
    }

    public override void SetPosition (Vector3 pos)
    {
        positionStart = pos;
    }
    
    public override void SetRotation (Quaternion rot)
    {
        rotationStart = rot;
    }
    
    public override void Trigger ()
    {
        isTriggered = true;
        castingTimeUsed = data.castingTime;
    }

    private void Update ()
    {
        if (isTriggered)
        {
            castingTimeUsed -= Time.deltaTime;
            if (castingTimeUsed <= 0)
            {
                Fire();
                isTriggered = false;
            }
        }
    }

    private void Fire ()
    {
        GameObject instance = Instantiate(data.projectilePrefab, positionStart, rotationStart);
        ProjectileAttackBehaviour pab = instance.transform.GetComponent<ProjectileAttackBehaviour>();
        pab.SetDir(dir2);
        pab.SetRange(data.range);
        pab.SetAttackOwner(AttackOwner);
        pab.SetSpeed(data.speed);
        pab.SetDamage(data.damage);
        NetworkServer.Spawn(instance);
    }
}