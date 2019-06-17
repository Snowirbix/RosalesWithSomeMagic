using UnityEngine;

public abstract class Attack : MonoBehaviour {
    public abstract float GetCooldown();
    public abstract void SetDir(Vector2 dir);
    public abstract void SetAttackOwner(GameObject Owner);

    public abstract float GetAnimationAttackTime();
}