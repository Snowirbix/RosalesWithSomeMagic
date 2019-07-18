using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class AreaAttack : Attack
{
    public new AreaAttackData data
    {
        get
        {
            return (AreaAttackData)base.data;
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
            this.position = clickPoint;
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
        // Area FX
        instances.Add(currentId, Instantiate(data.fireSpellFX, position, rotation));

        AreaAttackBehaviour aab = instances[currentId].GetComponent<AreaAttackBehaviour>();
            aab.id = currentId;
            aab.SetOwner(this);
    }

}
