using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class RaycastBigLevelAgent : Agent
{
    [SerializeField] private Transform targetBoxTransform;
    [SerializeField] private Transform buttonCheck;
    [SerializeField] private Transform buttonTransform;
    [SerializeField] private Transform door1;
    [SerializeField] private Transform door2;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private float buttonCheckDistance;
    [SerializeField] private LayerMask buttonLayerMask;
    [SerializeField] private GameObject monumentObject;
    [SerializeField] private GameObject irrgartenObject;

    [SerializeField] private Vector3[] possiblePositionsMonument;
    [SerializeField] private Vector3[] possiblePositionsIrrgarten;

    private bool buttonPressed;

    private CameraSensorMovement movementScript;

    public float currentState = 0f;

    public override void OnEpisodeBegin()
    {
        //General
        if (door1 != null)
        {
            door1.GetComponent<MeshRenderer>().enabled = true;
            door1.GetComponent<BoxCollider>().enabled = true;
            door2.GetComponent<MeshRenderer>().enabled = true;
            door2.GetComponent<BoxCollider>().enabled = true;
        }

        buttonPressed = false;

        if (movementScript == null)
        {
            movementScript = GetComponent<CameraSensorMovement>();
        }

        movementScript.ResetPlayer(new Vector3(Random.Range(-18.5f, -10.5f), 0.501f, Random.Range(-48f, 48f)));

        currentState = Academy.Instance.EnvironmentParameters.GetWithDefault("level_data", 0f);

        if (currentState < 1.0f) //Stage 1
        {
            
        }
        else if (currentState < 2.0f) //Stage 2
        {
            //Button
            buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 0.2f), 1.25f, Random.Range(-48f, 48f));
        }
        else if (currentState < 3.0f) //Stage 3
        {
            //Box with Target
            targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));

            //Button
            buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 0.2f), 1.25f, Random.Range(-48f, 48f));
        }
        else if (currentState < 4.0f) //Stage 4
        {
            //Box with Target
            targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
            targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

            //Button
            buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 0.2f), 1.25f, Random.Range(-48f, 48f));
        }
        else if (currentState < 5.0f) //Stage 5
        {
            //Box with Target
            targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
            targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

            //Button
            buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 20f), 1.25f, Random.Range(-48f, 48f));
        }
        else if (currentState < 6.0f) //Stage 6
        {
            //Box with Target
            targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
            targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

            //Button
            buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 47f), 1.25f, Random.Range(-48f, 48f));
        }
        else if (currentState < 7.0f) //Stage 7
        {
            //Box with Target
            targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
            targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

            //Enable Obstacles
            monumentObject.SetActive(true);
            irrgartenObject.SetActive(true);

            //Randomize button position
            float randomPosition = Random.Range(1f, 4f);

            if (randomPosition < 2f)
            {
                //Button
                buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 9f), 1.25f, Random.Range(-48f, 48f));
            }
            else if (randomPosition < 3f) //Button on monument
            {
                buttonTransform.localPosition = possiblePositionsMonument[Mathf.RoundToInt(Random.Range(0f, possiblePositionsMonument.Length - 1))];
            }
            else //Button in irrgarten
            {
                buttonTransform.localPosition = possiblePositionsIrrgarten[Mathf.RoundToInt(Random.Range(0f, possiblePositionsIrrgarten.Length - 1))];
            }
        }
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
            if (buttonPressed == false)
            {
                AddReward(0.5f);
                buttonPressed = true;
                if (door1 != null)
                {
                    door1.GetComponent<MeshRenderer>().enabled = false;
                    door1.GetComponent<BoxCollider>().enabled = false;
                    door2.GetComponent<MeshRenderer>().enabled = false;
                    door2.GetComponent<BoxCollider>().enabled = false;
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
