using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : hpSystem
{
    [Header("Menu Settings")]
    [SerializeField] private bool InPause;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject InterfacePaused;
    [Header("attack Settings")]
    [SerializeField] private Transform AttackOperator;
    [SerializeField] private float AttackRadio;
    [SerializeField] private float AttackDamage;
    [SerializeField] private float TimeBetweenAttack;
    [SerializeField] private float TimeNextAttack;
    [SerializeField] private float AttackDuration;
    [Header("Dash Settings")]
    [SerializeField] private float MaxCharge;
    [SerializeField] private float TimeCharge;
    
    private bool canDash=true;
    [SerializeField]private float DashTime;
    [SerializeField]private float DashSpeed;
    private bool canMove=true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        setstartingValue(getCurrentHP());
        //animator = GetComponent<Animator>();
    }
   void Update()
    {
        if(canMove){
            HorizontalMovement = Input.GetAxisRaw("Horizontal") * MoveSpeed;
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
            
            if(Input.GetButtonDown("Fire1") && TimeNextAttack <=0){
                Invoke("CharacterHit",AttackDuration);
                //animator.SetTrigger("AttackTrigger");
                TimeNextAttack=TimeBetweenAttack;
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (!InPause) {
                Pause();
            }
            else
            {
                Resume();
            }
        }
        if(TimeNextAttack>0){
            TimeNextAttack -= Time.deltaTime;
        }
        if(Input.GetButton("Fire3")){
            if(TimeCharge<=MaxCharge){
                TimeCharge += Time.deltaTime;
            }
        }
        if(Input.GetButtonUp("Fire3") && canDash){
            if(TimeCharge>=MaxCharge){
                StartCoroutine(Dash());
                Debug.Log("Dasheo");
            }else{
                Debug.Log("No Dasheo");
            }
            TimeCharge=0;
        }
        
    }
    private void CharacterHit(){
        Collider2D[] Objects = Physics2D.OverlapCircleAll(AttackOperator.position, AttackRadio);
        foreach (Collider2D colition in Objects){
            if(colition.CompareTag("Enemigo")){
                colition.transform.GetComponent<HP>().Damage(AttackDamage);
            }
        }
    }
    private IEnumerator Dash(){
        canMove=false;
        canDash=false;
        float FixedSpeed;
        if(LD){
            FixedSpeed= DashSpeed*1;
        }else{
            FixedSpeed= DashSpeed*-1;
        }
        rb.gravityScale=0;
        rb.velocity=new Vector2(FixedSpeed,0);
        //anim

        yield return new WaitForSeconds(DashTime);

        canMove=true;
        canDash=true;
        rb.gravityScale=4;
    }
    private void Pause()
    {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        InterfacePaused.SetActive(false);
        InPause =true;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        InterfacePaused.SetActive(!InterfacePaused.activeSelf);
        InPause = false;
    }
    private void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(AttackOperator.position, AttackRadio);
        Gizmos.DrawWireCube(OperadorSuelo.position, dimensionesCaja);
    }
}