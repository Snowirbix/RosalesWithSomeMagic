using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    [ReadOnly][SyncVar]
    public float health;

    public float maxHealth = 100f;

    protected Healthbar healthbar;

    private void Start()
    {
        healthbar = GetComponent<Healthbar>();
        
        health = maxHealth;
    }

    public void TakeDamage (int value)
    {
        if (isServer)
        {
            health -= value;
            healthbar.TakeDamage(health / maxHealth);
            healthbar.DisplayDamage(value);
        }
    }

    public void Heal (int value)
    {
        if (isServer)
        {
            health += value;
            healthbar.Heal(health / maxHealth);
        }
    }
}
