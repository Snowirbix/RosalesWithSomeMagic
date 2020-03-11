using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(State))]
public class MovementController : MonoBehaviour
{
    [ReadOnly]
    public Vector2 moveDirection = Vector2.zero;
    
    [ReadOnly]
    public Vector2 lookDirection = Vector2.zero;

    [Range(0.1f, 10f)]
    public float speed = 1f;

    public Transform rotator;

    #region components

    protected CharacterController charController;

    protected Animator animator;

    protected State state;

    #endregion components

    private void Awake ()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        state = GetComponent<State>();

        if (!rotator)
        {
            throw new UnassignedReferenceException();
        }
    }

    private void Update ()
    {
        Move();
        Rotate();
        Animate();
    }

    protected void Move ()
    {
        if (!state.root)
        {
            Vector3 dir3 = new Vector3(moveDirection.x, -0.4f, moveDirection.y); // half second to move down to the ground
            charController.Move(dir3 * speed * Time.deltaTime * state.speed);
        }
    }

    protected void Rotate ()
    {
        rotator.rotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg, Vector3.up);
    }
    
    protected void Animate ()
    {
        float x = 0f;
        float y = 0f;

        if (!state.root && !moveDirection.Equals(Vector2.zero))
        {
            float magnitude = moveDirection.magnitude;
            Vector2 moveDir = moveDirection.normalized;

            float moveAngle = Mathf.Atan2(moveDir.x, moveDir.y);
            float lookAngle = Mathf.Atan2(lookDirection.x, lookDirection.y);

            float angle = lookAngle - moveAngle;

            y = Mathf.Cos(angle) * magnitude * speed * state.speed;
            x =-Mathf.Sin(angle) * magnitude * speed * state.speed;
        }

        animator.SetFloat("x", x);
        animator.SetFloat("y", y);
    }
}
