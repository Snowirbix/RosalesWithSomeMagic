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


    // Start is called before the first frame update
    private void Start()
    {
        sphereCollider = transform.GetComponent<SphereCollider>();
        positionStart = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir3 = new Vector3(dir2.x,0,dir2.y);
        Move(dir3 * speed * Time.deltaTime);
    }

    void Move(Vector3 motion)
    {
        transform.position = transform.position + motion;
        
        Vector3 offset = positionStart - transform.position;
        if (offset.sqrMagnitude > range * range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider sphereCollider)
    {
        if (sphereCollider.gameObject != AttackOwner)
        {
            //deal damage
            Health hp = sphereCollider.gameObject.transform.GetComponent<Health>();
            hp.TakeDamage(damage);
           
            Destroy(gameObject);
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
