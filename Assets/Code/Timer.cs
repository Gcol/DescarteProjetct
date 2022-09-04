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

    public MotivationBar motiv;
    public MainLoop mainGame;
    public SpriteRenderer Arrow;
    public SpriteRenderer MinArrow;

    float baseAngle = 80f;
    int divAngle = 255;

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
                UpdateClockPosition();
            }
            else
            {
                TimeLeft = 0;
                TimerOn = false;
                mainGame.NewDay();
            }
        }
    }
    public void UpdateClockPosition()
    {
        float z = baseAngle - ((100 -(TimeLeft / MaxTime)) * divAngle);
        Arrow.transform.rotation = Quaternion.Euler(
            Arrow.transform.rotation.x,
            Arrow.transform.rotation.y,
            z
        );
        MinArrow.transform.rotation = Quaternion.Euler(
            Arrow.transform.rotation.x,
            Arrow.transform.rotation.y,
            z * 60
        );

    }

    public void reset()
    {
        TimeLeft = MaxTime;
        motiv.reset();
        UpdateClockPosition();
        TimerOn = false;
    }

    public void SetTimer(bool value){
        TimerOn = value;
    }
}
