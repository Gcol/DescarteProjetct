using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotivationBar : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth ;
    static public float current_percent;
    public float StartDebuf = 50;

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
        if (currentHealth != maxHealth || SpeedDecrease < 1)
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
        currentHealth = maxHealth - StartDebuf;
    }

    public void InverseMotivationLose()
    {
     SpeedDecrease *= -1;
    }

}
