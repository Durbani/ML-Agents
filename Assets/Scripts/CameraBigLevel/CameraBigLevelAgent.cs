using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CameraBigLevelAgent : Agent
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

    private CameraSensorMovement movementScript;

    public override void OnEpisodeBegin()
    {

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

        targetTransform.localPosition = new Vector3(Random.Range(-20f, 20f), 0.7f, Random.Range(-20f, 20f));
        movementScript.ResetPlayer(new Vector3(-13.6f + Academy.Instance.EnvironmentParameters.GetWithDefault("playerOffset", 0f), 2.3f, 0f));

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.0005f);

        float moveInput = actions.DiscreteActions[2] <= 1 ? actions.DiscreteActions[2] : -1;
        float turnInput = actions.DiscreteActions[3] <= 1 ? actions.DiscreteActions[3] : -1;
        int pressButton = actions.DiscreteActions[0];
        bool jumpPressed = actions.DiscreteActions[1] == 1 ? true : false;

        //Debug.Log(actions.DiscreteActions[1]);

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
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        discreteActions[2] = Input.GetAxis("Vertical") >= 0 ? Mathf.RoundToInt(Input.GetAxis("Vertical")) : 2;
        discreteActions[3] = Input.GetAxis("Horizontal") >= 0 ? Mathf.RoundToInt(Input.GetAxis("Horizontal")) : 2;

        discreteActions[0] = Input.GetKey(KeyCode.B) ? 1 : 0;
        discreteActions[1] = Input.GetButton("Jump") ? 1 : 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            floorMeshRenderer.material = winMaterial;
            Debug.Log("Target found!");
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
