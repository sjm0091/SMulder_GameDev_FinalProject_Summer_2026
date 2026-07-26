using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class JudgementController : MonoBehaviour
{
    // public TextMeshProUGUI judgementMessage;
    public ScoreKeeper scoreKeeperManager;
    public NPCController npcController;
    // public bool finalOutput;
    public bool gate1HasGift; // if false -> monster ate gift
    public bool gate2HasGift; // if false -> monster ate gift
    // public bool kingWantsGift; // set by NightBehavior.GenerateMonsters();
    public TextMeshProUGUI bannerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateEndResults(bool finalOutput, bool kingWantsGift)
    {

        bool survivedNight = CheckIfSurvivedVisit(finalOutput, kingWantsGift);
        string judgementText = DetermineJudgementText(survivedNight, kingWantsGift);

        bannerText.text = judgementText;
    }

    private bool CheckIfSurvivedVisit(bool finalOutput, bool kingWantsGift)
    {
        return finalOutput == kingWantsGift;
    } 

    private string DetermineJudgementText(bool survivedVisit, bool kingWantsGift)
    {
        string pleasedText = survivedVisit ? " pleased with his ": " displeased with his ";
        string giftText = ((kingWantsGift && survivedVisit) || (!kingWantsGift && !survivedVisit)) ? "gift" : "lack of a gift";
        string punctuationText = survivedVisit ? "!" : "...";

        string judgementText = "The King is" + pleasedText + giftText + punctuationText;

        return judgementText;
    }

    public string DetermineFinalJudgementText(bool survivedNight)
    {
        string text = survivedNight ? "Congratulations! The King approves of your presence in his kingdom!" : "The King disapproves of your presence. However, you may stay, for now...";

        return text;
    }

    public KeyValuePair<bool, string> CheckIfSurvivedNight(List<bool> survivedList)
    {
        bool survived = true;
        Debug.Log("list of pleasedking");
        foreach (bool pleasedKing in survivedList)
        {
            Debug.Log("pleasedKing: " + pleasedKing);
            if (!pleasedKing)
            {
                survived = false;
            }
        }

        switch (survived)
        {
            case true:
                scoreKeeperManager.AddNightSurvived();
                break;
            case false:
                scoreKeeperManager.RemoveLife();
                break;
        }

        Debug.Log("Survived = " + survived);

        string text = DetermineFinalJudgementText(survived);

        KeyValuePair<bool, string> response = new KeyValuePair<bool, string>(survived, text);


        return response;
    }
}
