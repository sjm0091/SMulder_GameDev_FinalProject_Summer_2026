using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class CircuitNode : MonoBehaviour
{
    // public JudgementController judgementController;
    public string nodeName;
    
    public List<bool> inputList = new List<bool>();
    public bool output;
    public bool finalOutput;

    public Transform wireConnection;
    public List<CircuitNode> inputs = new List<CircuitNode>();
    public List<CircuitNode> outputs = new List<CircuitNode>();
    public bool hasInputLimit = true;
    public int inputMax = 2;
    public bool hasOutputLimit = false;
    public int outputMax = 2;
    private GateBehaviorScript thisGate;
    // public DayNightController dayNightController;
    private bool nightOn = false;
    public bool actionsCompleted = false;
    // public TheKing theKing;

    public NightTimeGameManager nightTimeGameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisGate = GetComponent<GateBehaviorScript>();
        nightTimeGameManager = FindFirstObjectByType<NightTimeGameManager>();
        // dayNightController = FindFirstObjectByType<DayNightController>();
        // theKing = FindAnyObjectByType<TheKing>();
        // judgementController = FindFirstObjectByType<JudgementController>();

        // dayNightController.circuitNodes.Add(this);
        nightTimeGameManager.circuitNodes.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        // if (!dayNightController.isDay && !nightOn)
        // {
        //     StartNight();
        // }
    }

    public bool AddInput(CircuitNode node)
    {
        Debug.Log(nodeName + " Add Input: " + node.nodeName);
        if (hasInputLimit)
        {
            if (inputs.Count >= inputMax)
            {
                return false;
            }
        }

        inputs.Add(node);
        return true;
    }

    public bool AddInputValue(bool value)
    {
        Debug.Log(nodeName + " Add Input Value: " + value);
        // if (hasInputLimit)
        // {
        //     if (inputs.Count >= inputMax)
        //     {
        //         return false;
        //     }
        // }

        inputList.Add(value);
        return true;
    }

    public bool RemoveInput(CircuitNode node)
    {
        Debug.Log(nodeName + " Remove Input: " + node.nodeName);
        if (inputs.Count > 0)
        {
            inputs.Remove(node);
        } else
        {
            return false;
        }
        return true;
    }

    public bool AddOutput(CircuitNode node)
    {
        Debug.Log(nodeName + " Add Output: " + node.nodeName);
        if (hasOutputLimit)
        {
            if (outputs.Count >= outputMax)
            {
                return false;
            }
        }

        outputs.Add(node);
        return true;
    }

    public bool RemoveOutput(CircuitNode node)
    {
        Debug.Log(nodeName + " Remove Output: " + node.nodeName);
        if (outputs.Count > 0)
        {
            outputs.Remove(node);
        } else
        {
            return false;
        }
        return true;
    }

    public void ClearNode()
    {
        outputs.Clear();
        inputs.Clear();
        inputList.Clear();
        output = false;
        finalOutput = false;
        actionsCompleted = false;
        nightOn = false;

        Debug.Log(nodeName + " cleared successfully");
    }


    public void StartNight()
    {
        nightOn = true;
        Debug.Log("Start Night Activated: " + nodeName);
        // move to topView camera

        // start circuit run
        if (!nightTimeGameManager.isDay)
        {
            StartCoroutine(CircuitRunRoutine());
        }
    }

    IEnumerator CircuitRunRoutine()
    {
        Debug.Log("CircuitRunRoutine Begin: " + nodeName);
        Debug.Log("Final output = " + finalOutput + ", " + nodeName);
        while (inputList.Count != inputMax)
        {
            yield return new WaitForSeconds(0.5f);
            continue;
        }
        Debug.Log(nodeName + "Received all inputs:");
        foreach (bool input in inputList)
        {
            Debug.Log(nodeName + " input: " + input);
        }
        if (thisGate != null)
        {
            thisGate.input1 = inputList[0];
            thisGate.input2 = inputList[1];
        }
        

        bool newOutput = thisGate.PerformGateBehavior();

        
        output = newOutput;
        Debug.Log(nodeName + " output: " + output);

        foreach (CircuitNode node in outputs)
        {
            Debug.Log(nodeName + " adding value " + output + " to node: " + node.nodeName);
            node.AddInputValue(output);
        }

        if (finalOutput)
        {
            Debug.Log("Final Node: "+nodeName+". Final Output: " + output);
            Debug.Log("The King wants: " + nightTimeGameManager.kingWantsGift);
            Debug.Log("output == theKing.wantsGift: " + (output == nightTimeGameManager.kingWantsGift));
            nightTimeGameManager.finalOutput = output;
            // theKing.survivedNight = output == theKing.wantsGift;
            // theKing.ready = "yes";

            // judgementController.CalculateEndResults(output);
        }
        actionsCompleted = true;
        
    }
}
