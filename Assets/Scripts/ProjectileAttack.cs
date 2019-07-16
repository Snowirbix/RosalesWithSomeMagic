using UnityEngine;
using UnityEngine.Networking;

public class ProjectileAttack : Attack
{
    public Transform rightHand;

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
        dir2 = lookDir;
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

    // start casting spell
    public override void Trigger ()
    {
        if (isServer)
        {
            isTriggered = true;
            RpcTrigger(positionStart, rotationStart, dir2);
            castingTimeUsed = data.castingTime;
        }
    }

    [ClientRpc]
    protected void RpcTrigger (Vector3 pos, Quaternion rot, Vector2 dir)
    {
        castingTimeUsed = data.castingTime;
        isTriggered = true;
        dir2 = dir;
        positionStart = pos;
        rotationStart = rot;
        animator.SetTrigger("fireball");
        Instantiate(data.castSpellFX, pos, rot);
        Instantiate(data.castSpellHandFX, rightHand.position, rightHand.rotation, rightHand);
    }

    // called after casting time
    protected void Fire ()
    {
        Instantiate(data.fireSpellFX, positionStart, rotationStart);
        GameObject instance = Instantiate(data.projectilePrefab, positionStart, rotationStart);
        ProjectileAttackBehaviour pab = instance.transform.GetComponent<ProjectileAttackBehaviour>();
        pab.SetRange(data.range);
        pab.SetAttackOwner(AttackOwner);
        pab.SetDamage(data.damage);
        instance.GetComponent<Rigidbody>().velocity = new Vector3(dir2.x, 0, dir2.y) * data.speed;
    }
}