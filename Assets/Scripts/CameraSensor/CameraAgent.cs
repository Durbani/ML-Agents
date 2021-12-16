using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CameraAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform buttonCheck;
    [SerializeField] private Transform buttonTransform;
    [SerializeField] private Transform door;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private float buttonCheckDistance;
    [SerializeField] private LayerMask buttonLayerMask;

    private int buttonPressedCount;
    private bool buttonPressed;

    //JUMP
    private CameraSensorMovement movementScript;

    public override void OnEpisodeBegin()
    {
        //if(buttonTransform != null)
        //{
        //    buttonTransform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), 0.5f, Random.Range(-4.5f, -6.5f));
        //}

        if (door != null)
        {
            door.GetComponent<MeshRenderer>().enabled = true;
            door.GetComponent<BoxCollider>().enabled = true;
        }

        buttonPressedCount = 0;
        buttonPressed = false;

        if (movementScript == null)
        {
            movementScript = GetComponent<CameraSensorMovement>();
        }

        //JUMP
        //targetTransform.localPosition = new Vector3(Random.Range(-9f, 9f), 0.5f, Random.Range(1.5f, 9f));
        //movementScript.ResetPlayer(new Vector3(Random.Range(-9f, 9f), 0.51f, Random.Range(-9f, -1.5f)));

        //BUTTON
        //targetTransform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), 0.5f, Random.Range(2f, 6f));
        //movementScript.ResetPlayer(new Vector3(Random.Range(-2.5f, 2.5f), 0.51f, Random.Range(-1f, -4f)));

        //Button+Jump
        movementScript.ResetPlayer(new Vector3(0f, 0.501f, 0f));

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);

        Vector3 disctanceVector = targetTransform.position - transform.position;
        sensor.AddObservation(disctanceVector.magnitude);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (Vector3.Distance(targetTransform.position, transform.position) > 30f)
        {
            AddReward(-1f);
            EndEpisode();
        }

        AddReward(-0.0005f);

        float moveInput = actions.ContinuousActions[0];
        float turnInput = actions.ContinuousActions[1];
        int pressButton = actions.DiscreteActions[0];
        bool jumpPressed = actions.DiscreteActions[1] == 1 ? true : false;

        Debug.Log(actions.DiscreteActions[1]);

        movementScript.moveInput = moveInput;
        movementScript.turnInput = turnInput;
        movementScript.jumpPressed = jumpPressed;

        if (Physics.CheckSphere(buttonCheck.position, buttonCheckDistance, buttonLayerMask) && pressButton == 1)
        {
            buttonPressedCount++;
            if (buttonPressedCount > 1)
            {
                //AddReward(-0.5f);
            }
            if (buttonPressed == false)
            {
                AddReward(0.5f);
                buttonPressed = true;
                if (door != null)
                {
                    door.GetComponent<MeshRenderer>().enabled = false;
                    door.GetComponent<BoxCollider>().enabled = false;
                }
            }

        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        continousActions[0] = Input.GetAxis("Vertical");
        continousActions[1] = Input.GetAxis("Horizontal");

        discreteActions[0] = Input.GetKey(KeyCode.B) ? 1 : 0;
        discreteActions[1] = Input.GetButton("Jump") ? 1 : 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            floorMeshRenderer.material = winMaterial;
            AddReward(3f);
        }
        else if (other.CompareTag("Wall"))
        {
            floorMeshRenderer.material = loseMaterial;
            AddReward(-3f);
        }
        EndEpisode();
    }
}
