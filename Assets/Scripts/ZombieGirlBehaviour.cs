using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGirlBehaviour : MonoBehaviour
{
    public float range;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collider) {
        Vector3 distance = collider.transform.position - gameObject.transform.position;
        if(distance.sqrMagnitude < range * range)
        {
            animator.SetTrigger("playerInRange");
        }
        else
        {
            animator.SetFloat("Speed",1.0f);
        }
        
    }
}
