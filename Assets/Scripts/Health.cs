using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    [ReadOnly][SyncVar(hook = "OnChangedHealth")]
    public float health = 0;

    public float maxHealth = 100f;

    protected Healthbar healthbar;

    private void Start()
    {
        healthbar = GetComponent<Healthbar>();

        if (isServer)
        {
            health = maxHealth;
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

    public void TakeDamage (int value)
    {
        if (isServer)
        {
            health -= value;
            healthbar.TakeDamage(health / maxHealth);
            healthbar.DisplayDamage(value);
        }
    }

    protected void OnChangedHealth (float h)
    {
        if (!isServer)
        {
            // this hook can be called before Start
            if (healthbar == null)
            {
                healthbar = GetComponent<Healthbar>();
            }
            if (h < health)
            {
                healthbar.TakeDamage(h / maxHealth);
            }
            else
            {
                healthbar.Heal(h / maxHealth);
            }
        }
    }
}
