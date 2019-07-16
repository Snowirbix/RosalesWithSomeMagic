using UnityEngine;
using UnityEngine.Networking;
using System;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileAttackBehaviour : NetworkBehaviour
{
    protected float range;

    protected GameObject AttackOwner;

    protected int damage;

    protected SphereCollider sphereCollider;

    protected Vector3 positionStart;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        positionStart = transform.position;
    }

    private void Update()
    {
        Vector3 offset = positionStart - transform.position;

        float x = offset.magnitude / range;

        if (x >= 1.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter (Collider coll)
    {
        // if we are on the server
        if (NetworkServer.active)
        {
            if (coll.gameObject != AttackOwner)
            {
                //deal damage
                Health hp = coll.GetComponent<Health>();
                hp.TakeDamage(damage);
            }
        }
        
        Destroy(gameObject);
    }

    public void SetAttackOwner (GameObject Owner)
    {
        AttackOwner = Owner;
    }

    public void SetRange (float range)
    {
        this.range = range;
    }

    public void SetDamage (int damage)
    {
        this.damage = damage;
    }
}
