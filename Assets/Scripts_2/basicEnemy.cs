using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class basicEnemy : MonoBehaviour
{
    

    public int routine, direction;
    public float chronometer;
    public Animator animator;
    public float speed_walk;
    public float speed_run;
    public GameObject target;
    public bool attack;

    public Rigidbody2D rb2D;
    public float movementSpeed;
    public LayerMask belowLayer;
    public LayerMask frontLayer;
    public float belowDistance;
    public float frontDistance;
    public Transform belowController;
    public Transform frontController;
    public bool belowInfo;
    public bool frontInfo;

    private bool LookToRight = true;

    

    // Start is called before the first frame update
    void Start()
    {
        /*rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player");*///idle1
        //Behavior();
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.velocity = new Vector2(movementSpeed, rb2D.velocity.y);

        frontInfo = Physics2D.Raycast(frontController.position, transform.right, frontDistance, frontLayer);
        belowInfo = Physics2D.Raycast(belowController.position, transform.up * -1, belowDistance, belowLayer);

        if(frontInfo || !belowInfo)
        {
            //Girar
            Girar();
        }
    }

    private void Girar()
    {
        LookToRight = !LookToRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        movementSpeed *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(belowController.transform.position, belowController.transform.position + transform.up * -1 * belowDistance);
        Gizmos.DrawLine(frontController.transform.position, frontController.transform.position + transform.right * frontDistance);
        
    }


    /*public void Behavior()
    {
        animator.SetBool("run", false);
        chronometer += 1 * Time.deltaTime;
        if (chronometer >= 4)
        {
            routine = Random.RandomRange(0, 2);
            chronometer = 0;
        }

        switch (routine)
        {
            case 0:
                animator.SetBool("Walk", false);
                break;
            case 1:
                direction = Random.Range(0, 2);
                routine++;
                break;
            case 2:
                switch (direction)
                {
                    case 0:
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        transform.Translate(Vector3.right * speed_walk * Time.deltaTime);
                        break;
                    case 1:
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        transform.Translate(Vector3.right * speed_walk * Time.deltaTime);
                        break;
                }
                animator.SetBool("walk", true);
                break;
        }
    }*/






}

