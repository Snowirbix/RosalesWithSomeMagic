using UnityEngine;
using UnityEngine.Networking;

public class SpellManager : NetworkBehaviour
{
    public GameObject rightHandSpellSpawn;
    public AttackData attackDataLeftClick;
    [ReadOnly]
    public Attack attackLeftClick;
    private float cdClickLeft;
    private float cdClickLeftUsed = 0;

    private PlayerController playerController;

    private Animator animator;

    public void LeftClick (Vector2 lookDirection)
    {
        if (cdClickLeftUsed <= 0)
        {
            CmdLeftClick(lookDirection);
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
            animator.SetTrigger("fireball");
            playerController.SetAnimationAttackTime(attackLeftClick.GetAnimationAttackTime());
        }
    }

    /*public void ServerInstantiate (GameObject original, Vector3 position, Quaternion rotation)
    {
        CmdInstantiate(original, position, rotation);
    }

    [Command]
    public void CmdInstantiate (GameObject original, Vector3 position, Quaternion rotation)
    {
        GameObject instance = Instantiate(original, position, rotation);
        NetworkServer.Spawn(instance);
    }*/

    private void Start ()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        if (isServer)
        {
            // Add the script from the data
            attackLeftClick = gameObject.AddComponent(attackDataLeftClick.GetType()) as Attack;
            // Set the data to the script
            attackLeftClick.SetData(attackDataLeftClick);

            cdClickLeft = attackLeftClick.GetCooldown();
        }
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