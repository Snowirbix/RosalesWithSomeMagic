using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack", order = 1)]
public class ProjectileAttack : Attack {
    [Range(1,10)]
    public int range = 5;

    [Range(1f,20f)]
    public float speed = 10f;

    public float cooldown = 0.5f;

    public int damage = 10;

    public float animationAttackTime = 0.25f;

    public float castingTime = 0.20f;

    public GameObject projectileAttack;
    
    private Vector2 dir2;

    private GameObject AttackOwner;

    private SphereCollider sphereCollider;

    private Vector3 positionStart;

    private float castingTimeUsed;
    
    public override float GetCooldown(){
        return cooldown;
    }

    public override void SetDir(Vector2 lookDir){
        dir2 = lookDir.normalized;
    }

    public override void SetAttackOwner(GameObject Owner){
        AttackOwner = Owner;
    }

    public override float GetAnimationAttackTime(){
        return animationAttackTime;
    }

    public override void SetPosition(Vector3 pos){
        positionStart = pos;
    }
    public override void StartAttack(){

        GameObject go = Instantiate(projectileAttack, positionStart, Quaternion.identity);
        ProjectileAttackBehaviour pab = go.transform.GetComponent<ProjectileAttackBehaviour>();
        pab.SetDir(dir2);
        pab.SetRange(range);
        pab.SetAttackOwner(AttackOwner);
        pab.SetSpeed(speed);
        pab.SetDamage(damage);
    }

    public override bool Update(){
        castingTimeUsed -= Time.deltaTime;
        if(castingTimeUsed > 0){
            return true;
        }else
        {
            StartAttack();
            castingTimeUsed = castingTime;
            return false;
        }
    }

    public override void Start(){
        castingTimeUsed = castingTime;
    }
}