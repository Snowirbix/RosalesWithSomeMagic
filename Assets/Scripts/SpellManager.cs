using UnityEngine;
using UnityEngine.Networking;

public class SpellManager : NetworkBehaviour
{
    public GameObject rightHandSpellSpawn;

    public AttackData attackDataLeftClick;

    [ReadOnly]
    public Attack attackLeftClick;

    protected float cdClickLeft;

    protected float cdClickLeftUsed = 0;
    
    protected PlayerController playerController;

    protected Animator animator;

    public void LeftClick (Vector2 lookDirection)
    {
        if (cdClickLeftUsed <= 0)
        {
            CmdLeftClick(lookDirection);
            playerController.SetAnimationAttackTime(attackLeftClick.GetAnimationAttackTime());
        }
    }

    [Command]
    protected void CmdLeftClick (Vector2 lookDirection)
    {
        if (cdClickLeftUsed <= 0)
        {
            cdClickLeftUsed = cdClickLeft;
            attackLeftClick.SetPosition(rightHandSpellSpawn.transform.position);
            attackLeftClick.SetRotation(rightHandSpellSpawn.transform.rotation);
            attackLeftClick.SetDir(lookDirection);
            attackLeftClick.SetAttackOwner(transform.gameObject);
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
        attackLeftClick.SetData(attackDataLeftClick);

        cdClickLeft = attackLeftClick.GetCooldown();
    }

    private void Update ()
    {
        if (isServer)
        {
            if (cdClickLeftUsed > 0)
            {
                cdClickLeftUsed -= Time.deltaTime;
            }
        }
    }
}