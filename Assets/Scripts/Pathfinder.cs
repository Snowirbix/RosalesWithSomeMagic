using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class Pathfinder : MonoBehaviour
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
    
    protected CharacterController controller;

    private void Start ()
    {
        path = new NavMeshPath();
        controller = GetComponent<CharacterController>();
    }

    public void SetTarget (Transform target)
    {
        this.target = target;
        this.CalculatePath();
    }

    private void Update ()
    {
        if (target)
        {
            currentPathTime += Time.deltaTime;

            // target is close (followRadius) and moved (1f)
            if ((transform.position - target.position).sqrMagnitude < hardFollowRadius * hardFollowRadius && (target.position - lastTargetPosition).sqrMagnitude > 1f)
            {
                this.CalculatePath();
            }
            // or path is stale
            if (currentPathTime > staleTime)
            {
                this.CalculatePath();
            }

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                if (path.corners.Length > currentWaypoint)
                {
                    // waypoint reached
                    if ((path.corners[currentWaypoint] - transform.position).sqrMagnitude < waypointRadius * waypointRadius)
                    {
                        currentWaypoint++;
                    }
                    // if there are still waypoints to go
                    if (path.corners.Length > currentWaypoint)
                    {
                        controller.Move((path.corners[currentWaypoint] - transform.position).normalized * speed * Time.deltaTime);
                    }
                }
            }
        }
    }
    
    protected bool CalculatePath ()
    {
        lastTargetPosition = target.position;
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        currentWaypoint = 0;
        currentPathTime = 0;

        return path.status == NavMeshPathStatus.PathComplete;
    }

}
