using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    [SerializeField] private float tiempoEntreHits;
    private float tiempoInvencivilidad;
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            tiempoInvencivilidad -= Time.deltaTime;
            if(tiempoInvencivilidad <= 0)
            {
                other.GetComponent<hpSystem>().Daño(1);
                tiempoInvencivilidad = tiempoEntreHits;
            }
            
        }
    }
}
