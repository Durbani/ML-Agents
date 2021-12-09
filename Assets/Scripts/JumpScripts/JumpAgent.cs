using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class JumpAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    private JumpPlayerMovement movementScript;

    public override void OnEpisodeBegin()
    {
        if (movementScript == null)
        {
            movementScript = GetComponent<JumpPlayerMovement>();
        }
        targetTransform.localPosition = new Vector3(Random.Range(-9f, 9f), 0.5f, Random.Range(1.5f, 9f));
        movementScript.ResetPlayer(new Vector3(Random.Range(-9f, 9f), 0.51f, Random.Range(-9f, -1.5f)));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Movement
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        
        //Jump
        bool jumpPressed = actions.DiscreteActions[0] == 1 ? true : false;

        movementScript.moveX = moveX;
        movementScript.moveZ = moveZ;
        movementScript.jumpPressed = jumpPressed;
    }

    public void SubtractRewardForJump()
    {
        AddReward(-0.1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = -Input.GetAxis("Vertical");
        continousActions[1] = Input.GetAxis("Horizontal");

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetButton("Jump") ? 1 : 0;
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
            SetReward(-1f);
        }
        EndEpisode();
    }
}
