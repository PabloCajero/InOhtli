using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void ChangeMaxHP(float MaxHealth)
    {
        slider.maxValue=MaxHealth;
    }

    public void ChangeCurrentHealth(float CurrentHP)
    {
        slider.value= CurrentHP;
    }
    public void StartHealthBar(float HPValue)
    {
        ChangeCurrentHealth(HPValue);
    }
}
