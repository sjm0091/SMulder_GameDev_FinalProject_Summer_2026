// using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class NightBehavior : MonoBehaviour
{
    public JudgementController judgementController;
    public DayNightController gameManager;
    public List<CircuitNode> gateList = new List<CircuitNode>();
    // public Dictionary<string, bool> monsterList = new Dictionary<string, bool>();
    public bool monster1;
    public bool monster2;
    public int numMonsters = 2;
    public TextMeshProUGUI infoText;
    public bool kingWantsGift;
    public TheKing theKing;
    // pbli
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        theKing = GetComponent<TheKing>();
        GenerateMonsters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Run at start of day
    public void GenerateMonsters()
    {
        Debug.Log("Generating Monsters");
        // currently 2
        for (int i = 0; i < gateList.Count; i++)
        {
            Debug.Log("GM, i = " + i);
            bool isMonster = Random.value > 0.5 ? true : false;
            Debug.Log("isMonster: " + isMonster);
            
            // when monster is true, input value should be false
            gateList[i].AddInputValue(isMonster);
            gateList[i].AddInputValue(isMonster);
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

        judgementController.gate1HasGift = gateList[0];
        judgementController.gate2HasGift = gateList[1];

        bool wantsGift = Random.value > 0.5 ? true : false;
        theKing.wantsGift = wantsGift;
        kingWantsGift = wantsGift;
        Debug.Log("wantsGift: " + wantsGift);
        Debug.Log("theKing.wantsGift: " + theKing.wantsGift);

        // judgementController.kingWantsGift = kingWantsGift;

        infoText.text = "Nighttime Information \n\nGate 1: " + (monster1 ? "monster" : "safe") + " \nGate 2: " + (monster2 ? "monster" : "safe") + "\n\nKing desires: " + (kingWantsGift ? "gift" : "no gift");

    }

    // public IEnumerator RunCircuit(List<CircuitNode> nodes, List<CircuitNode> gates)
    // {
    //     Dictionary<CircuitNode, List<bool>> gateOutputs = new Dictionary<CircuitNode, List<bool>>();
    //     // for each gate
    //     foreach(CircuitNode gate in gates)
    //     {
    //         // for each node that is saved as an output for this gate
    //         foreach(CircuitNode outputNode in gate.outputs)
    //         {
    //             if (gateOutputs.ContainsKey(outputNode))
    //             {
    //                 gateOutputs[outputNode].Add(gate.output);
    //             } else
    //             {
    //                 List<bool> boolList = new List<bool>();
    //                 boolList.Add(gate.output);
    //                 gateOutputs.Add(outputNode, boolList);
    //             }
                
    //         }
    //     }

    //     // add outputs to input lists for first row of nodes
    //     foreach(CircuitNode node in nodes)
    //     {
    //         if (gateOutputs.ContainsKey(node))
    //         {
    //             foreach(bool value in gateOutputs[node])
    //             {
    //                 node.AddInputValue(value);
    //             }
    //         }
    //     }
    // }


}
