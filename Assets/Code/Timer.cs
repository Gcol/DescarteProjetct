using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float MaxTime;

    public bool TimerOn = false;
    private float TimeLeft;
    private float SpeedTime = 0;

    public TextMeshProUGUI TimerTxt;
    public MotivationBar motiv;
    public MainLoop mainGame;

    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            SpeedTime = (100 - (motiv.get_percent() * 100)) / 4;
            if (TimeLeft > 0)
            {
                TimeLeft -= (Time.deltaTime * SpeedTime);
            }
            else
            {
                Debug.Log("Time is Up");
                TimeLeft = 0;
                TimerOn = false;
                mainGame.NewDay();

            }
            updateTimer(TimeLeft);
        }
    }

    void updateTimer(float currentTime)
    {

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float secondes = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00} : {1:00}", minutes, secondes);
    }

    public void reset()
    {
        TimerOn = true;
        TimeLeft = MaxTime;
        motiv.reset();
    }

    public void ChangeFreeze(){
        TimerOn = !TimerOn;
    }

}
