using UnityEngine;
using UnityEngine.AI;
using Mirror;
using PathCreation;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(State))]
public class Pathfinder : NetworkBehaviour
{
    public float speed = 2f;

    [SerializeField][ReadOnly]
    protected float _speedBoost;
    public float speedBoost
    {
        get
        {
            return _speedBoost;
        }
        set
        {
            _speedBoost = value;
            animator.SetFloat("speed", value * speed);
        }
    }

    protected NavMeshPath path;
    
    protected State state;

    [ReadOnly]
    public Transform target;

    protected Vector3 lastTargetPosition;

    [Tooltip("Recompute path every staleTime (in seconds)")]
    public float staleTime = 1f;

    [Tooltip("Recompute path when the target moves")]
    public float hardFollowRadius = 10f;

    protected float currentPathTime = 0f;

    public float waypointRadius = 1f;

    protected int currentWaypoint;
    
    protected Animator animator;

    protected CharacterController controller;

    public PathCreator pathCreator;

    [SerializeField][ReadOnly]
    protected float dstTravelled = 0;

    public EndOfPathInstruction endOfPath;

    public Transform rotator;

    [SyncVar][SerializeField][ReadOnly]
    protected Vector3 direction;

    [SyncVar]
    protected Vector3 lookDirection;

    [SerializeField][ReadOnly]
    protected Mode mode;

    protected enum Mode
    {
        TARGET,
        PATH,
        PATROL
    }

    private void Awake ()
    {
        path = new NavMeshPath();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        state = GetComponent<State>();
        speedBoost = 1f;
        mode = Mode.PATROL;
    }

    public void SetTarget (Transform target)
    {
        if (isServer)
        {
            this.target = target;
            speedBoost = 1.5f;
            this.CalculatePath();
            mode = Mode.TARGET;
        }
    }

    public void UnsetTarget (Transform target)
    {
        if (isServer)
        {
            this.target = null;
            speedBoost = 1f;
            Vector3 dest = pathCreator.path.GetClosestPointOnPath(transform.position);
            dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
            CalculatePath(dest);
            mode = Mode.PATH;
        }
    }

    private void Update ()
    {
        if (isServer)
        {
            if (target != null && mode == Mode.TARGET)
            {
                // target is close (hardFollowRadius) and moved (1f)
                // or path is stale
                if (
                    ((transform.position - target.position).sqrMagnitude < hardFollowRadius * hardFollowRadius && (target.position - lastTargetPosition).sqrMagnitude > 1f) ||
                    (currentPathTime > staleTime)
                ) {
                    this.CalculatePath();
                }
            }

            if (
                (mode == Mode.PATH || mode == Mode.TARGET) &&
                (path.status == NavMeshPathStatus.PathComplete) &&
                (path.corners.Length > currentWaypoint)
            ) {
                currentPathTime += Time.deltaTime;

                // waypoint reached
                if ((path.corners[currentWaypoint] - transform.position).sqrMagnitude < waypointRadius * waypointRadius)
                {
                    currentWaypoint++;

                    if (path.corners.Length > currentWaypoint)
                    {
                        direction = (path.corners[currentWaypoint] - transform.position).normalized;
                        lookDirection = direction;
                    }
                    else
                    // We have reached the end
                    {
                        if (mode == Mode.PATH)
                        {
                            // Go back to patrol
                            mode = Mode.PATROL;
                        }

                        currentWaypoint = 0;
                        currentPathTime = 0;
                        direction = Vector3.zero;
                    }
                }

                controller.Move(direction * speed * speedBoost * state.speed * Time.deltaTime);
                Rotate(lookDirection);
            }
            if (mode == Mode.PATROL)
            {
                dstTravelled += speed * speedBoost * state.speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, endOfPath);
                rotator.rotation = Quaternion.Lerp(rotator.rotation, pathCreator.path.GetRotationAtDistance(dstTravelled, endOfPath), Time.deltaTime * 5f);
            }
        }
        else
        {
            // client
            //controller.Move(direction * speed * speedBoost * Time.deltaTime);
            //Rotate(lookDirection);
        }
    }
    
    protected void Rotate (Vector3 direction)
    {
        rotator.rotation = Quaternion.Lerp(rotator.rotation, Quaternion.AngleAxis(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, Vector3.up), Time.deltaTime * 5f);
    }
    
    protected bool CalculatePath ()
    {
        return CalculatePath(target.position);
    }

    protected bool CalculatePath (Vector3 dest)
    {
        if (isServer)
        {
            lastTargetPosition = dest;
            NavMesh.CalculatePath(transform.position, dest, NavMesh.AllAreas, path);
            currentWaypoint = 0;
            currentPathTime = 0;

            if (path.corners.Length > currentWaypoint)
            {
                direction = (path.corners[currentWaypoint] - transform.position).normalized;
                lookDirection = direction;
            }
        }

        return path.status == NavMeshPathStatus.PathComplete;
    }

}
