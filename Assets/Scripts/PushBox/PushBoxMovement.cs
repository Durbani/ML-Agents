using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBoxMovement : MonoBehaviour
{
    public float turnInput;
    public float moveInput;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float jumpHeight = 3f;

    public bool jumpPressed;
    private bool isOnGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform movableBox;
    [SerializeField] private float groundDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;

    private float gravity = -9.81f;
    private Vector3 velocity;
    private CharacterController characterController;

    public float velocityMultiply = 0.003f;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        Vector3 move = transform.forward * Mathf.Clamp(moveInput, -1f, 1f);
        if(!isOnGround)
        {
            move *= 0.7f;
        }
        characterController.Move(move * moveSpeed * Time.deltaTime);

        if (turnInput != 0f)
        {
            float angle = Mathf.Clamp(turnInput, -1f, 1f) * turnSpeed;
            transform.Rotate(Vector3.up, Time.deltaTime * angle);
        }


        //Jump
        if (jumpPressed && isOnGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        movableBox.GetComponent<Rigidbody>().velocity *= velocityMultiply;
    }

    public void ResetPlayer(Vector3 newPosition)
    {
        characterController.enabled = false;
        transform.localPosition = newPosition;
        characterController.enabled = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Movable"))
        {
            if(!(transform.position.y >= 2.5f))
            {
                //Debug.Log("Push!");
                //if (hit.collider.gameObject.GetComponent<Rigidbody>() == null) return;
                //var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
                //hit.collider.attachedRigidbody.velocity = pushDir * 10;

                Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();
                hit.collider.attachedRigidbody.AddForceAtPosition(forceDirection * 1, transform.position, ForceMode.Impulse);
            }
        }
    }
}
