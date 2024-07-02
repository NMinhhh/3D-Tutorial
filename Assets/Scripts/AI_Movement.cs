using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private Vector2 walkTimeRan;
    private float walkCounter;
    [SerializeField] private Vector2 waitTimeRan;
    private float waitCounter;

    private Vector3 stopPosition;
    private bool isWalking;

    private int direction;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        WalkTimeRandom();
        ChooseDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            anim.SetBool("isRunning", true);

            walkCounter -= Time.deltaTime;
            switch(direction)
            {
                case 0:
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(0, 90, 0);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    break;
                case 2:
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    break;
                case 3:
                    transform.localRotation = Quaternion.Euler(0, -90, 0);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    break;
            }
            if(walkCounter <= 0)
            {
                stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                isWalking = false;

                transform.position = stopPosition;

                anim.SetBool("isRunning", false);

                WaitTimeRandom();
            }
        }
        else
        {
            waitCounter -= Time.deltaTime; 
            if(waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }

    void ChooseDirection()
    {
        direction = Random.Range(0, 4);

        WalkTimeRandom();

        isWalking = true;
    }

    void WalkTimeRandom()
    {
        walkCounter = Random.Range(walkTimeRan.x, walkTimeRan.y);
    }

    void WaitTimeRandom()
    {
        waitCounter = Random.Range(waitTimeRan.x, waitTimeRan.y);
    }
}
