using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Pathfinder))]
public class ZombieGirlBehaviour : MonoBehaviour
{
    public float range;

    protected Animator animator;

    protected Pathfinder pathfinder;

    protected Health healthScript;

    [ReadOnly]
    public float health = 0;

    private bool dead = false;
    
    private void Start ()
    {
        animator = GetComponent<Animator>();
        pathfinder = GetComponent<Pathfinder>();
        healthScript = GetComponent<Health>();
        health = healthScript.health;
    }

    private void FixedUpdate()
    {
        health = healthScript.health;
        if(health == 0 && !dead)
        {
            Death();
        }
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
            }
            else
            {
                animator.SetBool("attack", false);
                animator.SetFloat("speed", 1.0f);
            }
        }
    }

    private void Death()
    {
        dead = true;
        pathfinder.enabled = false;
        animator.SetTrigger("dead");
    }
}
