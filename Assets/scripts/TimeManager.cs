using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float slowdownscale;

    public void SlowDownTime()
    {
        Time.timeScale = slowdownscale;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;

    }

    public void SpeedUpTime()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.01f;
    }
}