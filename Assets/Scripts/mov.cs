using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mov : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Animator animator;
    private float HorizontalMovement = 0f;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float Smooth;
    private Vector3 velocidad = Vector3.zero;
    private bool LD = true;


    [SerializeField] public float JumpForce;
    [SerializeField] private LayerMask Floor;
    [SerializeField] private Transform OperadorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool inFloor;
    private bool jump=false;
    //Valores del Dash
    public float dashSpeed = 20;
    public bool canDash;
    public bool makindDash;

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
            jump = true;
        }
    }
    private void FixedUpdate()
    {
        inFloor = Physics2D.OverlapBox(OperadorSuelo.position, dimensionesCaja, 0f, Floor);
        Move(HorizontalMovement*Time.deltaTime,jump);
        jump = false;
    }

    private void Move(float move,bool jump)
    {
        Vector3 velocidadObjetivo = new Vector2(move,rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocidadObjetivo,ref velocidad,Smooth);
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        if (move>0 && !LD)
        {
            TurnRigth();
        }
        else if(move<0 && LD)
        {
            TurnLeft();
        }
        if(inFloor && jump)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.X) && makindDash == false)//!makindDash
        {
            if (xRaw != 0 || yRaw != 0)
            {
                Dash(xRaw, yRaw);
            }
        }
    }
    private void TurnRigth()
    {
        LD = !LD;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    private void TurnLeft()
    {
        LD=!LD;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void CoolerJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.0f - 1) * Time.deltaTime;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * JumpForce;
    }

    private void Dash(float x, float y)
    {
        animator.SetBool("dash", true);
        Vector3 playerPosition = Camera.main.WorldToViewportPoint(transform.position);
        Camera.main.GetComponent<RippleEffect>().Emit(playerPosition);

        canDash = true;
        rb.velocity = Vector2.zero;
        rb.velocity += new Vector2(x, y).normalized * dashSpeed;
        StartCoroutine(PrepareDash());
    }

    private IEnumerator PrepareDash()
    {
        StartCoroutine(FloorDash());
        rb.gravityScale = 0;
        makindDash = true;

        yield return new WaitForSeconds(0.3f);
        rb.gravityScale = 1;
        makindDash = false;
        EndDash();
    }

    private IEnumerator FloorDash()
    {
        yield return new WaitForSeconds(0.15f);
        if (inFloor)
        {
            canDash = false;
            animator.SetBool("jump", false);
        }
    }

    public void EndDash()
    {
        animator.SetBool("dash", false);
    }

    private void TouchFloor()
    {
        canDash = false;
        makindDash = false;
        animator.SetBool("jump", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(OperadorSuelo.position, dimensionesCaja);
    }
}
