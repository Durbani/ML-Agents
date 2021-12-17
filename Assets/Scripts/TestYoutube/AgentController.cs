using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class AgentController : Agent
{
    [Tooltip("The platform to be moved around")]
    public GameObject platform;
    public GameObject boxWithDoor;

    private Vector3 startPosition;
    private CharacterControllerYoutube characterController;
    new private Rigidbody rigidbody;

    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private Transform buttonCheck;
    [SerializeField] private float buttonCheckDistance;
    [SerializeField] private LayerMask buttonLayerMask;
    [SerializeField] private Transform door1;
    [SerializeField] private Transform door2;
    [SerializeField] private Transform button;

    private int buttonPressedCount;
    private bool buttonPressed;

    public override void Initialize()
    {
        startPosition = transform.localPosition;
        characterController = GetComponent<CharacterControllerYoutube>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset agent position, rotation
        transform.localPosition = startPosition;
        transform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        rigidbody.velocity = Vector3.zero;

        buttonPressedCount = 0;
        buttonPressed = false;

        door1.GetComponent<MeshRenderer>().enabled = true;
        door1.GetComponent<BoxCollider>().enabled = true;
        door2.GetComponent<MeshRenderer>().enabled = true;
        door2.GetComponent<BoxCollider>().enabled = true;

        boxWithDoor.transform.localPosition = new Vector3(-13, 0, Random.Range(-16, 10));
        button.localPosition = new Vector3(-1, 0.75f, Random.Range(-16, 10));

        // Reset platform position (5 meters away from the agent in a random direction)
        //platform.transform.localPosition = startPosition + Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * 10f;
        //platform.transform.localPosition = new Vector3(platform.transform.localPosition.x, 0.75f, platform.transform.localPosition.z);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Read input values and round them. GetAxisRaw works better in this case
        // because of the DecisionRequester, which only gets new decisions periodically.
        int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        bool jump = Input.GetKey(KeyCode.Space);

        // Convert the actions to Discrete choices (0, 1, 2)
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = vertical >= 0 ? vertical : 2;
        actions[1] = horizontal >= 0 ? horizontal : 2;
        actions[2] = jump ? 1 : 0;
        actions[3] = Input.GetKey(KeyCode.B) ? 1 : 0;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.0005f);

        // Punish and end episode if the agent strays too far
        if (Vector3.Distance(startPosition, transform.localPosition) > 40f)
        {
            AddReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }

        // Convert actions from Discrete (0, 1, 2) to expected input values (-1, 0, +1)
        // of the character controller
        float vertical = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
        float horizontal = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;
        bool jump = actions.DiscreteActions[2] > 0;
        int pressButton = actions.DiscreteActions[3];

        characterController.ForwardInput = vertical;
        characterController.TurnInput = horizontal;
        characterController.JumpInput = jump;

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
                door1.GetComponent<MeshRenderer>().enabled = false;
                door1.GetComponent<BoxCollider>().enabled = false;
                door2.GetComponent<MeshRenderer>().enabled = false;
                door2.GetComponent<BoxCollider>().enabled = false;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the other object is a collectible, reward and end episode
        if (other.tag == "Collectible")
        {
            AddReward(5f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
    }
}
