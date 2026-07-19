using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class WireGeneration : MonoBehaviour
{
    public GameObject[] wireSegmentPrefabArray;
    public Transform player;
    public GameObject lastSegment;
    public TextMeshProUGUI interactionText;
    public Terrain terrain;
    
    private Vector3 spawnPos;
    private float minDistance = 10f;
    private float spawnDistance = 2f;
    private float zRotation; // TODO: implement based on port direction
    private float yRotation; // TODO: implement based on terrain rotation

    private Vector3 refAngle = new Vector3(0f, 0f, 1f);
    bool firstSectionOfTurn = true;
    bool interacting = false;
    public Vector2 interactInput;
    private float coolDown = 1f;
    private bool coolDownOn = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int myInput = 0;
        if (Keyboard.current.rKey.isPressed)
        {
            myInput = 1;
        }
        if (Keyboard.current.vKey.isPressed)
        {
            myInput = 0;
        }
        // Debug.Log(myInput);
        if (Vector3.Distance(lastSegment.transform.position, player.position) < minDistance)
        {
            interactionText.gameObject.SetActive(true);
            // Debug.Log(Vector3.Distance(lastSegment.transform.position, player.position));
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
         if (myInput > 0)
        {
            Debug.Log("triggered");
            if (CheckDistance(player) && !coolDownOn)
            {
                TerrainData terrainData = terrain.terrainData;
                Vector3 terrainSize = terrainData.size;
                StartCoroutine(RunCoolDown());
                Debug.Log("within distance");
                Vector3 direction = (player.position - lastSegment.transform.position).normalized;
                spawnPos = lastSegment.transform.position + (direction * spawnDistance);
                GameObject nextSegmentPrefab = ChooseViableSegment();
                Vector3 rotation = new Vector3(0f, 0f, 90f);
                
                yRotation = terrainData.GetInterpolatedHeight( // TODO: fix
                spawnPos.x / terrainSize.x, 
                spawnPos.y / terrainSize.z
            )   ;
                GameObject nextSegment = Instantiate(nextSegmentPrefab, spawnPos, Quaternion.Euler(0f, player.rotation.y + 180, yRotation + 90));
                lastSegment = nextSegment;
            }
        }
    }

    private IEnumerator RunCoolDown()
    {
        coolDownOn = true;
        yield return new WaitForSeconds(coolDown);
        coolDownOn = false;
    }

    public void OnWire(int value)
    {
        // Vector2 interactInput = value.Get<Vector2>();
        Debug.Log(interactInput);
    }

    private bool CheckDistance(Transform target)
    {
        Debug.Log("check distance triggered");
        if (Vector3.Distance(lastSegment.transform.position, target.position) <= minDistance)
        {
            return true;
        }
        return false;
    }

    private GameObject ChooseViableSegment()
    {
        WireSegmentDetails lastSegmentDetails = lastSegment.GetComponent<WireSegmentDetails>();
        int quadrant = CheckAngleSegment();
        GameObject chosenPrefab = wireSegmentPrefabArray[0];
        switch (quadrant)
        {
            case 1:
                chosenPrefab = wireSegmentPrefabArray[0];
                break;
            case 2:
                chosenPrefab = wireSegmentPrefabArray[1];
                break;
            case 3:
                chosenPrefab = wireSegmentPrefabArray[2];
                break;
            default:
                break;
            
        }
        return chosenPrefab;
    }

    public int CheckAngleSegment()
    {
        Vector3 playerDirection = (player.position - lastSegment.transform.position).normalized;
        float deltaAngle = Mathf.Acos(Vector3.Dot(playerDirection, refAngle)/(Vector3.Magnitude(playerDirection) * Vector3.Magnitude(refAngle)));
        int angleSegment = 1;
        if (deltaAngle <= (Mathf.PI / 45))
        {
            angleSegment = 1;
        }
        else if (deltaAngle <= Mathf.PI)
        {
            if (playerDirection.x >= 0) angleSegment = 2;
            else if (playerDirection.x <= 0) angleSegment = 3;
        }

        return angleSegment;
    }
}


