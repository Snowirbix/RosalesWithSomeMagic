using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class Pathfinder : MonoBehaviour
{
    protected NavMeshPath path;

    protected Transform target;

    protected Vector3 lastTargetPosition;

    [Tooltip("Recompute path every staleTime (in seconds)")]
    public float staleTime = 1f;

    public float followRadius = 10f;

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
        currentPathTime += Time.deltaTime;

        // target is close (followRadius) and moved (1f)
        if ((transform.position - target.position).sqrMagnitude < followRadius * followRadius && (target.position - lastTargetPosition).sqrMagnitude > 1f)
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
            // waypoint reached
            if ((path.corners[currentWaypoint] - transform.position).sqrMagnitude < waypointRadius * waypointRadius)
            {
                currentWaypoint++;
            }
            // if there are still waypoints to go
            if (path.corners.Length > currentWaypoint)
            {
                controller.Move(path.corners[currentWaypoint]);
            }
        }
    }
    
    protected bool CalculatePath ()
    {
        Debug.Log("CalcultatePath");
        lastTargetPosition = target.position;
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        currentWaypoint = 0;
        currentPathTime = 0;

        return path.status == NavMeshPathStatus.PathComplete;
    }

}
