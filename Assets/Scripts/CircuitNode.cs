using System.Collections.Generic;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CircuitNode : MonoBehaviour
{
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
    public DayNightController gameManager;
    private bool nightOn = false;
    public bool actionsCompleted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisGate = GetComponent<GateBehaviorScript>();
        gameManager = FindFirstObjectByType<DayNightController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isDay && !nightOn)
        {
            StartNight();
        }
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


    public void StartNight()
    {
        nightOn = true;
        Debug.Log("Start Night Activated: " + nodeName);
        // move to topView camera

        // start circuit run
        if (!gameManager.isDay)
        {
            StartCoroutine(CircuitRunRoutine());
        }
    }

    IEnumerator CircuitRunRoutine()
    {
        Debug.Log("CircuitRunRoutine: " + nodeName);
        Debug.Log("Final output = " + finalOutput + ", " + nodeName);
        while (inputList.Count != inputMax)
        {
            yield return new WaitForSeconds(0.5f);
            continue;
        }
        Debug.Log("out of loop");

        thisGate.input1 = inputList[0];
        thisGate.input2 = inputList[1];

        bool newOutput = thisGate.PerformGateBehavior();
        output = newOutput;
        Debug.Log(nodeName + " output: " + output);

        foreach (CircuitNode node in outputs)
        {
            node.AddInputValue(output);
        }

        if (finalOutput)
        {
            Debug.Log("Final Output: " + output);
        }
        actionsCompleted = true;
        
    }
}
