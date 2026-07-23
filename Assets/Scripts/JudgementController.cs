using TMPro;
using UnityEngine;

public class JudgementController : MonoBehaviour
{
    // public TextMeshProUGUI judgementMessage;
    public ScoreKeeper scoreKeeperManager;
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
        
        bool survivedNight = CheckIfSurvived(finalOutput, kingWantsGift);
        string judgementText = DetermineJudgementText(survivedNight, kingWantsGift);

        bannerText.text = judgementText;

        switch (survivedNight)
        {
            case true:
                scoreKeeperManager.AddNightSurvived();
                break;
            case false:
                scoreKeeperManager.RemoveLife();
                break;
        }
    }

    private bool CheckIfSurvived(bool finalOutput, bool kingWantsGift)
    {
        return finalOutput == kingWantsGift;
    } 

    private string DetermineJudgementText(bool survived, bool kingWantsGift)
    {
        string pleasedText = survived ? " pleased with his ": " displeased with his ";
        string giftText = ((kingWantsGift && survived) || (!kingWantsGift && !survived)) ? " gift " : " lack of a gift ";
        string punctuationText = survived ? "!" : "...";

        string judgementText = "The King is " + pleasedText + giftText + punctuationText;

        return judgementText;
    }
}
