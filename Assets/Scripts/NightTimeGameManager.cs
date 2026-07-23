using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class NightTimeGameManager : MonoBehaviour
{
    // Other Scripts
    public JudgementController judgementController;
    public WirePlaceMode wireScript;
    // public TheKing theKing;
    // public NightBehavior nightBehavior;


    // Scene
    public Camera nightCamera;
    public Camera mainCamera;
    public Light sun;
    public bool isDay;


    // LISTS
    // Set By Scripts
    public List<CircuitNode> circuitNodes = new List<CircuitNode>(); // set by Wire Place Mode
    public List<GameObject> wireParts = new List<GameObject>();

    // Set in Editor
    public List<CircuitNode> gates = new List<CircuitNode>();


    // BOOLS
    // set in Generate Monsters
    public bool kingWantsGift; 
    public bool monster1;
    public bool monster2;

    // Set by FinalNode (CircuitNode script)
    public bool finalOutput;

    
    // UI
    public TextMeshProUGUI infoText;
    public GameObject kingsMessage;

    public void Start()
    {
        wireScript = GetComponent<WirePlaceMode>();
        judgementController = GetComponent<JudgementController>();
        GenerateMonsters();
    }
   
    
    // Run at start of day
        public void GenerateMonsters()
    {
        Debug.Log("Generating Monsters");
        // currently 2
        for (int i = 0; i < gates.Count; i++)
        {
            Debug.Log("GM, i = " + i);
            bool isMonster = Random.value > 0.5 ? true : false;
            Debug.Log("isMonster: " + isMonster);
            
            // when monster is true, input value should be false
            gates[i].AddInputValue(isMonster);
            gates[i].AddInputValue(isMonster);
            if (i == 0)
            {
                monster1 = isMonster;
                Debug.Log("GM, monster1: " + isMonster);
            }
            if (i == 1)
            {
                monster2 = isMonster;
                Debug.Log("GM, monster2: " + isMonster);
            }
        }

        judgementController.gate1HasGift = gates[0];
        judgementController.gate2HasGift = gates[1];

        bool wantsGift = Random.value > 0.5 ? true : false;
        // theKing.wantsGift = wantsGift;
        kingWantsGift = wantsGift;
        Debug.Log("wantsGift: " + wantsGift);
        // Debug.Log("theKing.wantsGift: " + theKing.wantsGift);

        infoText.text = "Nighttime Information \n\nGate 1: " + (monster1 ? "monster" : "safe") + " \nGate 2: " + (monster2 ? "monster" : "safe") + "\n\nKing desires: " + (kingWantsGift ? "gift" : "no gift");

    }


    // Begins nighttime procedures -- triggered by button
    public void StartNight()
    {
        if (wireScript.finalNode == null)
        {
            return;
        }

        nightCamera.enabled = true;
        mainCamera.enabled = false;

        isDay = false;
        sun.gameObject.SetActive(false);

        Debug.Log("Night Begun");

        // Re-add gates to circuit node list
        foreach(CircuitNode gate in gates)
        {
            circuitNodes.Add(gate);
        }

        foreach (CircuitNode node in circuitNodes)
        {
            node.StartNight();
        }

        StartCoroutine(WaitForCircuitToFinish());
    }

    // Begins day in main scene
    public void StartDay() 
    {
        // theKing.ready = null;
        mainCamera.enabled = true;
        nightCamera.enabled = false;

        isDay = true;
        sun.gameObject.SetActive(true);


    }

    // Starts next day -- triggered by button
    public void OnClickNextDay()
    {
        ClearCircuit();
        GenerateMonsters();
        kingsMessage.SetActive(false);
        wireScript.ClearArea();
        wireScript.numCharNodes = 0;
        StartDay();

    }

    // Runs until all nodes are done. Gets Results
    IEnumerator WaitForCircuitToFinish()
    {
        bool notDone = true;
        while (notDone)
        {
            notDone = false;
            foreach(CircuitNode node in circuitNodes)
            {
                if (node.actionsCompleted == false)
                {
                    notDone = true;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("the king wants: " + kingWantsGift);
        // Debug.Log("final output: " + );
        kingsMessage.SetActive(true);
        judgementController.CalculateEndResults(finalOutput, kingWantsGift);

        // theKing.AfterNight();
        
    }

    // Clears circuit after night
    public void ClearCircuit()
    {
        List<CircuitNode> toRemove = new List<CircuitNode>();
        foreach (CircuitNode node in circuitNodes)
        {
            
            node.ClearNode();
            toRemove.Add(node);

        }
        foreach (CircuitNode gate in gates)
        {
            gate.ClearNode();
        }

        foreach (CircuitNode node in toRemove)
        {
            if (circuitNodes.Contains(node))
            {
                circuitNodes.Remove(node);
            }
            else if (gates.Contains(node))
            {
                gates.Remove(node);
            }
        }

    }
}
