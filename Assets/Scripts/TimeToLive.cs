using UnityEngine;

public class TimeToLive : MonoBehaviour
{
    public float timeToLive = 1f;

    protected float time;

    private void Start()
    {
        time = Time.time;
    }

    private void Update()
    {
        if (Time.time >= time + timeToLive)
        {
            Destroy(gameObject);
        }
    }
}
