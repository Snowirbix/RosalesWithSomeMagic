using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackBehaviour : MonoBehaviour
{
    private float range;
    private float speed;
    private Vector2 dir2;
    private SphereCollider sphereCollider;
    private Vector3 positionStart;

    private GameObject AttackOwner;

    private int damage;

    // Start is called before the first frame update
    void Start()
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

    void Move(Vector3 motion){
        transform.position = transform.position + motion;
        
        Vector3 offset = positionStart - transform.position;
        if(offset.sqrMagnitude > range * range){
            Destroy(gameObject);
        }
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

     public void SetDir(Vector2 lookDir){
        dir2 = lookDir.normalized;
    }

    public void SetAttackOwner(GameObject Owner){
        AttackOwner = Owner;
    }

    public void SetRange(float theRange){
        range = theRange;
    }

    public void SetSpeed(float newSpeed){
        speed = newSpeed;
    }

    public void SetDamage(int newDamage){
        damage = newDamage;
    }
}
