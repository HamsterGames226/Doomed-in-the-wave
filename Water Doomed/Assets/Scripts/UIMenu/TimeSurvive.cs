using UnityEngine;
using TMPro;
using System;

public class TimeSurvive : MonoBehaviour
{
    public static float timeSurvive = 0f;
    public TextMeshProUGUI textTimeSurvive;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (DieScript.life)
        {
            timeSurvive += Time.deltaTime;
        }

        textTimeSurvive.text = $"{Math.Floor(timeSurvive/60)}:{(timeSurvive % 60f):00}";
    }
}
