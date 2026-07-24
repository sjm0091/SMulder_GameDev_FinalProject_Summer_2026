using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

public class WirePlaceMode : MonoBehaviour
{
    public Vector3 outputSiteOffset = new Vector3(0f, 0f, 0f);
    public TextMeshProUGUI failText;
    public GameObject failTextPanel;
    public float failTextTime = 3f;
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
    public int numCharNodes;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spacing = 1;
        wireStart = true;
        numCharNodes = 0;
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
        Debug.Log("reached place wire");
        if (!wireMode)
        {
            Debug.Log("not wire mode");
            return;
        }

        foreach (Transform key in wireDict.Keys)
        {
            if (key.position == start)
            {
                Debug.Log("wire not placed. key.position == start");
                return;
            }
        }

        Debug.Log("after foreach");

        Vector3 direction = (end - start).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);

        Vector3 currentSpot = start;
        Debug.Log("currentSpot = " + currentSpot);
        Debug.Log("currentSpot == end: " + (currentSpot == end));
        bool first = true;
        while (currentSpot != end)
        {
            Debug.Log("while loop");
            
            GameObject wirePart = Instantiate(wirePrefab, currentSpot, rotation);
            Debug.Log("Instantiated wirepart: " + wirePart.name);
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
        Debug.Log("Place Wire End Triggered");
        if (!wireMode)
        {
            Debug.Log("Not wire mode");
            return false;
        }

        if (wireStart && node.outputsFull)
        {
            Debug.Log("Outputs full");
            SetFailText("Ouputs full");
            return false;
        }

        if (!wireStart && node.inputsFull)
        {
            Debug.Log("Inputs full");
            SetFailText("Inputs full");
            return false;
        }

        
        if (wireStart && node.outputSites.Count <= 0)
        {
            Debug.Log("Not first if");
            SetFailText("Ouputs full");
            return false;
        }

        if (!wireStart && (node.inputs.Count >= node.inputMax || node.inputSites.Count <= 0))
        {
            Debug.Log("Not second if");
            SetFailText("Inputs full");
            return false;
        }

        if ((!wireStart && CheckIfIOfull(node, true)) || (wireStart && CheckIfIOfull(node, false)))
        {
            Debug.Log("askfh;asdlkhfjo;adjkf;l");
            Debug.Log("wireStart: " + wireStart);
            string newText = wireStart ? "Outputs full" : "Inputs full";
            StartCoroutine(SetFailText(newText));
            return false;
        }

        GameObject parent;
        if (wireStart)
        {   
            Debug.Log("Output finding parent");
            int outputIndex = 0;
            for (int i = 0; i < node.outputSites.Count; i++)
            {
                if (i == node.outputs.Count)
                {
                    outputIndex = i;
                    break;
                }
            }
            parent = node.outputSites[outputIndex];
        } else
        {
            Debug.Log("Input finding parent");
            int inputIndex = 0;
            for (int i = 0; i < node.inputSites.Count; i++)
            {
                if (i == node.inputs.Count)
                {
                    inputIndex = i;
                    break;
                }
            }
            parent = node.inputSites[inputIndex];
        }


        

        GameObject thisWire = Instantiate(wireEndPrefab, pos, Quaternion.identity, parent.transform);
        thisWire.transform.position = parent.transform.position + outputSiteOffset;

        thisWire.transform.SetParent(node.gameObject.transform);
        parent.SetActive(false);

        Debug.Log("thisWire: " + thisWire.name);
        Debug.Log("Parent: " + parent.name);
        Debug.Log("WireStart = " + wireStart);

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

    public bool CheckIfIOfull(CircuitNode node, bool input)
    {
        Debug.Log("Check if io filled triggered");
        bool full = true;
        if (input)
        {
            full = true;
            foreach (GameObject i in node.inputSites)
            {
                if (i.activeSelf)
                {
                    full = false;
                }
            }
        }
        else if (!input)
        {
            full = true;
            foreach (GameObject i in node.outputSites)
            {
                if (i.activeSelf)
                {
                    full = false;
                }
            }
        }

        return full;
    }

    public void PlaceChar(Vector3 pos)
    {
        if (!wireMode)
        {
            return;
        }

        GameObject charSpot = Instantiate(charSpotPrefab, pos, Quaternion.identity);
        numCharNodes++;
        charSpot.GetComponent<CircuitNode>().nodeName = "Character: " + charSpot.GetComponent<GateBehaviorScript>().gateType + " " + numCharNodes;
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

    public IEnumerator SetFailText(string text)
    {
        failText.text = text;
        failText.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(failTextTime);

        float fullAlpha = 1;
        while (fullAlpha > 0)
        {
            Image failImg = failTextPanel.GetComponent<Image>();
            failImg.color = new Color(failImg.color.r, failImg.color.b, failImg.color.g, fullAlpha--);
        }

        failText.gameObject.SetActive(false);
        failText.text = "";
    }

    
}
