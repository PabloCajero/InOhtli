using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mov : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private float HorizontalMovement = 0f;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float Smooth;
    private Vector3 velocidad = Vector3.zero;
    private bool LD = true;


    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask Floor;
    [SerializeField] private Transform OperadorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool inFloor;
    [SerializeField] private bool InPause;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject InterfacePaused;

    private bool Jump=false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMovement = Input.GetAxisRaw("Horizontal") * MoveSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            Jump = true;
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
    }
    private void FixedUpdate()
    {
        inFloor = Physics2D.OverlapBox(OperadorSuelo.position, dimensionesCaja, 0f, Floor);
        Move(HorizontalMovement*Time.deltaTime,Jump);
        Jump = false;
    }

    private void Move(float move,bool jump)
    {
        Vector3 velocidadObjetivo = new Vector2(move,rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocidadObjetivo,ref velocidad,Smooth);
        if(move>0 && !LD)
        {
            Turn();
        }
        else if(move<0 && LD)
        {
            Turn();
        }
        if(inFloor && jump)
        {
            inFloor = false;
            rb.AddForce(new Vector2(0f, JumpForce));
        }
    }
    private void Turn()
    {
        LD=!LD;
        Vector3 scale = transform.localScale;
        scale.x *=-1;
        transform.localScale= scale;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(OperadorSuelo.position, dimensionesCaja);
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
}
