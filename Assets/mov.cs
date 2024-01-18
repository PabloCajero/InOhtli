using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mov : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private float movimientoHorizontal = 0f;
    [SerializeField] private float VelocidadMovimiento;
    [SerializeField] private float Smooth;
    private Vector3 velocidad = Vector3.zero;
    private bool LD = true;


    [SerializeField] private float fuerzaDeSalto;
    [SerializeField] private LayerMask Suelo;
    [SerializeField] private Transform OperadorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool enSuelo;
    private bool salto=false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal") * VelocidadMovimiento;
        if (Input.GetButtonDown("Jump"))
        {
            salto = true;
        }
    }
    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(OperadorSuelo.position, dimensionesCaja, 0f, Suelo);
        Mover(movimientoHorizontal*Time.deltaTime,salto);
        salto = false;
    }

    private void Mover(float mover,bool saltar)
    {
        Vector3 velocidadObjetivo = new Vector2(mover,rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocidadObjetivo,ref velocidad,Smooth);
        if(mover>0 && !LD)
        {
            Girar();
        }
        else if(mover<0 && LD)
        {
            Girar();
        }
        if(enSuelo && saltar)
        {
            enSuelo = false;
            rb.AddForce(new Vector2(0f, fuerzaDeSalto));
        }
    }
    private void Girar()
    {
        LD=!LD;
        Vector3 escala = transform.localScale;
        escala.x *=-1;
        transform.localScale= escala;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(OperadorSuelo.position, dimensionesCaja);
    }
}
