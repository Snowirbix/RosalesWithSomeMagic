using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class ProjectileAttack : Attack
{
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
    public override void Trigger (Vector3 clickPoint, Vector2 direction, Quaternion rotation)
    {
        // drop mutiple cast
        if (isServer && !isTriggered)
        {
            // start casting on server
            currentId++;
            castingTimeUsed = data.castingTime;
            isTriggered = true;

            // set variables
            this.position = skeleton.spellSpawnPoint.position;
            this.direction = direction;
            this.rotation = rotation;

            // trigger all clients
            RpcTrigger(position, direction, rotation, currentId);
        }
    }

    [ClientRpc]
    protected void RpcTrigger (Vector3 position, Vector2 direction, Quaternion rotation, int id)
    {
        // start casting on client
        currentId = id;
        castingTimeUsed = data.castingTime;
        isTriggered = true;

        // set variables
        this.position = position;
        this.direction = direction;
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

        ProjectileAttackBehaviour pab = instances[currentId].GetComponent<ProjectileAttackBehaviour>();
            pab.id = currentId;
            pab.range = data.range;
            pab.SetOwner(this);

        instances[currentId].GetComponent<Rigidbody>().velocity = new Vector3(direction.x, 0, direction.y) * data.speed;
    }

    public void Hit (int id, GameObject target)
    {
        Health hp = target.GetComponent<Health>();
        Assert.IsNotNull(hp, $"target {target.name} has no Health component !");
        hp.TakeDamage(data.damage);
        
        RpcHit(id, target);

        //TargetHit(target.GetComponent<NetworkIdentity>().connectionToClient, target);
    }

    // executes on target machine
    // on the attacker gameObject
    [TargetRpc]
    protected void TargetHit (NetworkConnection target, GameObject goTarget)
    {
        State state = goTarget.GetComponent<State>();

        State.TempModifier modifier = new State.TempModifier(1f);

        State.ModifierUpdate update = (State.Data d) => {
            d.speed -= data.slowCurve.Evaluate(1f-modifier.time);
        };

        modifier.SetDelegate(update);

        state.AddLocalModifier(modifier);
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