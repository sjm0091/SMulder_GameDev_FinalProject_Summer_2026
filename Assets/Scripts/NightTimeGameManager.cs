using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using Unity.VisualScripting;

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


    // Visits
    public List<bool> wantsGiftByVisit = new List<bool>();
    public int numVisits = 4;
    public int visitCount = 0;
    public List<bool> kingPleasedPerVisit = new List<bool>();
    public GameObject endOfDayPanel;
    public TextMeshProUGUI endOfDayMessage;
    public GameObject infoPanelGrid;
    public List<Image> gridCellsByGate = new List<Image>();
    public List<Image> outputGridCells = new List<Image>();
    public Sprite giftSprite;
    public Sprite noGiftSprite;

    // Outputs
    public List<bool[]> outputPossibilities = new List<bool[]>();
    

    public void Start()
    {
        bool[] list1 = {true, false, false, false};
        bool[] list2 = {false, false, false, true};
        bool[] list3 = {true, false, false, true};
        bool[] list4 = {false, true, true, false};
        bool[] list5 = {false, true, true, true};
        bool[] list6 = {true, true, true, false};

        outputPossibilities.Add(list1);
        outputPossibilities.Add(list2);
        outputPossibilities.Add(list3);
        outputPossibilities.Add(list4);
        outputPossibilities.Add(list5);
        outputPossibilities.Add(list6);

        wireScript = GetComponent<WirePlaceMode>();
        judgementController = GetComponent<JudgementController>();
        // GenerateMonsters();
        GenerateOutputs();
        visitCount = 0;
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

    public void SetGateInputs(int visitNum)
    {
        Debug.Log("Set Gate Inputs triggered. Visit #" + visitNum);
        switch(visitNum)
        {
            case 1:
                gates[0].AddInputValue(false);
                gates[0].AddInputValue(false);
                gates[1].AddInputValue(false);
                gates[1].AddInputValue(false);
                break;
            case 2:
                gates[0].AddInputValue(false);
                gates[0].AddInputValue(false);
                gates[1].AddInputValue(true);
                gates[1].AddInputValue(true);
                
                break;
            case 3:
                gates[0].AddInputValue(true);
                gates[0].AddInputValue(true);
                gates[1].AddInputValue(false);
                gates[1].AddInputValue(false);
                break;
            case 4:
                gates[0].AddInputValue(true);
                gates[0].AddInputValue(true);
                gates[1].AddInputValue(true);
                gates[1].AddInputValue(true);
                break;
        }

        judgementController.gate1HasGift = gates[0];
        judgementController.gate2HasGift = gates[1];

    }

    // Determines on which visits the kind wants a gift
    public void GenerateOutputs()
    {
        int index = Random.Range(0, outputPossibilities.Count - 1);
        bool[] outputList = outputPossibilities[index];

        List<bool> giftsByVisit = new List<bool>();
        for (int i = 0; i < outputList.Length; i++)
        {
            // bool wantsGift = Random.value > 0.5f ? true : false;

            bool wantsGift = outputList[i];

            Sprite newSprite = wantsGift ? giftSprite : noGiftSprite;
            outputGridCells[i].sprite = newSprite;
             
            giftsByVisit.Add(wantsGift);
        }

        wantsGiftByVisit = giftsByVisit; 
    }

    public void RunThroughVisit()
    {
        Debug.Log("numVisits: " + numVisits);
        Debug.Log("visitCount: " + visitCount);
        if (visitCount >= numVisits)
        {
            
            KeyValuePair<bool, string> response = judgementController.CheckIfSurvivedNight(kingPleasedPerVisit);

            bool survived = response.Key;
            SetEndOfDayText(response.Value);
            kingsMessage.SetActive(false);
            return;
        }
        // visitCount = 0;
        
        SetGateInputs(visitCount + 1);
        kingWantsGift = wantsGiftByVisit[visitCount];
        Debug.Log("Visit " + visitCount + ":");
        foreach (CircuitNode node in circuitNodes)
        {
            node.StartNight();
        }

        StartCoroutine(WaitForCircuitToFinish());
        visitCount++;
        
    }

    public void SetEndOfDayText(string text)
    {
        endOfDayPanel.SetActive(true);
        endOfDayMessage.text = text;
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

        RunThroughVisit();

        // Re-add gates to circuit node list
        // foreach(CircuitNode gate in gates)
        // {
        //     // circuitNodes.Add(gate);
        // }

        // foreach (CircuitNode node in circuitNodes)
        // {
        //     node.StartNight();
        // }

        // StartCoroutine(WaitForCircuitToFinish());
    }

    // Begins day in main scene
    public void StartDay() 
    {
        // theKing.ready = null;
        mainCamera.enabled = true;
        nightCamera.enabled = false;

        isDay = true;
        sun.gameObject.SetActive(true);

        GenerateOutputs();


    }

    public void OnClickNextVisit()
    {
        foreach (CircuitNode node in circuitNodes)
        {
            node.ClearNodeAfterVisit();
        }

        kingsMessage.SetActive(false);

        RunThroughVisit();

        // if (visitCount >= numVisits)
        // {
            
        // }
    }

    // Starts next day -- triggered by button
    public void OnClickNextDay()
    {
        ClearCircuit();
        // GenerateMonsters();
        kingsMessage.SetActive(false);
        endOfDayPanel.SetActive(false);
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
        kingPleasedPerVisit.Add(kingWantsGift == finalOutput);
        

        // theKing.AfterNight();
        
    }

    // Clears circuit after night
    public void ClearCircuit()
    {
        List<CircuitNode> toRemove = new List<CircuitNode>();
        foreach (CircuitNode node in circuitNodes)
        {
            
            node.ClearNode();
            if (!gates.Contains(node))
            {
                toRemove.Add(node);
            }
            

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
            // else if (gates.Contains(node))
            // {
            //     gates.Remove(node);
            // }
        }
        visitCount = 0;

    }
}
