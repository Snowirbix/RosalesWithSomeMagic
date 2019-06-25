using UnityEngine;
using System.Collections.Generic;

public class SpellManager : MonoBehaviour {
    public GameObject rightHandSpellSpawn;
    public Attack attackclickLeft;
    private float cdClickLeft;
    private float cdClickLeftUsed = 0;
    private List<Attack> attackList = new List<Attack>();

    private PlayerController playerController;

    private Animator animator;
    public void LeftClick(Vector2 lookDirection){
        if(cdClickLeftUsed <= 0)
        {
            cdClickLeftUsed = cdClickLeft;
            attackclickLeft.Start();
            attackclickLeft.SetPosition(rightHandSpellSpawn.transform.position);
            attackclickLeft.SetDir(lookDirection);
            attackclickLeft.SetAttackOwner(transform.gameObject);
            animator.SetTrigger("fireball");
            playerController.SetAnimationAttackTime(attackclickLeft.GetAnimationAttackTime());

            attackList.Add(attackclickLeft);
        }
        
    }

    void Start()
    {
        cdClickLeft = attackclickLeft.GetCooldown();
        animator = transform.GetComponent<Animator>();
        playerController = transform.GetComponent<PlayerController>();
    }

    void Update()
    {
        for(int i = attackList.Count - 1; i >= 0; i--)
        {
            if(!attackList[i].Update())
            {
                attackList.RemoveAt(i);
            }
        }

        if(cdClickLeftUsed > 0)
        {
            cdClickLeftUsed -= Time.deltaTime;
        }
    }

}