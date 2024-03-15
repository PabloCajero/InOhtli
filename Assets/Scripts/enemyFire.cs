using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class enemyFire : MonoBehaviour
{
    public Transform controladorDisparo;
    public float lineDistance;
    public LayerMask playerLayer;
    public bool playerInRange;
    public float timeBetween;
    public float timeLast;
    public GameObject enemyBullet;
    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        playerInRange = Physics2D.Raycast(controladorDisparo.position, transform.right, lineDistance, playerLayer);

        if (playerInRange)
        {
            if (Time.time > timeBetween + timeLast)
            {
                timeLast = Time.time;
                Invoke(nameof(Shot), waitTime);
            }
        }
    }

    private void Shot()
    {
        Instantiate(enemyBullet, controladorDisparo.position, controladorDisparo.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorDisparo.position, controladorDisparo.position + transform.right * lineDistance);
    }
}
