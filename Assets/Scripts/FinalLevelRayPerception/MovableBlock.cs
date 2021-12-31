using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlock : MonoBehaviour
{
    [SerializeField] private FinalLevelRayPerceptionAgent agentScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlockTrigger"))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ
                    | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            agentScript.AddRewardForPushingBlock();
        }
        if (other.CompareTag("Wall")) {
            Debug.Log("End Episode because block was pushed against wall...!");
            agentScript.PushedBlockAgainstWall();
        }
    }
}
