using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ButtonPressToGoal : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform wall;
    [SerializeField] private Transform buttonCheck;
    [SerializeField] private Transform buttonTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float buttonCheckDistance;
    [SerializeField] private LayerMask buttonLayerMask;

    private int buttonPressedCount;

    private bool buttonPressed;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), 0.51f, Random.Range(-1f, -4f));
        targetTransform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), 0.5f, Random.Range(2f, 6f));
        buttonTransform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), 0.5f, Random.Range(-4.5f, -6.5f));

        //transform.localPosition = new Vector3(0, 0.51f, -2.5f);
        //targetTransform.localPosition = new Vector3(0, 0.5f, 5f);

        wall.GetComponent<MeshRenderer>().enabled = true;
        wall.GetComponent<BoxCollider>().enabled = true;

        buttonPressedCount = 0;
        buttonPressed = false;
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
        AddReward(-0.0005f);

        float moveX = actions.ContinuousActions[0] * -1;
        float moveZ = actions.ContinuousActions[1];
        int pressButton = actions.DiscreteActions[0];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

        if (Physics.CheckSphere(buttonCheck.position, buttonCheckDistance, buttonLayerMask) && pressButton == 1)
        {
            buttonPressedCount++;
            if (buttonPressedCount > 1)
            {
                AddReward(-0.5f);
            }
            if (buttonPressed == false)
            {
                buttonPressed = true;
                wall.GetComponent<MeshRenderer>().enabled = false;
                wall.GetComponent<BoxCollider>().enabled = false;
            }

        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        
        continousActions[0] = Input.GetAxis("Vertical");
        continousActions[1] = Input.GetAxis("Horizontal");

        discreteActions[0] = 0;
        if(Input.GetKey(KeyCode.Space))
        {
            discreteActions[0] = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            Debug.Log("Hit target: +5");
            floorMeshRenderer.material = winMaterial;
            AddReward(5f);
        }
        else if (other.CompareTag("Wall"))
        {
            Debug.Log("Hit wall: -5");
            floorMeshRenderer.material = loseMaterial;
            AddReward(-5f);
        }
        Debug.Log("Button pressed: " + buttonPressedCount);
        EndEpisode();
    }
}
