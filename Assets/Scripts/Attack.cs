using UnityEngine;
using UnityEngine.Networking;

public abstract class Attack : NetworkBehaviour {
    public abstract void SetData(AttackData data);
    public abstract float GetCooldown();
    public abstract void SetDir(Vector2 dir);
    public abstract void SetAttackOwner(GameObject Owner);
    public abstract float GetAnimationAttackTime();
    public abstract void Trigger();
    public abstract void SetPosition(Vector3 pos);
    public abstract void SetRotation(Quaternion rot);
}