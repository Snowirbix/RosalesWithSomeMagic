using UnityEngine;

public class ProjectileAttack : Attack {
    [Range(1,10)]
    public int range = 1;

    [Range(1f,20f)]
    public float speed = 5f;

    public float cooldown = 0.5f;

    public float damage = 10;

    private Vector2 dir2;

    private GameObject AttackOwner;

    private SphereCollider sphereCollider;

    void Start()
    {
        sphereCollider = transform.GetComponent<SphereCollider>();
    }
    void Update()
    {
        Vector3 dir3 = new Vector3(dir2.x,0,dir2.y);
        Move(dir3 * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider sphereCollider)
    {
        if(sphereCollider.gameObject != AttackOwner){
            //deal damage
            Health hp = sphereCollider.gameObject.transform.GetComponent<Health>();
            hp.TakeDamage(damage);
        }
    }
    void Move(Vector3 motion){
        transform.position = transform.position + motion;
    }

    public override float GetCooldown(){
        return cooldown;
    }

    public override void SetDir(Vector2 lookDir){
        dir2 = lookDir;
    }

    public override void SetAttackOwner(GameObject Owner){
        AttackOwner = Owner;
    }
}