using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor;

public class NightTimeGameManager : MonoBehaviour
{
    // Other Scripts
    public JudgementController judgementController;
    public WirePlaceMode wireScript;
    public NPCController npcController;
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
    public bool forGate;

    // Set by FinalNode (CircuitNode script)
    public bool finalOutput;

    
    // UI
    public TextMeshProUGUI infoText;
    public GameObject kingsMessage;

    // Audio
    public AudioSource audioSource;
    public AudioClip ButtonClickClip;
    public AudioClip UIButtonClick;


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
    public int nightsCount;


    // Characters
    public List<GameObject> characterPrefabs = new List<GameObject>();
    public GameObject nextCharacter;

    // Outputs
    public Dictionary<GameObject, bool[]> outputPossibilities = new Dictionary<GameObject, bool[]>();
    

    public void Start()
    {
        bool[] list1 = {true, false, false, false}; // NOR
        bool[] list2 = {false, false, false, true}; // AND
        bool[] list3 = {true, false, false, true}; // XNOR
        bool[] list4 = {false, true, true, false}; // XOR
        bool[] list5 = {false, true, true, true}; // OR
        bool[] list6 = {true, true, true, false}; // NAND
        bool[] list7 = {true, false}; // NOR

        outputPossibilities.Add(characterPrefabs[0], list1);
        outputPossibilities.Add(characterPrefabs[1], list2);
        outputPossibilities.Add(characterPrefabs[2], list3);
        outputPossibilities.Add(characterPrefabs[3], list4);
        outputPossibilities.Add(characterPrefabs[4], list5);
        outputPossibilities.Add(characterPrefabs[5], list6);
        outputPossibilities.Add(characterPrefabs[6], list7);

        wireScript = GetComponent<WirePlaceMode>();
        judgementController = GetComponent<JudgementController>();
        // GenerateMonsters();
        GenerateOutputs();
        // Debug.Log("start day set ");
        SetUpForNOT(forGate);
        visitCount = 0;

        npcController.SpawnAvailableCharacters();
    }
   
    
    // Run at start of day
    //     public void GenerateMonsters()
    // {
    //     Debug.Log("Generating Monsters");
    //     // currently 2
    //     for (int i = 0; i < gates.Count; i++)
    //     {
    //         Debug.Log("GM, i = " + i);
    //         bool isMonster = Random.value > 0.5 ? true : false;
    //         Debug.Log("isMonster: " + isMonster);
            
    //         // when monster is true, input value should be false
    //         gates[i].AddInputValue(isMonster);
    //         gates[i].AddInputValue(isMonster);
    //         if (i == 0)
    //         {
    //             monster1 = isMonster;
    //             Debug.Log("GM, monster1: " + isMonster);
    //         }
    //         if (i == 1)
    //         {
    //             monster2 = isMonster;
    //             Debug.Log("GM, monster2: " + isMonster);
    //         }
    //     }

    //     judgementController.gate1HasGift = gates[0];
    //     judgementController.gate2HasGift = gates[1];

    //     bool wantsGift = Random.value > 0.5 ? true : false;
    //     // theKing.wantsGift = wantsGift;
    //     kingWantsGift = wantsGift;
    //     Debug.Log("wantsGift: " + wantsGift);
    //     // Debug.Log("theKing.wantsGift: " + theKing.wantsGift);

    //     infoText.text = "Nighttime Information \n\nGate 1: " + (monster1 ? "monster" : "safe") + " \nGate 2: " + (monster2 ? "monster" : "safe") + "\n\nKing desires: " + (kingWantsGift ? "gift" : "no gift");

    // }



    // Run by - StartDay() & RunThroughVisit()
    // Functions:
    // - adds inputs (set) to gates - Handles Not gate
    public void SetGateInputs(int visitNum)
    {
        Debug.Log("Set Gate Inputs triggered. Visit #" + visitNum);

        if (forGate)
        {
            SetUpForNOT(true);
            Debug.Log("NOT gate");
            numVisits = 2;
            switch(visitCount + 1)
            {
                case 1:
                    gates[0].AddInputValue(true);
                    gates[0].AddInputValue(true);
                    break;
                case 2:
                    gates[0].AddInputValue(false);
                    gates[0].AddInputValue(false);
                    break;
            }
            judgementController.gate1HasGift = gates[0];
            return;
        }

        SetUpForNOT(false);
        numVisits = 4;
        // visitNum = 4;
        switch(visitCount + 1)
        {
            case 1:
                Debug.Log("Case 1");
                gates[0].AddInputValue(true);
                gates[0].AddInputValue(true);
                gates[1].AddInputValue(true);
                gates[1].AddInputValue(true);
                break;
            case 2:
                Debug.Log("Case 2");
                gates[0].AddInputValue(true);
                gates[0].AddInputValue(true);
                gates[1].AddInputValue(false);
                gates[1].AddInputValue(false);
                break;
            case 3:
                Debug.Log("Case 3");
                gates[0].AddInputValue(false);
                gates[0].AddInputValue(false);
                gates[1].AddInputValue(true);
                gates[1].AddInputValue(true);
                break;
            case 4:
                Debug.Log("Case 4");
                gates[0].AddInputValue(false);
                gates[0].AddInputValue(false);
                gates[1].AddInputValue(false);
                gates[1].AddInputValue(false);
                break;
        }

        judgementController.gate1HasGift = gates[0];
        judgementController.gate2HasGift = gates[1];

    }

    // Run By - Start() & SetGateInputs()
    // Functions:
    // - Sets input sprites - handles NOT gate
    public void SetUpForNOT(bool notGate)
    {
        for(int i = 0; i < gridCellsByGate.Count; i++)
        {
            gridCellsByGate[i].gameObject.SetActive(true);
            if (i < outputGridCells.Count)
            {
                outputGridCells[i].gameObject.SetActive(true);
            }
            
        }
        Debug.Log("Set up for not triggered");
        Debug.Log("Set up for not: " + notGate);
        if (notGate)
        {
            Debug.Log("Set up for not: " + notGate);
            for (int i = 0; i < gridCellsByGate.Count; i++)
            {
                Debug.Log("grid cell " + i);
                switch (i + 1)
                {
                    
                    case 1:
                        gridCellsByGate[i].sprite = noGiftSprite;
                        break;
                    case 2:
                        gridCellsByGate[i].sprite = noGiftSprite;
                        break;
                    case 3:
                        gridCellsByGate[i].sprite = giftSprite;
                        outputGridCells[i].gameObject.SetActive(false);
                        break;
                    case 4:
                        gridCellsByGate[i].sprite = giftSprite;
                        outputGridCells[i].gameObject.SetActive(false);
                        break;
                    default:
                        Debug.Log("setting cell " + i + " to null and inactive");
                        Debug.Log("name: " + gridCellsByGate[i].name);
                        gridCellsByGate[i].sprite = null;
                        gridCellsByGate[i].gameObject.SetActive(false);
                        break;
                }
            }
        } else
        {
            for (int i = 0; i < gridCellsByGate.Count; i++)
            {
                switch (i + 1)
                {
                    case 1:
                        gridCellsByGate[i].sprite = noGiftSprite;
                        break;
                    case 2:
                        gridCellsByGate[i].sprite = noGiftSprite;
                        break;
                    case 3:
                        gridCellsByGate[i].sprite = noGiftSprite;
                        break;
                    case 4:
                        gridCellsByGate[i].sprite = giftSprite;
                        break;
                    case 5:
                        gridCellsByGate[i].sprite = giftSprite;
                        break;
                    case 6:
                        gridCellsByGate[i].sprite = noGiftSprite;
                        break;
                    case 7:
                        gridCellsByGate[i].sprite = giftSprite;
                        break;
                    case 8:
                        gridCellsByGate[i].sprite = giftSprite;
                        break;
                    default:
                        gridCellsByGate[i].sprite = null;
                        break;
                }
            }
        }
    }

    
    // Run by - Start & StartDay()
    // Determines on which visits the king wants a gift
    // Functions:
    // - gets random output (or: 1st night: 0, 2nd night: 1) (gates)
    // - sets GiftsByVisit
    // - Gets random WantsGift
    public void GenerateOutputs()
    {
        Debug.Log("generate outputs triggered");
        int index = Random.Range(0, outputPossibilities.Count - 1);
        // int index = 6; // for testing NOT gate

        if (nightsCount == 0)
        {
            index = 5; // Start with NAND gate
        }
        if (nightsCount == 1)
        {
            index = 6; // Then do NOT gate
        }

        // Proceed randomly

        // Debug.Log("Gate: " + characterPrefabs[index].GetComponent<InteractableObject>().itemData.gateName);

        if (index == 6)
        {
            gates[1].gameObject.SetActive(false);
            forGate = true;
        } else
        {
            gates[1].gameObject.SetActive(true);
            forGate = false;
        }



        nextCharacter = characterPrefabs[index];
        bool[] outputList = outputPossibilities[characterPrefabs[index]];
        foreach (bool output in outputList)
        {
            Debug.Log("output: " + output);
        }

        List<bool> giftsByVisit = new List<bool>();
        for (int i = 0; i < outputList.Length; i++)
        {
            // bool wantsGift = Random.value > 0.5f ? true : false;

            bool wantsGift = outputList[i];

            Sprite newSprite = wantsGift ? giftSprite : noGiftSprite;
            outputGridCells[i].sprite = newSprite;
             
            giftsByVisit.Add(wantsGift);
        }
        for (int i = outputList.Length; i < 4;i++)
        {
            outputGridCells[i].sprite = null;
        }

        wantsGiftByVisit = giftsByVisit; 
    }


    // Run by - StartNight() & OnClickNextVisit()
    // Functions:
    // - Handles 1 visit
    // - Handles surviving after all visits
    // - Adds new character after all visits
    // - sets gate input (function)
    // - starts night for all nodes
    // - waits for each routine to finish (function)
    // - increments VisitCount
    public void RunThroughVisit()
    {
        Debug.Log("numVisits: " + numVisits);
        Debug.Log("visitCount: " + visitCount);
        if (visitCount >= numVisits)
        {
            
            KeyValuePair<bool, string> response = judgementController.CheckIfSurvivedNight(kingPleasedPerVisit);

            bool survived = response.Key;
            if (!survived)
            {
                nightsCount--; // try day again
            }
            else
            {
                GameObject newChar = null;
                switch (nightsCount)
                {
                    case 0:
                        Debug.Log("no new character");
                        newChar = null;
                        break;
                    case 1:
                        Debug.Log("Adding NOT");
                        newChar = characterPrefabs[6];
                        break;
                    default:
                        int ctr = 0;
                        bool chosen = false;
                        // if (nightsCount == 0)
                        // {
                        //     newChar = characterPrefabs[0];
                        // }
                        // else if (nightsCount == 1)
                        // {
                        //     newChar = characterPrefabs[6];
                        // }
                        while (!chosen)
                        {
                            if (ctr > 7)
                            {
                                break;
                            }
                            int index = Random.Range(0, characterPrefabs.Count - 1);
                            
                            if (!npcController.UnlockedCharacter(characterPrefabs[index]))
                            {
                                chosen = true;
                            }
                            if (chosen)
                            {
                                newChar = characterPrefabs[index];
                            }
                            
                            ctr++;
                        }
                        
                        break;
                }
                if (newChar != null)
                {
                    Debug.Log("new character added");
                        npcController.AddCharacter(newChar);
                }
                
            }

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
            if (gates.Contains(node) && !node.gameObject.activeSelf) // skips inactive nodes (for NOT gate)
            {
                continue;
            }
            else if (gates.Contains(node))
            {
                
            }
            node.StartNight();
        }

        StartCoroutine(WaitForCircuitToFinish());
        visitCount++;
        
    }


    // Run by - RunThroughVisit()
    // Functions:
    // activates end of day panel and text
    public void SetEndOfDayText(string text)
    {
        endOfDayPanel.SetActive(true);
        endOfDayMessage.text = text;
    }


    // Run By - Start Night Button
    // Begins nighttime procedures -- triggered by button
    // Functions:
    // - switches cameras
    // - disables sun
    // - Runs through first visit (function)
    public void StartNight()
    {
        
        if (wireScript.finalNode == null)
        {
            return;
        }
        audioSource.PlayOneShot(UIButtonClick);

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

    // Run By - OnClickNextDay()
    // Begins day in main scene
    // Functions:
    // - switches cameras
    // - enables sun
    // - Generates  outputs and wants gates (function)
    // - Sets the gate inputs visit 1 and kingwants (function)
    public void StartDay() 
    {
        nightsCount++;
        // theKing.ready = null;
        mainCamera.enabled = true;
        nightCamera.enabled = false;

        isDay = true;
        sun.gameObject.SetActive(true);

        GenerateOutputs();
        if (forGate)
        {
            SetUpForNOT(true);
        } else
        {
            SetUpForNOT(false);
        }

        npcController.SpawnAvailableCharacters();
        
        // SetGateInputs(1);


    }

    // Run By - Next Visit Button
    // Functions:
    // - clears nodes after visit (function)
    // - runs through visit (function)
    // - disables kings message
    public void OnClickNextVisit()
    {
        audioSource.PlayOneShot(UIButtonClick);

        foreach (CircuitNode node in circuitNodes)
        {
            node.ClearNodeAfterVisit();
        }

        kingsMessage.SetActive(false);

        // if (visitCount >= numVisits)
        // {
        //     return;
        // }

        RunThroughVisit();

        
    }

    // Run By - Next Day Button
    // Starts next day -- triggered by button
    // Functions:
    // - clears circuit (function)
    // - sets messages inactive
    // - sets num of nodes to 0
    // - starts day (function)
    public void OnClickNextDay()
    {
        audioSource.PlayOneShot(UIButtonClick);
        ClearCircuit();
        kingPleasedPerVisit = new List<bool>();
        // GenerateMonsters();
        kingsMessage.SetActive(false);
        endOfDayPanel.SetActive(false);
        wireScript.ClearArea();
        wireScript.numCharNodes = 0;
        npcController.ClearAfterDay();


        
        StartDay();

    }

    // Run By - RunThroughVisit
    // Runs until all nodes are done. Gets Results
    // Functions:
    // - waits for all nodes to set actionsCompleted to true
    // - sets kingsMessage active
    // - calculates results for visit (function)
    // - saves whether pleased this visit
    IEnumerator WaitForCircuitToFinish()
    {
        Debug.Log("all active circuitnodes: ");
        foreach (CircuitNode node in circuitNodes)
        {
            if (node.gameObject.activeSelf)
            {
                Debug.Log("node: " + node.nodeName);
            }
            
        }
        bool notDone = true;
        while (notDone)
        {
            notDone = false;
            foreach(CircuitNode node in circuitNodes)
            {
                if (node.actionsCompleted == false)
                {
                    if (!node.gameObject.activeSelf)
                    {
                        continue;
                    }
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

    // Run By - OnClickNextDay()
    // Clears circuit after night
    // Functions:
    // - calls ClearNode on all circuit nodes
    // - removes all nodes except for gate nodes from circuitNodes
    // - sets visit count back to 0
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

    public void OnClickClearCircuit()
    {
        audioSource.PlayOneShot(UIButtonClick);
        ClearCircuit();
        wireScript.ClearArea();
        wireScript.numCharNodes = 0;
        wireScript.wireStart = true;
    }
}
