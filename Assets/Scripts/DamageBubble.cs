using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class DamageBubble : NetworkBehaviour
{
    public GameObject damageTextPrefab;
    
    public Transform canvas;

    public void DisplayDamage (int damageAmount, int health, int maxHealth)
    {
        if (isServer)
        {
            RpcDisplayDamage(damageAmount);
        }
    }

    [ClientRpc]
    protected void RpcDisplayDamage (int damageAmount)
    {
        GameObject instance = Instantiate(damageTextPrefab, canvas.transform.position , Quaternion.Euler(50, 0, 0), canvas.transform);
        instance.transform.localPosition =  new Vector3(71, -305, 0.3f);
        Text text = instance.GetComponent<Text>();
        text.text = damageAmount.ToString();
        text.color = Color.red;
        text.fontSize = (int)Mathf.Lerp(50, 150, damageAmount / 1000);
    }
}
