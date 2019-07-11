using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : NetworkBehaviour
{
    public LayerMask groundLayer;
    private PlayerController playerController;

    private SpellManager spellManager;

    private void Start ()
    {
        if (!isLocalPlayer)
        {
            enabled = false;
        }
        playerController = GetComponent<PlayerController>();
        spellManager = GetComponent<SpellManager>();
    }

    private void Update ()
    {
        Keyboard();
        Mouse();
    }

    private void Keyboard()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 dir2 = new Vector2(h, v);
        dir2 = (dir2.magnitude > 1f) ? dir2.normalized : dir2;

        playerController.SetMoveDirection(dir2);
    }

    private void Mouse ()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200f, groundLayer))
        {
            Vector3 dir3 = (hit.point - transform.position);
            Vector2 dir2 = new Vector2(dir3.x, dir3.z);
            dir2 = (dir2.magnitude > 1f) ? dir2.normalized : dir2;

            playerController.SetLookDirection(dir2);

            if(Input.GetMouseButtonDown(0))
            {
                spellManager.LeftClick(playerController.lookDirection);
            }
        }
    }
}
