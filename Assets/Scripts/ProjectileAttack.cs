using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ProjectileAttack : Attack
{
    //[ReadOnly]
    public new ProjectileAttackData data
    {
        get
        {
            return (ProjectileAttackData)base.data;
        }
        set
        {
            base.data = value;
        }
    }

    protected float castingTimeUsed;

    protected bool isTriggered = false;

    protected int currentId;
    
    protected Dictionary<int, GameObject> instances = new Dictionary<int, GameObject>();


    #region components
    protected Animator animator;

    protected SphereCollider sphereCollider;

    protected Skeleton skeleton;

    #endregion components

    private void Start ()
    {
        animator = GetComponent<Animator>();
        skeleton = GetComponent<Skeleton>();
    }

    // update cast timer
    private void Update ()
    {
        if (isTriggered)
        {
            castingTimeUsed -= Time.deltaTime;

            if (castingTimeUsed <= 0)
            {
                // set is triggered to false BEFORE Fire
                // so when Fire throws an error, it only executes once
                isTriggered = false;
                Fire();
            }
        }
    }

    // start casting spell
    public override void Trigger ()
    {
        // drop mutiple cast
        if (isServer && !isTriggered)
        {
            // start casting on server
            currentId++;
            castingTimeUsed = data.castingTime;
            isTriggered = true;

            // set variables
            position = skeleton.spellSpawnPoint.position;
            rotation = skeleton.spellSpawnPoint.rotation;
                // not that easyy
                //rotation = Quaternion.LookRotation(direction, Vector3.up);

            // trigger all clients
            RpcTrigger(position, rotation, direction, currentId);
        }
    }

    [ClientRpc]
    protected void RpcTrigger (Vector3 position, Quaternion rotation, Vector2 direction, int id)
    {
        // start casting on client
        currentId = id;
        castingTimeUsed = data.castingTime;
        isTriggered = true;

        // set variables
        this.direction = direction;
        this.position = position;
        this.rotation = rotation;

        // trigger animation
        animator.SetTrigger("fireball");

        // VFX
        Instantiate(data.castSpellFX, position, rotation);
        Instantiate(data.castSpellHandFX, skeleton.rightHand.position, skeleton.rightHand.rotation, skeleton.rightHand);
    }

    // called after casting time
    protected void Fire ()
    {
        // VFX
        Instantiate(data.fireSpellFX, position, rotation);

        // Projectile
        instances.Add(currentId, Instantiate(data.projectilePrefab, position, rotation));

        ProjectileAttackBehaviour pab = instances[currentId].transform.GetComponent<ProjectileAttackBehaviour>();
            pab.id = currentId;
            pab.range = data.range;
            pab.SetOwner(this);

        instances[currentId].GetComponent<Rigidbody>().velocity = new Vector3(direction.x, 0, direction.y) * data.speed;
    }

    public void Hit (int id, GameObject target)
    {
        Health hp = target.GetComponent<Health>();
            hp.TakeDamage(data.damage);
        
        RpcHit(id, target);
    }

    [ClientRpc(channel = Channels.DefaultUnreliable)]
    protected void RpcHit (int id, GameObject target)
    {
        // VFX
        Skeleton targetSkel = target.GetComponent<Skeleton>();
        Instantiate(data.spellHitFx, targetSkel.damageSpawnPoint.position, targetSkel.damageSpawnPoint.rotation, targetSkel.damageSpawnPoint.transform);

        if (instances[id] != null)
        {
            Destroy(instances[id]);
        }
    }
}