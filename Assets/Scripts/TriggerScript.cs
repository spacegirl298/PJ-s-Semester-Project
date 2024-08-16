using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerScript : MonoBehaviour
{
    public GameObject closedDoor;
    public GameObject openDoor;

    public GameObject Roomba;
    public float speed = 1.4f;
    public float detectionRadius = 5f;

    public LayerMask playerLayer; //which layer player is

    public Transform pointA;   //points the roomba will move between
    public Transform pointB;

    private GameObject player;
    private bool isChasingPlayer = false;
    private bool isOff = false;
    private Transform currentTarget;

    
    //public int jumpCount = 0;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentTarget = pointB;
        closedDoor.SetActive(true);
        openDoor.SetActive(false);
    }

    private void OnTriggerEnter(Collider collided)
    {
        if (collided.tag == "Player")
        {
            TurnOffRoomba();
        }

    }

    private void Update()
    {
        if (isOff)
        {
            TurnOffRoomba();

        }
        else
        {
            DetectPlayer();
            if (isChasingPlayer)
            {
                ChasePlayer();
            }
            else
            {
                MoveAround();
            }
        }


        if (!isOff)
        {
            DetectPlayer();
            if (isChasingPlayer)
            {
                ChasePlayer();
            }
            else
            {
                MoveAround();
            }
        }
        else
        {
            TurnOffRoomba();
        }
    }

    private void MoveAround()
    {

        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);


        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
            Debug.Log("Roomba is Moving");
        }
    }

    private void DetectPlayer()
    {

        if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
        {
            isChasingPlayer = true;
        }
        else
        {
            isChasingPlayer = false;
        }
    }

    private void ChasePlayer() //follows player obnly along x n y axis and doesnt go up when player jumps (had that issue initially)
    {

        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        transform.rotation = Quaternion.Euler(new Vector3(-89.98f, 0f, 0f));

        Debug.Log("Detection");
    }

    private void TurnOffRoomba()
    {
        closedDoor.SetActive(false);
        openDoor.SetActive(true);

        isOff = true;
        speed = 0f;
        transform.rotation = Quaternion.Euler(new Vector3(-89.98f, 0f, 0f)); //https://stackoverflow.com/questions/34054380/rotate-item-90-degree-rotations#:~:text=You%20should%20look%20at%20Quaternion%2C%20transform.rotation%20and%20Quaternion.Euler%28%29.,by%2090%C2%B0%20in%20the%20x-axis.%20%E2%80%93%20Maximilian%20Gerhardt
       
        Debug.Log("Roomba off.");
    }



}