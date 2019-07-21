using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIcontroller : NetworkBehaviour
{

    public SpellManager spellManager;

    public Health healthScript;

    public RectTransform life;

    public Text lifeText;

    public Image cooldownLeftImage;

    public Text cooldownLeftText;

    public Canvas uI;
    private float health;
    private float maxHealth;

    private float maxwidth;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            uI.enabled = false;
        }
        //Set spell image here and life
        maxHealth = healthScript.maxHealth;
        health = maxHealth;
        lifeText.text = health.ToString() + "/" + maxHealth.ToString();
        maxwidth = life.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        Life();
        Cooldown();
    }

    void Cooldown(){
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

    void Life(){
        float healthNow = healthScript.health;
        if(healthNow != health)
        {
            health = healthNow;
            float healthRatio = health/maxHealth;
            life.sizeDelta = new Vector2(healthRatio * maxwidth, life.sizeDelta.y);
            lifeText.text = health.ToString() + "/" + maxHealth.ToString();
        }
    }
}
