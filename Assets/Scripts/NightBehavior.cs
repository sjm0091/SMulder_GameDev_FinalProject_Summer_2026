// using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class NightBehavior : MonoBehaviour
{
    public DayNightController gameManager;
    public List<CircuitNode> gateList = new List<CircuitNode>();
    // public Dictionary<string, bool> monsterList = new Dictionary<string, bool>();
    public bool monster1;
    public bool monster2;
    public int numMonsters = 2;
    public TextMeshProUGUI infoText;
    public bool kingWantsGift;
    // pbli
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMonsters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMonsters()
    {
        // currently 2
        for (int i = 0; i < gateList.Count; i++)
        {
            bool isYes = Random.value > 0.5 ? true : false;
            Debug.Log("isYes: " + isYes);
            
            // when monster is true, input value should be false
            gateList[i].AddInputValue(true);
            gateList[i].AddInputValue(isYes);
            if (i == 0)
            {
                monster1 = isYes;
            }
            if (i == 1)
            {
                monster2 = isYes;
            }
        }

        bool wantsGift = Random.value > 0.5 ? true : false;
        kingWantsGift = wantsGift;

        infoText.text = "Nighttime Information \n\nGate 1: " + (monster1 ? "monster" : "safe") + " \nGate 2: " + (monster2 ? "monster" : "safe") + "\n\nKing desires: " + (kingWantsGift ? "gift" : "no gift");

    }

    
}
