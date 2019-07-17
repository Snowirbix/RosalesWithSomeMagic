using System;
using UnityEngine;
using UnityEngine.Networking;

public class SpellManager : NetworkBehaviour
{
    public AttackData attackDataLeftClick;

    [ReadOnly]
    public Attack attackLeftClick;

    protected float cdClickLeft;

    protected float cdClickLeftUsed = 0;
    
    protected PlayerController playerController;

    protected Animator animator;

    [Obsolete("Change lookDirection to direction and get the click position")]
    public void LeftClick (Vector2 lookDirection)
    {
        if (cdClickLeftUsed <= 0)
        {
            if (!isServer)
            {
                cdClickLeftUsed = cdClickLeft;
            }
            CmdLeftClick(lookDirection);
            playerController.SetAnimationAttackTime(attackLeftClick.data.animationAttackTime);
        }
    }

    [Command]
    protected void CmdLeftClick (Vector2 lookDirection)
    {
        // quick fix when latency
        if (cdClickLeftUsed <= 0.2f)
        {
            cdClickLeftUsed = cdClickLeft;
            attackLeftClick.direction = lookDirection;
            attackLeftClick.Trigger();
        }
    }

    private void Start ()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        
        // Get the script related to the data
        attackLeftClick = GetComponent(attackDataLeftClick.GetType()) as Attack;
        // Set the data to the script
        attackLeftClick.data = attackDataLeftClick;

        cdClickLeft = attackLeftClick.data.cooldown;
    }

    private void Update ()
    {
        if (cdClickLeftUsed > 0)
        {
            cdClickLeftUsed -= Time.deltaTime;
        }
    }
}