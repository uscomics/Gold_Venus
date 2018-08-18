using UnityEngine;
using System.Collections;

public class DeactivateByTime : MonoBehaviour
{
    public float lifetime = 1.0f;
    public bool activate = false;
    public bool flipAndRepeat = false;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    } // Start

    void Update()
    {
        if (Time.time >= startTime + lifetime)
        {
            gameObject.SetActive(activate);
        }
        if (flipAndRepeat)
        {
            activate = !activate;
            ResetTime();
        }
    }

    public void ResetTime()
    {
        startTime = Time.time;
    }

    public void ResetObjectsActivateState()
    {
        gameObject.SetActive(!activate);
    }
}
