using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Healthbar : MonoBehaviour
{
    [Range(0f, 1f)]
    public float deltaTime = 0.2f;

    public RectTransform backgroundBar;
    public RectTransform damageBar;
    public RectTransform healthBar;
    protected float maxWidth;
    protected List<Change> changes = new List<Change>();

    protected struct Change
    {
        public float healthRatio;
        public float time;
    }

    private void Awake()
    {
        maxWidth = backgroundBar.rect.width;
    }

    private void Update()
    {
        for (int i = changes.Count-1; i >= 0; i--)
        {
            if (Time.unscaledTime > changes[i].time + deltaTime)
            {
                damageBar.sizeDelta = new Vector2(changes[i].healthRatio * maxWidth, damageBar.sizeDelta.y);
                changes.RemoveAt(i);
            }
        }
    }

    public void Damage (int damageAmount, int health, int maxHealth)
    {
        float healthRatio = (float)health / (float)maxHealth;
        healthBar.sizeDelta = new Vector2(healthRatio * maxWidth, healthBar.sizeDelta.y);

        // delayed update
        Change change = new Change();
        change.healthRatio = healthRatio;
        change.time = Time.unscaledTime;
        changes.Add(change);
    }

    public void Heal (int healAmount, int health, int maxHealth)
    {
        float healthRatio = (float)health / (float)maxHealth;
        healthBar.sizeDelta = new Vector2(healthRatio * maxWidth, healthBar.sizeDelta.y);
        // instant update
        damageBar.sizeDelta = new Vector2(healthRatio * maxWidth, damageBar.sizeDelta.y);
    }
}
