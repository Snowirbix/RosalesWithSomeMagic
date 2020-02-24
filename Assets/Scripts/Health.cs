using System;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class Health : NetworkBehaviour
{
    [ReadOnly][SyncVar(hook = "OnChangedHealth")]
    public int health = 0;

    public int maxHealth = 100;

    [System.Serializable]
    public class HealEvent : UnityEvent<int, int, int> {};

    [System.Serializable]
    public class DamageEvent  : UnityEvent<int, int, int> {};

    /// <summary>
    /// heal amount, health, max health
    /// </summary>
    public HealEvent    healEvent;
    /// <summary>
    /// damage amount, health, max health
    /// </summary>
    public DamageEvent  damageEvent;
    public UnityEvent   deathEvent;

    [ReadOnly]
    public bool isDead = false;

    private void Start()
    {
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
            healEvent.Invoke(value, health, maxHealth);
        }
    }

    public void Damage (int value)
    {
        if (isServer)
        {
            health -= value;
            health = Math.Max(health, 0);
            damageEvent.Invoke(value, health, maxHealth);

            if (health == 0 && !isDead)
            {
                isDead = true;
                deathEvent.Invoke();
            }
        }
    }

    protected void OnChangedHealth (int oldValue, int newValue)
    {
        if (!isServer)
        {
            if (newValue > health)
            {
                healEvent.Invoke(newValue-oldValue, newValue, maxHealth);
            }
            else
            {
                damageEvent.Invoke(newValue-oldValue, newValue, maxHealth);
            }
        }
    }
}
