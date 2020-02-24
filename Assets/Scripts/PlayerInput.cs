using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : NetworkBehaviour
{
    public LayerMask groundLayer;
    protected PlayerController playerController;

    protected SpellManager spellManager;

    private void Awake ()
    {
        playerController = GetComponent<PlayerController>();
        spellManager = GetComponent<SpellManager>();
    }

    private void Update ()
    {
        Keyboard();
        Mouse();
    }

    protected void Keyboard()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 dir2 = new Vector2(h, v);
        dir2 = (dir2.magnitude > 1f) ? dir2.normalized : dir2;

        playerController.SetMoveDirection(dir2);
    }

    protected void Mouse ()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200f, groundLayer))
        {
            Vector3 dir3 = (hit.point - transform.position);
            Vector2 dir2 = new Vector2(dir3.x, dir3.z);
            Vector2 lookDirection = (dir2.magnitude > 1f) ? dir2.normalized : dir2;
            dir2.Normalize();
            Quaternion rotation = Quaternion.AngleAxis(Mathf.Atan2(dir2.x, dir2.y) * Mathf.Rad2Deg, Vector3.up);

            playerController.SetLookDirection(lookDirection);

            if(Input.GetMouseButtonDown(0))
            {
                spellManager.LeftClick(hit.point, dir2, rotation);
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                spellManager.Ultimate(hit.point, dir2, rotation);
            }
        }
    }
}
