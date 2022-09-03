using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;
    public float SpeedTime = 1;

    public TextMeshProUGUI  TimerTxt;

    // Start is called before the first frame update
    void Start()
    {
        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {

            if (TimeLeft > 0)
            {
                TimeLeft -= (Time.deltaTime * SpeedTime);
                updateTimer(TimeLeft);
                SpeedTime += 0.0001f;
            }
            else
            {
                Debug.Log("Time is Up");
                TimeLeft = 0;
                TimerOn = false;
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float secondes = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00} : {1:00}", minutes, secondes);
    }
}
