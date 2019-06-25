using UnityEngine;


public abstract class Attack : ScriptableObject {
    public abstract float GetCooldown();
    public abstract void SetDir(Vector2 dir);
    public abstract void SetAttackOwner(GameObject Owner);
    public abstract float GetAnimationAttackTime();
    public abstract void StartAttack();
    public abstract void SetPosition(Vector3 pos);
    public abstract bool Update();
    public abstract void Start();
}