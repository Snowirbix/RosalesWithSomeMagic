using UnityEngine;

public class ProjectileAttack : Attack {
    [Range(1,10)]
    public int range = 5;

    [Range(1f,20f)]
    public float speed = 5f;

    public float cooldown = 0.5f;

    public float damage = 10;

    private Vector2 dir2;

    private GameObject AttackOwner;

    private SphereCollider sphereCollider;

    private Vector3 positionStart;

    void Start()
    {
        sphereCollider = transform.GetComponent<SphereCollider>();
        positionStart = transform.position;
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
           
            Destroy(gameObject);
        }
    }
    void Move(Vector3 motion){
        transform.position = transform.position + motion;
        
        Vector3 offset = positionStart - transform.position;
        if(offset.sqrMagnitude > range * range){
            Destroy(gameObject);
        }
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