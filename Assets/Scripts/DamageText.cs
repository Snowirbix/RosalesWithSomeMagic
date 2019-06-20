using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Text text;

    private float timeToLive = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.up * 2;
        timeToLive -= Time.deltaTime;
        if(timeToLive <= 0 ){
            Destroy(gameObject);
        }   
    }


}
