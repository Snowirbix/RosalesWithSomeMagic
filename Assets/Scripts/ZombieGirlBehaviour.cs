using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Pathfinder))]
public class ZombieGirlBehaviour : NetworkBehaviour
{
    public float range;

    public int damage = 30;

    protected Animator animator;

    protected Pathfinder pathfinder;

    protected Health healthScript;

    protected CharacterController characterController;

    public Canvas healthBar;

    [ReadOnly]
    public Transform target;

    public float sightDistance = 7f;

    public float hearDistance = 3f;

    public Transform rotator;

    public LayerMask detectionLayerMask;

    private void Start ()
    {
        animator = GetComponent<Animator>();
        pathfinder = GetComponent<Pathfinder>();
        healthScript = GetComponent<Health>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update ()
    {
        if (isServer)
        {
            Transform bestTarget = null;
            float bestScore = 0;
            
            foreach (PlayerController player in PlayerController.players)
            {
                float score = Detect(player.transform);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestTarget = player.transform;
                }
            }

            if (bestScore == 0 && pathfinder.target)
            {
                pathfinder.UnsetTarget(pathfinder.target);
            }
            if (bestScore > 0 && (pathfinder.target == null || pathfinder.target != bestTarget))
            {
                pathfinder.SetTarget(bestTarget);
            }
            // start attack if someone is in range
            if (bestScore == 3f && !animator.GetBool("attack"))
            {
                RpcStartAttack();
            }
            // stop attack if no one is in range
            if (bestScore < 3f && animator.GetBool("attack"))
            {
                RpcStopAttack();
            }
        }
    }

    // return a detection score
    protected float Detect (Transform t)
    {
        Vector3 direction = (t.position - transform.position);
        Vector3 pos  = transform.position + Vector3.up;

        if (direction.sqrMagnitude <= range * range)
        {
            return 3f;
        }
        else if (direction.sqrMagnitude <= hearDistance * hearDistance)
        {
            Debug.DrawRay(pos, direction, Color.blue, Time.deltaTime*2);
            return 2f;
        }
        else if (direction.sqrMagnitude <= sightDistance * sightDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(pos, direction, out hit, sightDistance, detectionLayerMask))
            {
                if (hit.transform == t)
                {
                    if (Vector3.Angle(direction, rotator.forward) <= 80)
                    {
                        Debug.DrawRay(pos, direction, Color.green, Time.deltaTime*2);
                        return Mathf.Lerp(2f, 0f, direction.sqrMagnitude / (sightDistance * sightDistance));
                    }
                    else
                    {
                        Debug.DrawRay(pos, direction, new Color(1, 0.2f, 0), Time.deltaTime*2);
                    }
                }
                else
                {
                    Debug.DrawRay(pos, direction, Color.red, Time.deltaTime*2);
                }
            }
        }

        return 0f;
    }

    [ClientRpc]
    protected void RpcStartAttack ()
    {
        animator.SetBool("attack", true);
    }

    [ClientRpc]
    protected void RpcStopAttack ()
    {
        animator.SetBool("attack", false);
    }

    // Called by AnimationEvent
    public void Damage ()
    {
        foreach (PlayerController player in PlayerController.players)
        {
            Vector3 direction = (player.transform.position - transform.position);
            
            // if player is in range and in front of the zombie
            if (direction.sqrMagnitude < range * range && Vector3.Angle(direction, rotator.forward) <= 80)
            {
                player.GetComponent<Health>()?.TakeDamage(damage);
            }
        }
    }

    public void Death()
    {
        RpcDeath();
    }

    [ClientRpc]
    public void RpcDeath ()
    {
        pathfinder.enabled = false;
        characterController.enabled = false;
        healthBar.enabled = false;
        animator.SetTrigger("dead");
    }
}
