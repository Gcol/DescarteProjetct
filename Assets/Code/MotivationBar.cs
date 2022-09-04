using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotivationBar : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth ;
    static public float current_percent;
    public bool motivIdle;
    public float motivBoost = 10f;

    public float SpeedDecrease = -3f;

    public Image HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        if ((currentHealth != maxHealth || SpeedDecrease < 1) && motivIdle == false)
        {
            currentHealth += (Time.deltaTime * SpeedDecrease);
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        current_percent = currentHealth / maxHealth;
        HealthBar.fillAmount = current_percent;
    }

    public float get_percent()
    {
        return current_percent;
    }

    public void reset()
    {
        currentHealth = maxHealth;
        freezeMotivation();
    }


    public void freezeMotivation()
    {
        motivIdle = true;
    }

    public void unfreezeMotivation()
    {
        motivIdle = false;
    }

    public void pozClopeMotivBoost()
    {
        currentHealth += motivBoost;
    }
}
