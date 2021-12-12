using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayerMovement : MonoBehaviour
{

    public float moveX;
    public float moveZ;

    public float moveSpeed = 10f;
    public float jumpHeight = 3f;

    public bool jumpPressed;
    private bool isOnGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;

    private float gravity = -9.81f;
    private Vector3 velocity;
    private CharacterController characterController;
    private JumpAgent jumpAgentScript;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        jumpAgentScript = GetComponent<JumpAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Jump
        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isOnGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Movement
        //transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * moveSpeed * Time.deltaTime);


        //Jump
        if (jumpPressed && isOnGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpAgentScript.SubtractRewardForJump();
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void ResetPlayer(Vector3 newPosition)
    {
        characterController.enabled = false;
        transform.localPosition = newPosition;
        characterController.enabled = true;
    }
}
