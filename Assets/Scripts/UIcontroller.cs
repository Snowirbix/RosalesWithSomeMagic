﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIcontroller : MonoBehaviour
{

    public SpellManager spellManager;

    public RectTransform healthbar;

    public Text healthText;

    public Image cooldownLeftImage;

    public Text cooldownLeftText;

    public Image leftClickSpell;

    public Canvas uI;

    private float maxWidth;

    // Start is called before the first frame update
    private void Awake()
    {
        // Set spell image here and life
        leftClickSpell.sprite = spellManager.attackDataLeftClick.image;
        maxWidth = healthbar.rect.width;
    }

    // Update is called once per frame
    private void Update()
    {
        Cooldown();
    }

    protected void Cooldown()
    {
        float cooldownLeft = spellManager.cdClickLeftUsed;
        if(cooldownLeft > 0)
        {
            if(!cooldownLeftImage.enabled || !cooldownLeftText.enabled)
            {
                cooldownLeftImage.enabled = true;
                cooldownLeftText.enabled = true;
            }
            cooldownLeftText.text = cooldownLeft.ToString("F2");
        }
        else
        {
            if(cooldownLeftImage.enabled || cooldownLeftText.enabled)
            {
                cooldownLeftImage.enabled = false;
                cooldownLeftText.enabled = false;
            }
        }
    }

    public void OnChangedHealth(int amount, int health, int maxHealth)
    {
        float healthRatio = (float)health/(float)maxHealth;
        healthbar.sizeDelta = new Vector2(healthRatio * maxWidth, healthbar.sizeDelta.y);
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
    }
}
