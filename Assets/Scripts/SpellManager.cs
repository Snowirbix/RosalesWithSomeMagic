using UnityEngine;

public class SpellManager : MonoBehaviour {
    public GameObject clickLeftSpell;
    private Attack attackclickLeft;
    private float cdClickLeft;

    private PlayerController playerController;

    private Animator animator;
    public void LeftClick(Vector2 lookDirection){
        GameObject go = Instantiate(clickLeftSpell,new Vector3(transform.position.x,transform.position.y + 1, transform.position.z),Quaternion.identity);
        Attack at = go.GetComponent<Attack>();
        at.SetDir(lookDirection);
        at.SetAttackOwner(transform.gameObject);
        animator.SetTrigger("fireball");
        playerController.SetAnimationAttackTime(at.GetAnimationAttackTime());
    }

    void Start()
    {
        attackclickLeft = clickLeftSpell.GetComponent<Attack>();
        cdClickLeft = attackclickLeft.GetCooldown();
        animator = transform.GetComponent<Animator>();
        playerController = transform.GetComponent<PlayerController>();
    }

}