using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : NetworkBehaviour
{
    [SyncVar][ReadOnly]
    public Vector2 moveDirection = Vector2.zero;
    
    [SyncVar][ReadOnly]
    public Vector2 lookDirection = Vector2.zero;
    [Range(0.1f, 10f)]
    public float speed = 1f;

    public Transform rotator;

    [ReadOnly]
    public bool castingSpell = false;

    private float animationAttackTime = 0;
    protected CharacterController charController;
    protected Animator animator;

    private void Start ()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (!rotator)
        {
            throw new UnassignedReferenceException();
        }
    }

    private void Update ()
    {
        if (!castingSpell)
        {
            if (isLocalPlayer)
            {
                Move();
                Rotate();
            }
        }
        else
        {
            animationAttackTime -= Time.deltaTime;

            if(animationAttackTime <= 0)
            {
                animationAttackTime = 0;
                castingSpell = false;
            }
        }

        Animate();
    }

    protected void Move ()
    {
        Vector3 dir3 = new Vector3(moveDirection.x, 0, moveDirection.y);
        charController.Move(dir3 * speed * Time.deltaTime);
    }

    protected void Rotate ()
    {
        rotator.rotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg, Vector3.up);
    }

    protected void Animate ()
    {
        float x = 0f;
        float y = 0f;

        if (!moveDirection.Equals(Vector2.zero))
        {
            float magnitude = moveDirection.magnitude;
            Vector2 moveDir = moveDirection.normalized;

            float moveAngle = Mathf.Atan2(moveDir.x, moveDir.y);
            float lookAngle = Mathf.Atan2(lookDirection.x, lookDirection.y);

            float angle = lookAngle - moveAngle;

            Vector3 dir3 = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.up) * Vector3.right * magnitude;

            x = dir3.z;
            y = dir3.x;
        }

        animator.SetFloat("x", x);
        animator.SetFloat("y", y);
    }

    public void SetAnimationAttackTime (float attackTime)
    {
        castingSpell = true;
        animationAttackTime = attackTime;
    }

    public void SetMoveDirection (Vector3 direction)
    {
        this.moveDirection = direction;
        this.CmdSetMoveDirection(direction);
    }

    public void SetLookDirection (Vector3 direction)
    {
        this.lookDirection = direction;
        this.CmdSetLookDirection(direction);
    }

    [Command]
    public void CmdSetMoveDirection (Vector3 direction)
    {
        this.moveDirection = direction;
    }

    [Command]
    public void CmdSetLookDirection (Vector3 direction)
    {
        this.lookDirection = direction;
    }
}
