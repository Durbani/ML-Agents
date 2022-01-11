using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PushBoxAgent : Agent
{
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform movableBoxTransform;
    [SerializeField] private Transform targetPlatformTransform;

    public bool enableFinalLevel = false;

    private PushBoxMovement movementScript;

    public float currentState = 0f;

    public override void OnEpisodeBegin()
    {
        if (movementScript == null)
        {
            movementScript = GetComponent<PushBoxMovement>();
        }

        //Debug.Log(Academy.Instance.EnvironmentParameters.GetWithDefault("level_data", 0f));

        if (!enableFinalLevel)
        {
            currentState = Academy.Instance.EnvironmentParameters.GetWithDefault("level_data", 0f);

            if (currentState < 3.0f) //Stage 3
            {
                movementScript.ResetPlayer(new Vector3(0f, 0.501f, 7f));
            }

            if (currentState < 1.0f) //Stage 1
            {
                movableBoxTransform.localPosition = new Vector3(0.37f, 1f, -6f);
            }
            else if (currentState < 2.0f) //Stage 2
            {
                movableBoxTransform.localPosition = new Vector3(0.37f, 1f, 0f);
            }
            else if (currentState < 3.0f) //Stage 3
            {
                movableBoxTransform.localPosition = new Vector3(0.37f, 1f, 2.5f);
            }
            else if (currentState < 4.0f) //Stage 4
            {
                movableBoxTransform.localPosition = new Vector3(0.37f, 1f, 4f);
                movementScript.ResetPlayer(new Vector3(Random.Range(-9, 9), 0.501f, Random.Range(5, 9)));
            }
            else if (currentState < 5.0f) //Stage 5
            {
                movableBoxTransform.localPosition = new Vector3(Random.Range(-6.5f, 6.5f), 1f, Random.Range(-5.5f, 2));
                movementScript.ResetPlayer(new Vector3(Random.Range(-9, 9), 0.501f, Random.Range(5, 9)));
            }
            else //Stage 6
            {
                movableBoxTransform.localPosition = new Vector3(Random.Range(-6.5f, 6.5f), 1f, Random.Range(-5.5f, 2));
                movementScript.ResetPlayer(new Vector3(Random.Range(-9, 9), 0.501f, Random.Range(5, 9)));
                targetPlatformTransform.localPosition = new Vector3(Random.Range(-8.75f, 8.75f), 2, -8.75f);
            }
        }
        else
        {
            movableBoxTransform.localPosition = new Vector3(Random.Range(-6.5f, 6.5f), 1f, Random.Range(-5.5f, 2));
            movementScript.ResetPlayer(new Vector3(Random.Range(-9, 9), 0.501f, Random.Range(5, 9)));
            targetPlatformTransform.localPosition = new Vector3(Random.Range(-8.75f, 8.75f), 2, -8.75f);
        }

        //movableBoxTransform.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.forward);

        Vector3 coinVector = targetTransform.position - transform.position;
        sensor.AddObservation(coinVector.normalized);
        sensor.AddObservation(coinVector.magnitude);

        sensor.AddObservation(movableBoxTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.0005f);

        //Continues Movement
        float moveInput = actions.ContinuousActions[0];
        float turnInput = actions.ContinuousActions[1];
        bool jumpPressed = actions.DiscreteActions[0] == 1 ? true : false;

        movementScript.moveInput = moveInput;
        movementScript.turnInput = turnInput;
        movementScript.jumpPressed = jumpPressed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;

        //continues move
        continousActions[0] = Input.GetAxis("Vertical");
        continousActions[1] = Input.GetAxis("Horizontal");

        discreteActions[0] = Input.GetButton("Jump") ? 1 : 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            floorMeshRenderer.material = winMaterial;
            Debug.Log("Target found!");
            AddReward(3f);
            EndEpisode();
        }
        else if (other.CompareTag("Wall"))
        {
            floorMeshRenderer.material = loseMaterial;
            AddReward(-3f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.CompareTag("Movable"))
        //{
        //    Debug.Log("Push!!!!");
        //    collision.collider.gameObject.transform.position += transform.forward * 0.001f;
        //}
    } 
}
