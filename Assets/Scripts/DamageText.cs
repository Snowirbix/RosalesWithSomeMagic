using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    protected Text text;

    public float timeToLive = .5f;
    
    private void Start ()
    {
        text = transform.GetComponent<Text>();
    }


    private void Update ()
    {
        transform.localPosition += Vector3.up * 2;
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }   
    }
}
