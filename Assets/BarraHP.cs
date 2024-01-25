using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraHP : MonoBehaviour
{
    private Slider slider;
    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void CambiarMaxHP(float MaxHP)
    {
        slider.maxValue=MaxHP;
    }

    public void CambiarHPActual(float ActualHP)
    {
        slider.value= ActualHP;
    }
    public void StartBarraHP(float HPValue)
    {
        CambiarHPActual(HPValue);
    }
}
