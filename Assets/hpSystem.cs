using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpSystem : MonoBehaviour
{
    [SerializeField] private float ActualHP;
    [SerializeField] private float MaxHP;
    [SerializeField] private BarraHP barraHP;

    private void Start()
    {
        ActualHP = MaxHP;
    }

    public void Daño(float ValorDaño)
    {
        ActualHP -= ValorDaño;
        barraHP.CambiarHPActual(ActualHP);
        if (ActualHP <= 0 )
        {
            Destroy(gameObject);
        }
    }

    public void Cura(float ValorCura)
    {
        if ((ActualHP+ValorCura) > ActualHP)
        {
            ActualHP = MaxHP;
        }
        else
        {
            ActualHP += ValorCura;
        }
    }
}
