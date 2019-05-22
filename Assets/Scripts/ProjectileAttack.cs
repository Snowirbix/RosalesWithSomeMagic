using UnityEngine;

public class ProjectileAttack : Attack {
    [Range(1,10)]
    public int range = 1;

    [Range(1f,20f)]
    public float speed = 5f;

    public float cooldown = 0.5f;

    private Vector2 dir2;
    void Update()
    {
        Vector3 dir3 = new Vector3(dir2.x,0,dir2.y);
        Move(dir3 * speed * Time.deltaTime);
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
}