using UnityEngine;
using UnityEngine.Networking;

public class ProjectileAttack : Attack
{
    [ReadOnly]
    public ProjectileAttackData data;

    protected Vector2 dir2;

    protected GameObject AttackOwner;

    protected SphereCollider sphereCollider;

    protected Vector3 positionStart;

    protected Quaternion rotationStart;

    protected float castingTimeUsed;

    protected bool isTriggered = false;

    protected Animator animator;

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

    private void Start ()
    {
        animator = GetComponent<Animator>();
    }

    private void Update ()
    {
        if (isServer && isTriggered)
        {
            castingTimeUsed -= Time.deltaTime;
            if (castingTimeUsed <= 0)
            {
                Fire();
                isTriggered = false;
            }
        }
    }

    // start casting spell
    public override void Trigger ()
    {
        if (isServer)
        {
            isTriggered = true;
            castingTimeUsed = data.castingTime;
            RpcCastSpell(positionStart, rotationStart);
        }
    }

    [ClientRpc]
    protected void RpcCastSpell (Vector3 pos, Quaternion rot)
    {
        animator.SetTrigger("fireball");
        Instantiate(data.castSpellPrefab, pos, rot);
    }

    // called after casting time
    protected void Fire ()
    {
        if (isServer)
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
}