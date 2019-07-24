using UnityEngine;

[RequireComponent(typeof(Pathfinder))]
public class ZombieGirlBehaviour : MonoBehaviour
{
    public float range;

    protected Animator animator;

    protected Pathfinder pathfinder;
    
    private void Start ()
    {
        animator = GetComponent<Animator>();
        pathfinder = GetComponent<Pathfinder>();
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
}
