using UnityEngine;

public class Health : MonoBehaviour
{
    public Healthbar healthbar;

    //[ReadOnly]
    public float health;
    public float maxHealth = 100f;

    private void Start()
    {
        if (!healthbar)
        {
            throw new UnassignedReferenceException();
        }
        
        health = maxHealth;
    }

    private void Update()
    {
        // for testing purpose only
        if (Input.GetMouseButtonDown(0))
        {
            this.TakeDamage(10f);
        }
    }

    public void TakeDamage (float value)
    {
        health -= value;
        healthbar.TakeDamage(health / maxHealth);
    }

    public void Heal (float value)
    {
        health += value;
        healthbar.Heal(health / maxHealth);
    }
}
