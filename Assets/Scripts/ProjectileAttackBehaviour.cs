using UnityEngine;
using UnityEngine.Networking;

public class ProjectileAttackBehaviour : NetworkBehaviour
{
    [SyncVar]
    protected float range;

    [SyncVar]
    protected float speed;

    [SyncVar]
    protected Vector2 dir2;

    [SyncVar]
    protected GameObject AttackOwner;

    [SyncVar]
    protected int damage;

    protected SphereCollider sphereCollider;

    protected Vector3 positionStart;
    
    public AnimationCurve scale = AnimationCurve.Constant(0, 1, 1);

    private void Start()
    {
        sphereCollider = transform.GetComponent<SphereCollider>();
        positionStart = transform.position;
    }

    void Update()
    {
        Vector3 dir3 = new Vector3(dir2.x,0,dir2.y);
        Move(dir3 * speed * Time.deltaTime);
    }

    void Move(Vector3 motion)
    {
        if (isServer)
        {
            transform.position = transform.position + motion;
        }

        Vector3 offset = positionStart - transform.position;

        float x = offset.magnitude / range;

        transform.localScale = Vector3.one * scale.Evaluate(x);

        if (isServer)
        {
            if (x >= 1.0f)
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider sphereCollider)
    {
        if (isServer)
        {
            if (sphereCollider.gameObject != AttackOwner)
            {
                //deal damage
                Health hp = sphereCollider.gameObject.transform.GetComponent<Health>();
                hp.TakeDamage(damage);
            
                NetworkServer.Destroy(gameObject);
            }
        }
    }

     public void SetDir (Vector2 lookDir)
     {
        dir2 = lookDir.normalized;
    }

    public void SetAttackOwner (GameObject Owner)
    {
        AttackOwner = Owner;
    }

    public void SetRange (float range)
    {
        this.range = range;
    }

    public void SetSpeed (float speed)
    {
        this.speed = speed;
    }

    public void SetDamage (int damage)
    {
        this.damage = damage;
    }
}
