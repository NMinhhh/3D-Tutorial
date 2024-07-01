using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [SerializeField] private float speed = 8;
    [SerializeField] private float gravity = -9.81f * 2;
    [SerializeField] private float jumpHeight = 5;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;

    private Vector3 velocity;

    private bool isGrounded;

    [SerializeField] private LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * xInput + transform.forward * zInput;

        characterController.Move(move * speed * Time.deltaTime);

        if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
