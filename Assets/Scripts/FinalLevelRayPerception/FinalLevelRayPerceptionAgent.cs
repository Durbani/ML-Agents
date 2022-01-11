using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;

public class FinalLevelRayPerceptionAgent : Agent
{
    [SerializeField] private Transform targetBoxTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform buttonCheck;
    [SerializeField] private Transform buttonTransform;
    [SerializeField] private Transform door;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private float buttonCheckDistance;
    [SerializeField] private LayerMask buttonLayerMask;
    [SerializeField] private GameObject monumentObject;
    [SerializeField] private GameObject irrgartenObject;
    [SerializeField] private GameObject jumpSectionObject;
    [SerializeField] private Transform movableBlockTransform;
    [SerializeField] private Transform movablePlatformTransform;

    [SerializeField] private GameObject outsideWall1;
    [SerializeField] private GameObject outsideWall2;
    [SerializeField] private GameObject outsideWall3;
    [SerializeField] private GameObject outsideWall4;

    [SerializeField] private GameObject outsideWallNextStage1;
    [SerializeField] private GameObject outsideWallNextStage2;
    [SerializeField] private GameObject outsideWallNextStage3;
    [SerializeField] private GameObject outsideWallNextStage4;


    [SerializeField] private Vector3[] possiblePositionsMonument;
    [SerializeField] private Vector3[] possiblePositionsIrrgarten;

    [SerializeField] private GameObject jumpsectionWall;

    public bool enableFinalLevel = false;

    private bool buttonPressed;

    private PushBoxMovement movementScript;

    public float currentState = 0f;

    private bool buttonOnJumpSection = false;
    private bool rewardIsAlreadyTaken = false;
    private bool pushBlockRewardAlreadyTaken = false;
    [SerializeField] private Transform agentHandTransform;

    public override void OnEpisodeBegin()
    {
        //General
        if (door != null)
        {
            door.GetComponent<MeshRenderer>().enabled = true;
            door.GetComponent<BoxCollider>().enabled = true;
        }

        buttonPressed = false;
        buttonTransform.gameObject.SetActive(true);

        targetBoxTransform.gameObject.SetActive(false);

        if (movementScript == null)
        {
            movementScript = GetComponent<PushBoxMovement>();
        }

        buttonOnJumpSection = false;
        rewardIsAlreadyTaken = false;
        pushBlockRewardAlreadyTaken = false;

        jumpsectionWall.SetActive(true);
        movementScript.SetLiftStatus(true);

        //Debug.Log(Academy.Instance.EnvironmentParameters.GetWithDefault("level_data", 0f));

        if (!enableFinalLevel)
        {

            //currentState = Academy.Instance.EnvironmentParameters.GetWithDefault("level_data", 0f);

            if (currentState < 3.0f)
            {
                movementScript.ResetPlayer(new Vector3(-10.3f, 0.501f, -0.4f));
            }
            else
            {
                movementScript.ResetPlayer(new Vector3(Random.Range(-18.5f, -10.5f), 0.501f, Random.Range(-48f, 48f)));
            }

            if (currentState > 3.0f) 
            {
                if (outsideWall1.activeSelf)
                {
                    outsideWall1.SetActive(false);
                    outsideWall2.SetActive(false);
                    outsideWall3.SetActive(false);
                    outsideWall4.SetActive(false);
                }

                if (outsideWallNextStage1.activeSelf)
                {
                    outsideWallNextStage1.SetActive(false);
                    outsideWallNextStage2.SetActive(false);
                    outsideWallNextStage3.SetActive(false);
                    outsideWallNextStage4.SetActive(false);
                }
            }

            if (currentState < 1.0f) //Stage 1
            {
                //Box with Target
                targetBoxTransform.localPosition = new Vector3(-26.7f, 0f, -0.2f);

                //Button
                buttonTransform.localPosition = new Vector3(-16.5f, 1.334f, -4.3f);
            }
            else if (currentState < 2.0f) //Stage 2
            {
                //Box with Target
                targetBoxTransform.localPosition = new Vector3(-26.7f, 0f, -0.2f);

                //Button
                buttonTransform.localPosition = new Vector3(Random.Range(-18f, -14f), 1.25f, Random.Range(-13f, 13f));
            }
            else if (currentState < 3.0f) //Stage 3
            {
                if (outsideWall1.activeSelf)
                {
                    outsideWall1.SetActive(false);
                    outsideWall2.SetActive(false);
                    outsideWall3.SetActive(false);
                    outsideWall4.SetActive(false);
                }

                //Box with Target
                targetBoxTransform.localPosition = new Vector3(-26.7f, 0f, -0.2f);

                //Button
                buttonTransform.localPosition = new Vector3(Random.Range(-18f, -14f), 1.25f, Random.Range(-24f, 24f));
            }
            else if (currentState < 4.0f) //Stage 4
            {
                if (outsideWallNextStage1.activeSelf)
                {
                    outsideWallNextStage1.SetActive(false);
                    outsideWallNextStage2.SetActive(false);
                    outsideWallNextStage3.SetActive(false);
                    outsideWallNextStage4.SetActive(false);
                }
                //Box with Target
                targetBoxTransform.localPosition = new Vector3(-26.7f, 0f, -0.2f);

                //Button
                buttonTransform.localPosition = new Vector3(Random.Range(-18.5f, -11f), 1.25f, Random.Range(-48f, 48f));
            }
            else if (currentState < 5.0f) //Stage 5
            {
                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));

                //Button
                buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 0.2f), 1.25f, Random.Range(-48f, 48f));
            }
            else if (currentState < 6.0f) //Stage 6
            {
                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Button
                buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 0.2f), 1.25f, Random.Range(-48f, 48f));
            }
            else if (currentState < 7.0f) //Stage 7
            {
                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Button
                buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 20f), 1.25f, Random.Range(-48f, 48f));
            }
            else if (currentState < 8.0f) //Stage 8
            {
                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Button
                buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 47f), 1.25f, Random.Range(-48f, 48f));
            }
            else if (currentState < 9.0f)
            {
                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Enable Obstacles
                monumentObject.SetActive(true);
                irrgartenObject.SetActive(true);

                for (int i = 0; i < irrgartenObject.transform.childCount; i++)
                {
                    Transform wall = irrgartenObject.transform.GetChild(i);
                    if (wall.gameObject.tag.Equals("Untagged"))
                    {
                        break;
                    }
                    wall.gameObject.tag = "Untagged";
                    wall.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                }

                //Randomize button position
                float randomPosition = Random.Range(1f, 4f);

                if (randomPosition < 2f)
                {
                    //Button
                    buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 9f), 1.25f, Random.Range(1f, 48f));
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
            else if (currentState < 10.0f) //new push section
            {
                jumpSectionObject.SetActive(true);

                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Plattform und Pushbox
                movablePlatformTransform.localPosition = new Vector3(Random.Range(-11f, 2f), 3f, Random.Range(-40f, -10f));
                movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));

                //Enable Obstacles
                monumentObject.SetActive(true);
                irrgartenObject.SetActive(true);

                for (int i = 0; i < irrgartenObject.transform.childCount; i++)
                {
                    Transform wall = irrgartenObject.transform.GetChild(i);
                    if (wall.gameObject.tag.Equals("Untagged"))
                    {
                        break;
                    }
                    wall.gameObject.tag = "Untagged";
                    wall.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                }

                //Randomize button position
                float randomPosition = Random.Range(0f, 6f);

                if (randomPosition < 3f)
                {
                    buttonOnJumpSection = true;

                    //Box with Target
                    targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                    targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                    //Plattform und Pushbox
                    movablePlatformTransform.localPosition = new Vector3(-6.08f, 3f, -37.38f);
                    movableBlockTransform.localPosition = new Vector3(-5.25f, 0.85f, -27.02f);

                    float platformX = movablePlatformTransform.localPosition.x;
                    float platformZ = movablePlatformTransform.localPosition.z;
                    float platformScaleX = movablePlatformTransform.localScale.x;
                    float platformScaleZ = movablePlatformTransform.localScale.z;

                    //Button new position
                    buttonTransform.localPosition = new Vector3(Random.Range(platformX - platformScaleX / 2 + 0.5f, platformX + platformScaleX / 2 - 0.5f), 5.83f, Random.Range(platformZ - platformScaleZ / 2 + 0.5f, platformZ + platformScaleZ / 2 - 0.5f));
                }
                else if (randomPosition < 4f)
                {
                    //Button
                    buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 9f), 1.25f, Random.Range(1f, 48f));
                }
                else if (randomPosition < 5f) //Button on monument
                {
                    buttonTransform.localPosition = possiblePositionsMonument[Mathf.RoundToInt(Random.Range(0f, possiblePositionsMonument.Length - 1))];
                }
                else //Button in irrgarten
                {
                    buttonTransform.localPosition = possiblePositionsIrrgarten[Mathf.RoundToInt(Random.Range(0f, possiblePositionsIrrgarten.Length - 1))];
                }
            }
            else if (currentState < 11.0f) //new push section
            {
                jumpSectionObject.SetActive(true);

                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Plattform und Pushbox
                movablePlatformTransform.localPosition = new Vector3(Random.Range(-11f, 2f), 3f, Random.Range(-40f, -10f));
                movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));

                //Enable Obstacles
                monumentObject.SetActive(true);
                irrgartenObject.SetActive(true);

                for (int i = 0; i < irrgartenObject.transform.childCount; i++)
                {
                    Transform wall = irrgartenObject.transform.GetChild(i);
                    if (wall.gameObject.tag.Equals("Untagged"))
                    {
                        break;
                    }
                    wall.gameObject.tag = "Untagged";
                    wall.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                }

                //Randomize button position
                float randomPosition = Random.Range(0f, 6f);

                if (randomPosition < 3f)
                {
                    buttonOnJumpSection = true;

                    //Box with Target
                    targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                    targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                    //Plattform und Pushbox
                    movablePlatformTransform.localPosition = new Vector3(-6.08f, 3f, -37.38f);
                    movableBlockTransform.localPosition = new Vector3(-5.25f, 0.85f, -20.1f);

                    float platformX = movablePlatformTransform.localPosition.x;
                    float platformZ = movablePlatformTransform.localPosition.z;
                    float platformScaleX = movablePlatformTransform.localScale.x;
                    float platformScaleZ = movablePlatformTransform.localScale.z;

                    //Button new position
                    buttonTransform.localPosition = new Vector3(Random.Range(platformX - platformScaleX / 2 + 0.5f, platformX + platformScaleX / 2 - 0.5f), 5.83f, Random.Range(platformZ - platformScaleZ / 2 + 0.5f, platformZ + platformScaleZ / 2 - 0.5f));
                }
                else if (randomPosition < 4f)
                {
                    //Button
                    buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 9f), 1.25f, Random.Range(1f, 48f));
                }
                else if (randomPosition < 5f) //Button on monument
                {
                    buttonTransform.localPosition = possiblePositionsMonument[Mathf.RoundToInt(Random.Range(0f, possiblePositionsMonument.Length - 1))];
                }
                else //Button in irrgarten
                {
                    buttonTransform.localPosition = possiblePositionsIrrgarten[Mathf.RoundToInt(Random.Range(0f, possiblePositionsIrrgarten.Length - 1))];
                }
            }
            else if (currentState < 12.0f) //new push section
            {
                jumpSectionObject.SetActive(true);

                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Plattform und Pushbox
                movablePlatformTransform.localPosition = new Vector3(Random.Range(-11f, 2f), 3f, Random.Range(-40f, -10f));
                movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));

                //Enable Obstacles
                monumentObject.SetActive(true);
                irrgartenObject.SetActive(true);

                for (int i = 0; i < irrgartenObject.transform.childCount; i++)
                {
                    Transform wall = irrgartenObject.transform.GetChild(i);
                    if (wall.gameObject.tag.Equals("Untagged"))
                    {
                        break;
                    }
                    wall.gameObject.tag = "Untagged";
                    wall.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                }

                //Randomize button position
                float randomPosition = Random.Range(0f, 6f);

                if (randomPosition < 3f)
                {
                    buttonOnJumpSection = true;

                    //Box with Target
                    targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                    targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                    //Plattform und Pushbox
                    movablePlatformTransform.localPosition = new Vector3(-6.08f, 3f, -37.38f);
                    movableBlockTransform.localPosition = new Vector3(-5.25f, 0.85f, -12.1f);

                    float platformX = movablePlatformTransform.localPosition.x;
                    float platformZ = movablePlatformTransform.localPosition.z;
                    float platformScaleX = movablePlatformTransform.localScale.x;
                    float platformScaleZ = movablePlatformTransform.localScale.z;

                    //Button new position
                    buttonTransform.localPosition = new Vector3(Random.Range(platformX - platformScaleX / 2 + 0.5f, platformX + platformScaleX / 2 - 0.5f), 5.83f, Random.Range(platformZ - platformScaleZ / 2 + 0.5f, platformZ + platformScaleZ / 2 - 0.5f));
                }
                else if (randomPosition < 4f)
                {
                    //Button
                    buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 9f), 1.25f, Random.Range(1f, 48f));
                }
                else if (randomPosition < 5f) //Button on monument
                {
                    buttonTransform.localPosition = possiblePositionsMonument[Mathf.RoundToInt(Random.Range(0f, possiblePositionsMonument.Length - 1))];
                }
                else //Button in irrgarten
                {
                    buttonTransform.localPosition = possiblePositionsIrrgarten[Mathf.RoundToInt(Random.Range(0f, possiblePositionsIrrgarten.Length - 1))];
                }
            }
            else if (currentState < 13.0f) //new push section
            {
                jumpSectionObject.SetActive(true);

                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Plattform und Pushbox
                movablePlatformTransform.localPosition = new Vector3(Random.Range(-11f, 2f), 3f, Random.Range(-40f, -10f));
                movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));

                //Enable Obstacles
                monumentObject.SetActive(true);
                irrgartenObject.SetActive(true);

                for (int i = 0; i < irrgartenObject.transform.childCount; i++)
                {
                    Transform wall = irrgartenObject.transform.GetChild(i);
                    if (wall.gameObject.tag.Equals("Untagged"))
                    {
                        break;
                    }
                    wall.gameObject.tag = "Untagged";
                    wall.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                }

                //Randomize button position
                float randomPosition = Random.Range(0f, 6f);

                if (randomPosition < 3f)
                {
                    buttonOnJumpSection = true;

                    //Box with Target
                    targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                    targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                    //Plattform und Pushbox
                    movablePlatformTransform.localPosition = new Vector3(-6.08f, 3f, -37.38f);
                    movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));

                    float platformX = movablePlatformTransform.localPosition.x;
                    float platformZ = movablePlatformTransform.localPosition.z;
                    float platformScaleX = movablePlatformTransform.localScale.x;
                    float platformScaleZ = movablePlatformTransform.localScale.z;

                    while (((platformX - platformScaleX / 2) < movableBlockTransform.localPosition.x && (platformX + platformScaleX / 2) > movableBlockTransform.localPosition.x) && ((platformZ - platformScaleZ / 2) < movableBlockTransform.localPosition.z && (platformZ + platformScaleZ / 2) > movableBlockTransform.localPosition.z))
                    {
                        movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));
                    }

                    //Button new position
                    buttonTransform.localPosition = new Vector3(Random.Range(platformX - platformScaleX / 2 + 0.5f, platformX + platformScaleX / 2 - 0.5f), 5.83f, Random.Range(platformZ - platformScaleZ / 2 + 0.5f, platformZ + platformScaleZ / 2 - 0.5f));
                }
                else if (randomPosition < 4f)
                {
                    //Button
                    buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 9f), 1.25f, Random.Range(1f, 48f));
                }
                else if (randomPosition < 5f) //Button on monument
                {
                    buttonTransform.localPosition = possiblePositionsMonument[Mathf.RoundToInt(Random.Range(0f, possiblePositionsMonument.Length - 1))];
                }
                else //Button in irrgarten
                {
                    buttonTransform.localPosition = possiblePositionsIrrgarten[Mathf.RoundToInt(Random.Range(0f, possiblePositionsIrrgarten.Length - 1))];
                }
            }
            else if (currentState < 14.0f) //new push section
            {
                jumpSectionObject.SetActive(true);

                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Plattform und Pushbox
                movablePlatformTransform.localPosition = new Vector3(Random.Range(-11f, 2f), 3f, Random.Range(-40f, -10f));
                movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));

                //Enable Obstacles
                monumentObject.SetActive(true);
                irrgartenObject.SetActive(true);

                for (int i = 0; i < irrgartenObject.transform.childCount; i++)
                {
                    Transform wall = irrgartenObject.transform.GetChild(i);
                    if (wall.gameObject.tag.Equals("Untagged"))
                    {
                        break;
                    }
                    wall.gameObject.tag = "Untagged";
                    wall.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                }

                //Randomize button position
                float randomPosition = Random.Range(0f, 6f);

                if (randomPosition < 3f)
                {
                    buttonOnJumpSection = true;

                    //Box with Target
                    targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                    targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                    //Plattform und Pushbox
                    movablePlatformTransform.localPosition = new Vector3(Random.Range(-11f, 2f), 3f, Random.Range(-40f, -10f));
                    movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));

                    float platformX = movablePlatformTransform.localPosition.x;
                    float platformZ = movablePlatformTransform.localPosition.z;
                    float platformScaleX = movablePlatformTransform.localScale.x;
                    float platformScaleZ = movablePlatformTransform.localScale.z;

                    while (((platformX - platformScaleX / 2) < movableBlockTransform.localPosition.x && (platformX + platformScaleX / 2) > movableBlockTransform.localPosition.x) && ((platformZ - platformScaleZ / 2) < movableBlockTransform.localPosition.z && (platformZ + platformScaleZ / 2) > movableBlockTransform.localPosition.z))
                    {
                        movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));
                    }

                    //Button new position
                    buttonTransform.localPosition = new Vector3(Random.Range(platformX - platformScaleX / 2 + 0.5f, platformX + platformScaleX / 2 - 0.5f), 5.83f, Random.Range(platformZ - platformScaleZ / 2 + 0.5f, platformZ + platformScaleZ / 2 - 0.5f));
                }
                else if (randomPosition < 4f)
                {
                    //Button
                    buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 9f), 1.25f, Random.Range(1f, 48f));
                }
                else if (randomPosition < 5f) //Button on monument
                {
                    buttonTransform.localPosition = possiblePositionsMonument[Mathf.RoundToInt(Random.Range(0f, possiblePositionsMonument.Length - 1))];
                }
                else //Button in irrgarten
                {
                    buttonTransform.localPosition = possiblePositionsIrrgarten[Mathf.RoundToInt(Random.Range(0f, possiblePositionsIrrgarten.Length - 1))];
                }
            }
            else if (currentState < 15.0f) //Stage 9
            {
                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Plattform und Pushbox
                movablePlatformTransform.localPosition = new Vector3(Random.Range(-11f, 2f), 3f, Random.Range(-40f, -10f));
                movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));

                //Enable Obstacles
                monumentObject.SetActive(true);
                irrgartenObject.SetActive(true);

                for (int i = 0; i < irrgartenObject.transform.childCount; i++)
                {
                    Transform wall = irrgartenObject.transform.GetChild(i);
                    if (wall.gameObject.tag.Equals("Untagged"))
                    {
                        break;
                    }
                    wall.gameObject.tag = "Untagged";
                    wall.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                }

                //Randomize button position
                float randomPosition = Random.Range(0f, 4f);

                if(randomPosition < 1f)
                {
                    buttonOnJumpSection = true;

                    float platformX = movablePlatformTransform.localPosition.x;
                    float platformZ = movablePlatformTransform.localPosition.z;
                    float platformScaleX = movablePlatformTransform.localScale.x;
                    float platformScaleZ = movablePlatformTransform.localScale.z;

                    while (((platformX - platformScaleX / 2) < movableBlockTransform.localPosition.x && (platformX + platformScaleX / 2) > movableBlockTransform.localPosition.x) && ((platformZ - platformScaleZ / 2) < movableBlockTransform.localPosition.z && (platformZ + platformScaleZ / 2) > movableBlockTransform.localPosition.z))
                    {
                        movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));
                    }

                    //Button new position
                    buttonTransform.localPosition = new Vector3(Random.Range(platformX - platformScaleX / 2 + 0.5f, platformX + platformScaleX / 2 - 0.5f), 5.83f, Random.Range(platformZ - platformScaleZ / 2 + 0.5f, platformZ + platformScaleZ / 2 - 0.5f));
                }
                else if (randomPosition < 2f)
                {
                    //Button
                    buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 9f), 1.25f, Random.Range(1f, 48f));
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
            else if (currentState < 16.0f) //Stage 10
            {
                //Box with Target
                targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
                targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                //Plattform und Pushbox
                movablePlatformTransform.localPosition = new Vector3(Random.Range(-11f, 2f), 3f, Random.Range(-40f, -10f));
                movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));

                //Enable Obstacles
                monumentObject.SetActive(true);
                irrgartenObject.SetActive(true);

                for (int i = 0; i < irrgartenObject.transform.childCount; i++)
                {
                    Transform wall = irrgartenObject.transform.GetChild(i);

                    if (wall.gameObject.tag.Equals("Wall"))
                    {
                        break;
                    }

                    wall.gameObject.tag = "Wall";
                    wall.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                }


                //Randomize button position
                float randomPosition = Random.Range(0f, 4f);

                if (randomPosition < 1f)
                {
                    buttonOnJumpSection = true;

                    float platformX = movablePlatformTransform.localPosition.x;
                    float platformZ = movablePlatformTransform.localPosition.z;
                    float platformScaleX = movablePlatformTransform.localScale.x;
                    float platformScaleZ = movablePlatformTransform.localScale.z;

                    while (((platformX - platformScaleX / 2) < movableBlockTransform.localPosition.x && (platformX + platformScaleX / 2) > movableBlockTransform.localPosition.x) && ((platformZ - platformScaleZ / 2) < movableBlockTransform.localPosition.z && (platformZ + platformScaleZ / 2) > movableBlockTransform.localPosition.z))
                    {
                        movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));
                    }

                    //Button new position
                    buttonTransform.localPosition = new Vector3(Random.Range(platformX - platformScaleX / 2 + 0.5f, platformX + platformScaleX / 2 - 0.5f), 5.83f, Random.Range(platformZ - platformScaleZ / 2 + 0.5f, platformZ + platformScaleZ / 2 - 0.5f));
                }
                else if(randomPosition < 2f)
                {
                    //Button
                    buttonTransform.localPosition = new Vector3(Random.Range(-8.4f, 9f), 1.25f, Random.Range(1f, 48f));
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
        else
        {
            movementScript.ResetPlayer(new Vector3(Random.Range(-18.5f, -10.5f), 0.501f, Random.Range(-48f, 48f)));

            //movableBlockTransform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY
            //        | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            //Box with Target
            targetBoxTransform.localPosition = new Vector3(Random.Range(-40f, -25f), 0f, Random.Range(-37f, 37f));
            targetBoxTransform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

            //Plattform und Pushbox
            movablePlatformTransform.localPosition = new Vector3(Random.Range(-11f, 2f), 3f, Random.Range(-40f, -10f));
            movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));

            //Enable Obstacles
            monumentObject.SetActive(true);
            irrgartenObject.SetActive(true);
            jumpSectionObject.SetActive(true);

            if (outsideWall1.activeSelf)
            {
                outsideWall1.SetActive(false);
                outsideWall2.SetActive(false);
                outsideWall3.SetActive(false);
                outsideWall4.SetActive(false);
            }

            if (outsideWallNextStage1.activeSelf)
            {
                outsideWallNextStage1.SetActive(false);
                outsideWallNextStage2.SetActive(false);
                outsideWallNextStage3.SetActive(false);
                outsideWallNextStage4.SetActive(false);
            }

            for (int i = 0; i < irrgartenObject.transform.childCount; i++)
            {
                Transform wall = irrgartenObject.transform.GetChild(i);
                if (wall.gameObject.tag.Equals("Untagged"))
                {
                    break;
                }
                wall.gameObject.tag = "Untagged";
                wall.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            }

            //Randomize button position
            float randomPosition = Random.Range(0f, 4f);

            if (randomPosition < 1f)
            {
                buttonOnJumpSection = true;

                float platformX = movablePlatformTransform.localPosition.x;
                float platformZ = movablePlatformTransform.localPosition.z;
                float platformScaleX = movablePlatformTransform.localScale.x;
                float platformScaleZ = movablePlatformTransform.localScale.z;

                while (((platformX - platformScaleX / 2) < movableBlockTransform.localPosition.x && (platformX + platformScaleX / 2) > movableBlockTransform.localPosition.x) && ((platformZ - platformScaleZ / 2) < movableBlockTransform.localPosition.z && (platformZ + platformScaleZ / 2) > movableBlockTransform.localPosition.z))
                {
                    movableBlockTransform.localPosition = new Vector3(Random.Range(-17f, 8f), 0.85f, Random.Range(-33f, -11f));
                }

                //Button new position
                buttonTransform.localPosition = new Vector3(Random.Range(platformX - platformScaleX / 2 + 0.5f, platformX + platformScaleX / 2 - 0.5f), 5.83f, Random.Range(platformZ - platformScaleZ / 2 + 0.5f, platformZ + platformScaleZ / 2 - 0.5f));
            }
            else if (randomPosition < 2f)
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

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(buttonPressed);

        sensor.AddObservation(transform.forward);

        if (!buttonPressed)
        {
            Vector3 buttonVector = buttonTransform.position - transform.position;
            sensor.AddObservation(buttonVector.normalized);
            sensor.AddObservation(buttonVector.magnitude);
        }
        else
        {
            Vector3 coinVector = targetTransform.position - transform.position;
            sensor.AddObservation(coinVector.normalized);
            sensor.AddObservation(coinVector.magnitude);
        }

        //Position movable block
        sensor.AddObservation(movableBlockTransform.localPosition);

        //Distance movable block to center of platform
        Vector3 movableDistanceRelation = movablePlatformTransform.position - movableBlockTransform.position;
        sensor.AddObservation(movableDistanceRelation.normalized);
        sensor.AddObservation(movableDistanceRelation.magnitude);

        //sensor.AddObservation(agentHandTransform.localPosition);

        //sensor.AddObservation(pushBlockRewardAlreadyTaken);

        //Is block in acceptable jump region true else false
        //if (movableDistanceRelation.magnitude >= 9f && movableDistanceRelation.magnitude <= 13f)
        //{
        //    sensor.AddObservation(true);
        //}
        //else
        //{
        //    sensor.AddObservation(false);
        //}
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.0005f);

        //Discrete Movement
        //float moveInput = actions.DiscreteActions[2] <= 1 ? actions.DiscreteActions[2] : -1;
        //float turnInput = actions.DiscreteActions[3] <= 1 ? actions.DiscreteActions[3] : -1;
        //int pressButton = actions.DiscreteActions[0];
        //bool jumpPressed = actions.DiscreteActions[1] == 1 ? true : false;

        //Continues Movement
        float moveInput = actions.ContinuousActions[0];
        float turnInput = actions.ContinuousActions[1];
        int pressButton = actions.DiscreteActions[0];
        bool jumpPressed = actions.DiscreteActions[1] == 1 ? true : false;
        bool liftPressed = actions.DiscreteActions[2] == 1 ? true : false;

        //Debug.Log(moveInput);

        movementScript.moveInput = moveInput;
        movementScript.turnInput = turnInput;
        movementScript.jumpPressed = jumpPressed;
        movementScript.liftBox = liftPressed;

        if (Physics.CheckSphere(buttonCheck.position, buttonCheckDistance, buttonLayerMask) && pressButton == 1)
        {
            if (buttonPressed == false)
            {
                AddReward(0.5f);
                buttonPressed = true;
                buttonTransform.gameObject.SetActive(false);
                targetBoxTransform.gameObject.SetActive(true);

                if (door != null)
                {
                    door.GetComponent<MeshRenderer>().enabled = false;
                    door.GetComponent<BoxCollider>().enabled = false;
                }

                //If pushblock is in acceptable region to jump and current level is far enough -> give extra points for the pushing
                //Vector3 movableDistanceRelation = movablePlatformTransform.position - movableBlockTransform.position;
                //if (buttonOnJumpSection && (movableDistanceRelation.magnitude >= 9f && movableDistanceRelation.magnitude <= 13f))
                //{
                //    Debug.Log("Giving extra points for positioning of the push block!");
                //    AddReward(0.5f);
                //}
            }

        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;

        //Discrete
        //discreteActions[2] = Input.GetAxis("Vertical") >= 0 ? Mathf.RoundToInt(Input.GetAxis("Vertical")) : 2;
        //discreteActions[3] = Input.GetAxis("Horizontal") >= 0 ? Mathf.RoundToInt(Input.GetAxis("Horizontal")) : 2;

        //continues move
        continousActions[0] = Input.GetAxis("Vertical");
        continousActions[1] = Input.GetAxis("Horizontal");

        discreteActions[0] = Input.GetKey(KeyCode.B) ? 1 : 0;
        discreteActions[1] = Input.GetButton("Jump") ? 1 : 0;
        discreteActions[2] = Input.GetKey(KeyCode.L) ? 1 : 0;

        Vector3 movableDistanceRelation = movablePlatformTransform.position - movableBlockTransform.position;
        if (movableDistanceRelation.magnitude >= 9f && movableDistanceRelation.magnitude <= 13f) {
            Debug.Log("TRUE --- Distance: " + movableDistanceRelation.magnitude);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PointArea") && buttonOnJumpSection)
        {
            if (!rewardIsAlreadyTaken)
            {
                Debug.Log("PointArea reached...");
                rewardIsAlreadyTaken = true;
                AddReward(0.5f);
            }
        }
        else if (other.CompareTag("PointArea"))
        {
            Debug.Log("PointArea reached without Button on top = minus reward...");
            AddReward(-3f);
            EndEpisode();
        }

        if (other.CompareTag("Target"))
        {
            floorMeshRenderer.material = winMaterial;
            Debug.Log("Target found!");
            AddReward(3f);
            EndEpisode();
        }
        else if (other.CompareTag("Wall"))
        {
            Debug.Log("Wall hitted!");
            floorMeshRenderer.material = loseMaterial;
            AddReward(-3f);
            movementScript.SetLiftStatus(false);
            EndEpisode();
        }
    }

    public void AddRewardForPushingBlock()
    {
        if (buttonOnJumpSection && !pushBlockRewardAlreadyTaken && movementScript.IsPlayerOnGround())
        {
            pushBlockRewardAlreadyTaken = true;
            jumpsectionWall.SetActive(false);
            movementScript.SetLiftStatus(false);
            AddReward(0.5f);
        }
    }

    public void PushedBlockAgainstWall()
    {
        AddReward(-3);
        movementScript.SetLiftStatus(false);
        EndEpisode();
    }
}
