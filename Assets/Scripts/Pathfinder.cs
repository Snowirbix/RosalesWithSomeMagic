using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using PathCreation;

[RequireComponent(typeof(CharacterController))]
public class Pathfinder : NetworkBehaviour
{
    public float speed = 3f;

    protected NavMeshPath path;

    protected Transform target;

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

    private void Start ()
    {
        path = new NavMeshPath();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetFloat("speed", 1.0f);
        mode = Mode.PATROL;
    }

    public void SetTarget (Transform target)
    {
        if (isServer)
        {
            this.target = target;
            this.CalculatePath();
            mode = Mode.TARGET;
        }
    }

    public void UnsetTarget (Transform target)
    {
        if (isServer)
        {
            this.target = null;
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

                controller.Move(direction * speed * Time.deltaTime);
                Rotate(lookDirection);
            }
            if (mode == Mode.PATROL)
            {
                dstTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, endOfPath);
                rotator.rotation = pathCreator.path.GetRotationAtDistance(dstTravelled, endOfPath);
            }
        }
        else
        {
            // client
            //controller.Move(direction * speed * Time.deltaTime);
            //Rotate(lookDirection);
        }
    }
    
    protected void Rotate (Vector3 direction)
    {
        rotator.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, Vector3.up);
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
