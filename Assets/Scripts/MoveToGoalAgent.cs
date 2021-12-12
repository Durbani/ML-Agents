using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private float moveSpeed = 4f;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-1.5f, 1.5f), 0.51f, Random.Range(-4f, 0));
        targetTransform.localPosition = new Vector3(Random.Range(-1.5f, 1.5f), 0.5f, Random.Range(1f, 4));
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
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxis("Vertical");
        continousActions[1] = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            floorMeshRenderer.material = winMaterial;
            AddReward(1f);
        }
        else if (other.CompareTag("Wall"))
        {
            floorMeshRenderer.material = loseMaterial;
            AddReward(-1f);
        }
        EndEpisode();
    }
}
