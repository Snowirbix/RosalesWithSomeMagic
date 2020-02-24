using System;
using UnityEngine;
using Mirror;

public class SpellManager : NetworkBehaviour
{
    #region basicAttack

    public AttackData attackDataLeftClick;

    [ReadOnly]
    public Attack attackLeftClick;

    protected float cdClickLeft;

    [HideInInspector]
    public float cdClickLeftUsed = 0;

    #endregion basicAttack

    #region ultimate

    public AttackData attackDataUltimate;

    [ReadOnly]
    public Attack attackUltimate;

    protected float cdUltimate;

    [HideInInspector]
    public float cdUltimateUsed = 0;

    #endregion ultimate
    
    #region components

    protected PlayerController playerController;

    protected Animator animator;

    protected State state;

    #endregion components

    private void Start ()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        state = GetComponent<State>();
        
        // Get the script related to the data
        attackLeftClick = GetComponent(attackDataLeftClick.GetType()) as Attack;
        // Set the data to the script
        attackLeftClick.data = attackDataLeftClick;

        cdClickLeft = attackLeftClick.data.cooldown;
        
        // Get the script related to the data
        attackUltimate = GetComponent(attackDataUltimate.GetType()) as Attack;
        // Set the data to the script
        attackUltimate.data = attackDataUltimate;

        cdUltimate = attackUltimate.data.cooldown;
    }

    private void Update ()
    {
        if (cdClickLeftUsed > 0)
        {
            cdClickLeftUsed -= Time.deltaTime;
        }
        
        if (cdUltimateUsed > 0)
        {
            cdUltimateUsed -= Time.deltaTime;
        }
    }

    public void LeftClick (Vector3 clickPoint, Vector2 direction, Quaternion rotation)
    {
        if (cdClickLeftUsed <= 0 && !state.silence)
        {
            if (!isServer)
            {
                cdClickLeftUsed = cdClickLeft;
            }
            CmdLeftClick(clickPoint, direction, rotation);
            playerController.SetAnimationAttackTime(attackLeftClick.data.animationAttackTime);
        }
    }

    [Command]
    protected void CmdLeftClick (Vector3 clickPoint, Vector2 direction, Quaternion rotation)
    {
        // quick fix when latency
        if (cdClickLeftUsed <= 0.1f && !state.silence)
        {
            cdClickLeftUsed = cdClickLeft;
            attackLeftClick.Trigger(clickPoint, direction, rotation);
        }
    }

    public void Ultimate (Vector3 clickPoint, Vector2 direction, Quaternion rotation)
    {
        if (cdUltimateUsed <= 0 && !state.silence)
        {
            if (!isServer)
            {
                cdUltimateUsed = cdUltimate;
            }
            CmdUltimate(clickPoint, direction, rotation);
            playerController.SetAnimationAttackTime(attackUltimate.data.animationAttackTime);
        }
    }

    [Command]
    protected void CmdUltimate (Vector3 clickPoint, Vector2 direction, Quaternion rotation)
    {
        // quick fix when latency
        if (cdUltimateUsed <= 0.1f && !state.silence)
        {
            cdUltimateUsed = cdUltimate;
            attackUltimate.Trigger(clickPoint, direction, rotation);
        }
    }
}