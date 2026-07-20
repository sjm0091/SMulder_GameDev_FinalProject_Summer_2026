using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TheKing : MonoBehaviour
{
    public bool wantsGift = false;
    public bool getsGift = false;
    public GameObject kingsMessage;
    public TextMeshProUGUI bannerText;
    public ScoreKeeper scoreKeeper;
    public bool survivedNight;
    public string surviveText = "The King is happy with his ";
    public string failText = "The King is displeased with his ";
    public WirePlaceMode wireScript;
    public DayNightController dayNightController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreKeeper = GetComponent<ScoreKeeper>();
        wireScript = GetComponent<WirePlaceMode>();
        dayNightController = GetComponent<DayNightController>();
        kingsMessage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AfterNight()
    {
        kingsMessage.SetActive(true);
        if (survivedNight)
        {
            Survive();
        } else
        {
            Fail();
        }
    }

    public void Survive()
    {
        bannerText.text = surviveText + (wantsGift ? "gift!" : "lack of a gift!");
        scoreKeeper.AddNightSurvived();
    }

    public void Fail()
    {
        bannerText.text = failText + (wantsGift ? "lack of a gift..." : "gift...");
        scoreKeeper.RemoveLife();
    }

    public void OnClickNextDay()
    {
        kingsMessage.SetActive(false);
        wireScript.ClearArea();
        dayNightController.StartDay();

    }
}
