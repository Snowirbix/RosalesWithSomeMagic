using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGirlBehaviour : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider collider) {
        animator.SetTrigger("playerInRange");
    }
}
