using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class WirePlaceMode : MonoBehaviour
{
    public GameObject wirePrefab;
    public GameObject wireEndPrefab;
    public GameObject charSpotPrefab;
    public List<GameObject> charSpotPrefabs;
    // public TMP_Dropdown dropdown;
    public List<Transform> wireStarts = new List<Transform>();
    public Dictionary<Transform, List<Transform>> wireDict = new Dictionary<Transform, List<Transform>>();
    public Dictionary<GameObject, GameObject> wireEndPairs = new Dictionary<GameObject, GameObject>();
    public bool wireMode = false;
    public GameObject wireCreationPanel;
    public Transform player;
    public List<GameObject> charSpotList = new List<GameObject>();
    private float spacing;
    private Transform currentKey;
    private bool wireStart = true;
    private GameObject currentWireStart;
    private CircuitNode currentFirstNode;
    public List<Button> gateButtons = new List<Button>();
    public string currentGateSelected;
    public GameObject finalNode;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spacing = 1;
        wireStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (wireMode)
        {
            
        }
    }

    public void ToggleWireMode()
    {
        wireMode = !wireMode;
        if (wireMode)
        {
            wireCreationPanel.SetActive(true);
        } else
        {
            wireCreationPanel.SetActive(false);
        }
    }

    public void PlaceWire(Vector3 start, Vector3 end)
    {
        if (!wireMode)
        {
            return;
        }

        foreach (Transform key in wireDict.Keys)
        {
            if (key.position == start)
            {
                return;
            }
        }

        Vector3 direction = (end - start).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);

        Vector3 currentSpot = start;
        bool first = true;
        while (currentSpot != end)
        {
            
            GameObject wirePart = Instantiate(wirePrefab, currentSpot, rotation);
            if (first)
            {
                wireDict[wirePart.transform] = new List<Transform>();
                currentKey = wirePart.transform;
            } else
            {
                wireDict[currentKey].Add(wirePart.transform);
            }
            currentSpot += direction * spacing;
            if (Vector3.Distance(currentSpot, end) < 0.5)
            {
                wireStarts.Add(wirePart.transform);
                break;
            }

            wireStarts.Add(wirePart.transform);

            first = false;
        }

        
        
    }

    public bool PlaceWireEnd(Vector3 pos, CircuitNode node)
    {
        if (!wireMode)
        {
            return false;
        }

        

        GameObject thisWire = Instantiate(wireEndPrefab, pos, Quaternion.identity);

        if (wireStart)
        {
            currentWireStart = thisWire;
            // wireEndPairs.Add()
            wireEndPairs[currentWireStart] = currentWireStart;
            currentFirstNode = node;
            wireStarts.Add(thisWire.transform);
        } else
        {
            wireEndPairs[currentWireStart] = thisWire;
            bool inputAdded = node.AddInput(currentFirstNode);
            wireStarts.Add(thisWire.transform);
            if (!inputAdded)
            {
                return false;
            }
            bool outputAdded = currentFirstNode.AddOutput(node);
            if (!outputAdded)
            {
                node.RemoveInput(currentFirstNode);
                return false;
            }
            
        }

        


        wireStart = !wireStart;
        return true;
    }

    public void PlaceChar(Vector3 pos)
    {
        if (!wireMode)
        {
            return;
        }

        GameObject charSpot = Instantiate(charSpotPrefab, pos, Quaternion.identity);
        charSpotList.Add(charSpot);
        wireStarts.Add(charSpot.transform);
    }

    public void ClearArea()
    {
        List<Transform> toDestroy = new List<Transform>();
        foreach (Transform wire in wireStarts)
        {
            toDestroy.Add(wire);
        }
        for (int i = 0; i < toDestroy.Count; i++)
        {
            Destroy(toDestroy[i].gameObject);
        }
        wireStarts.Clear();
        wireDict.Clear();
        wireStart = true;
    }

    public void OnDropdownChange(string value)
    {
        GameObject item = null;
        for (int i = 0; i < charSpotPrefabs.Count; i++)
        {
            if (charSpotPrefabs[i].GetComponent<ItemData>().itemName == value)
            {
                item = charSpotPrefabs[i];
            }
        }

        charSpotPrefab = item;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("on submit: " + eventData.ToString());
        OnDropdownChange(eventData.ToString());
    }

    public void SetNANDButton()
    {
        currentGateSelected = "NANDSpot";
        SetCharPrefab(currentGateSelected);
    }
    public void SetNORButton()
    {
        currentGateSelected = "NORSpot";
        SetCharPrefab(currentGateSelected);
    }
    public void SetANDButton()
    {
        currentGateSelected = "ANDSpot";
        SetCharPrefab(currentGateSelected);
    }
    public void SetNOTButton()
    {
        currentGateSelected = "NOTSpot";
        SetCharPrefab(currentGateSelected);
    }
    public void SetXORButton()
    {
        currentGateSelected = "XORSpot";
        SetCharPrefab(currentGateSelected);
    }
    public void SetXNORButton()
    {
        currentGateSelected = "XNORSpot";
        SetCharPrefab(currentGateSelected);
    }
    public void SetORButton()
    {
        currentGateSelected = "ORSpot";
        SetCharPrefab(currentGateSelected);
    }

    public void SetCharPrefab(string value)
    {
        Debug.Log("Set to: " + value);
        GameObject item = null;
        for (int i = 0; i < charSpotPrefabs.Count; i++)
        {
            if (charSpotPrefabs[i].GetComponent<InteractableObject>().itemData.itemName == value)
            {
                item = charSpotPrefabs[i];
            }
        }

        charSpotPrefab = item;
    }

    public void SetFinalNode(GameObject node)
    {
        if (finalNode != null)
        {
            finalNode.GetComponent<CircuitNode>().finalOutput = false;
        }
        finalNode = node;
        node.GetComponent<CircuitNode>().finalOutput = true;

        

        Debug.Log("FinalNode: " + finalNode.name);

    }

    
}
