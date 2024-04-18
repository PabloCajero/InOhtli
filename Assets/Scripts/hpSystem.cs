using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpSystem : MonoBehaviour
{
    [SerializeField] public float CurrentHealth;
    [SerializeField] private float MaxHealth;
    [SerializeField] private HealthBar HealthBar;

    private void Start()
    {
        
    }

    public void Damage(float DamageValue)
    {
        CurrentHealth -= DamageValue;
         Debug.Log("Damage: " + DamageValue);
        if(HealthBar!=null){
            HealthBar.ChangeCurrentHealth(CurrentHealth);
        }
        if (CurrentHealth <= 0 )
        {
            Destroy(gameObject);
        }
    }

    public void Cure(float CureValue)
    {
        if ((CurrentHealth + CureValue) > CurrentHealth)
        {
            CurrentHealth = MaxHealth;
        }
        else
        {
            CurrentHealth += CureValue;
        }
    }
    public void SetHP(float newHP){
        CurrentHealth=newHP;
    }
    public float GetHP(){
        return CurrentHealth;
    }
}
