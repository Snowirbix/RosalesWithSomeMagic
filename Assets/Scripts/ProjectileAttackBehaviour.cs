using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileAttackBehaviour : NetworkBehaviour
{
    public int id;

    public float range;

    public LayerMask enemiesLayerMask;

    protected Vector3 positionStart;

    protected GameObject attackOwner;

    protected ProjectileAttack projectileAttack;

    private void Start()
    {
        positionStart = transform.position;
    }

    private void Update()
    {
        Vector3 offset = positionStart - transform.position;

        // destroy when range is reached
        if (offset.sqrMagnitude >= range * range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter (Collider collider)
    {
        // if we are on the server
        if (NetworkServer.active)
        {
            if (!collider.isTrigger)
            {
                projectileAttack.Hit(this.id, collider.gameObject);
            }
        }
    }

    public void SetOwner (ProjectileAttack projectileAttack)
    {
        this.projectileAttack = projectileAttack;
        this.attackOwner = projectileAttack.gameObject;
    }
}
