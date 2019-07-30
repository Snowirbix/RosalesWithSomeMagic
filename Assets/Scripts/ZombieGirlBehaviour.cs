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

    private void Start ()
    {
        animator = GetComponent<Animator>();
        pathfinder = GetComponent<Pathfinder>();
        healthScript = GetComponent<Health>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter (Collider collider)
    {
        if (collider.tag == "Player")
        {
            pathfinder.SetTarget(collider.transform);
        }
    }

    private void OnTriggerStay (Collider collider)
    {
        if (collider.tag == "Player")
        {
            Vector3 direction = collider.transform.position - transform.position;

            if (direction.sqrMagnitude < range * range)
            {
                animator.SetBool("attack", true);
                target = collider.transform;
            }
            else
            {
                animator.SetBool("attack", false);
                animator.SetFloat("speed", 1.0f);
            }
        }
    }

    public void Damage ()
    {
        Vector3 direction = target.position - transform.position;

        if (direction.sqrMagnitude < range * range)
        {
            target.GetComponent<Health>()?.TakeDamage(damage);
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
