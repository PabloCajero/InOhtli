using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform AttackOperator;
    [SerializeField] private float AttackRadio;
    [SerializeField] private float AttackDamage;
    [SerializeField] private float TimeBetweenAttack;
    [SerializeField] private float TimeNextAttack;
    [SerializeField] private float AttackDuration;
    private Animator animator;

    private void Start(){
        animator = GetComponent<Animator>();
    }
    private void Update(){
        if(TimeNextAttack>0){
            TimeNextAttack -= Time.deltaTime;
        }
        if(Input.GetButtonDown("Fire1") && TimeNextAttack <=0){
            Invoke("CharacterHit",0.5f);
            animator.SetTrigger("AttackTrigger");
            TimeNextAttack=TimeBetweenAttack;
        }
    }
    private void CharacterHit(){
        Collider2D[] Objects = Physics2D.OverlapCircleAll(AttackOperator.position, AttackRadio);
        foreach (Collider2D colition in Objects){
            if(colition.CompareTag("Enemigo")){
                colition.transform.GetComponent<hpSystem>().Damage(AttackDamage);
            }
        }
    }
    private void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(AttackOperator.position, AttackRadio);
    }
    
}
