using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //<>

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 direction;
    //private CinemachineVirtualCamera vm;
    private Vector2 MovementDirection;

    [Header(("Estadisticas del jugador"))]
    public float movementSpeed = 10;
    public float jumpForse = 7;
    public float dashSpeed = 20;

    [Header(("Colisiones"))]
    public LayerMask floorLayer;
    public float collisionRadius;
    public Vector2 below;

    [Header(("Booleanos del jugador"))]
    public bool canMove = true;
    public bool inFloors = true;
    public bool canDash;
    public bool makindDash;
    public bool touchingFloors;
    public bool isShaking;
    public bool isAttacking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //vm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();  
        Graps();
    }

    private void Attack(Vector2 direction)
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(!isAttacking && !makindDash)
            {
                isAttacking = true;
                animator.SetFloat("attackX", direction.x);
                animator.SetFloat("attackY", direction.y);

                animator.SetBool("attack", true);
            }
        }
    }

    private void EndAttack()
    {
        animator.SetBool("attack", false);
        isAttacking = false;
    }

    private Vector2 AttackDirection(Vector2 MovementDirection, Vector2 direction)
    {
        if(rb.velocity.x == 0 && direction.y != 0)
        {
            return new Vector2(0, direction.y);
        }
        return new Vector2(MovementDirection.x, direction.y);
    }

    /*private IEnumerator CameraDamage()
    {
        isShaking = true;
        CinemachineBasicMultiChannelPerlin cinemachinebasicmultichannelperlin = vm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachinebasicmultichannelperlin.m_AmplitudeGain = 5;
        yield return new WaitForSeconds(0.3f);
        cinemachinebasicmultichannelperlin.m_AmplitudeGain = 0;
        isShaking = false;
    }

    private IEnumerator CameraDamage(float timeDamage)
    {
        isShaking = true;
        CinemachineBasicMultiChannelPerlin cinemachinebasicmultichannelperlin = vm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachinebasicmultichannelperlin.m_AmplitudeGain = 5;
        yield return new WaitForSeconds(timeDamage);
        cinemachinebasicmultichannelperlin.m_AmplitudeGain = 0;
        isShaking = false;
    }*/

    private void Dash(float x, float y)
    {
        animator.SetBool("dash", true);
        Vector3 playerPosition = Camera.main.WorldToViewportPoint(transform.position);
        Camera.main.GetComponent<RippleEffect>().Emit(playerPosition);
        //StartCoroutine(CameraDamage());

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
        if(inFloors)
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

    private void Movement()
    {
        //Se determina en que dirección del eje se esta moviendo el jugador mediante las teclas de flecha.
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        //En la variable privada direccion, se estipula la direccion del eje mediante las variables x,y previamente definidas con las teclas de flecha 
        direction = new Vector2(x, y);
        Vector2 directionRaw = new Vector2(xRaw, yRaw);

        //Se llama a la funcion de caminar
        Walk();
        //Se llama a la funcion atacar
        Attack(AttackDirection(MovementDirection, directionRaw));

        //Se valida si el personaje puede saltar solo cuando se aprieta esoacio y este está en el suelo.
        CoolerJump();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(inFloors)
            {
                animator.SetBool("jump", true);
                Jump();
            }
        }

        if(Input.GetKeyDown(KeyCode.X) && makindDash == false)//!makindDash
        {
            if(xRaw != 0 || yRaw !=0)
            {
                Dash(xRaw, yRaw);
            }
        }

        if(inFloors && !touchingFloors)
        {
            TouchFloor();
            touchingFloors = true;
        }

        if(!inFloors || touchingFloors)
        {
            touchingFloors = false;
        }

        //Se setea una nueva variable velocidad en 1 o -1 dependiendo la velocidad del RigidBody2D.
        float speed;
        if (rb.velocity.y > 0)
        {
            speed = 1;
        }
        else
        {
            speed = -1;
        }
        //Si el personaje no esta en el suelo, se asigna la nuevca variable de velocidad al animador. Si si lo esta, se finaliza el salto.
        if (!inFloors)
        {
            animator.SetFloat("verticalSpeed", speed);
        }
        else
        {
            if(speed == -1)
                EndJump();
        }
    }

    //Funcion exclusiva para finalizar la animación del salto
    public void EndJump()
    {
        animator.SetBool("jump", false);
    }

    private void CoolerJump()
    {
        if(rb.velocity.y < 0) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.0f - 1) * Time.deltaTime;
        }
    }

    private void Graps()
    {
        inFloors = Physics2D.OverlapCircle((Vector2)transform.position + below, collisionRadius, floorLayer);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForse;
    }

    private void Walk()
    {
        if (canMove && !makindDash)
        {
            rb.velocity = new Vector2(direction.x * movementSpeed, rb.velocity.y);

            if (direction != Vector2.zero)
            {
                if(!inFloors)
                {
                    animator.SetBool("jump", true);
                } else
                {
                    animator.SetBool("walk", true);
                }
                if(direction.x < 0 && transform.localScale.x > 0)
                {
                    MovementDirection = AttackDirection(Vector2.left, direction);
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                } else if(direction.x > 0 && transform.localScale.x < 0) 
                {
                    MovementDirection = AttackDirection(Vector2.right, direction);
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            } else
            {
                if(direction.y > 0 && direction.x == 0)
                {
                    MovementDirection = AttackDirection(direction, Vector2.up);
                }
                animator.SetBool("walk", false);
            }
        }
    }
        
}
