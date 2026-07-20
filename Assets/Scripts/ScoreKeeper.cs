using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.Video;

public class ScoreKeeper : MonoBehaviour
{
    public int life = 3;
    public int nightsSurvived = 0;
    public int nightGoal = 5;
    public GameObject winScreen;
    public GameObject loseScreen;
    public TextMeshProUGUI nightsSurvivedText;
    public TextMeshProUGUI livesText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNightSurvived()
    {
        nightsSurvived++;
        nightsSurvivedText.text = "Nights survived: " + nightsSurvived;
        if (nightsSurvived >= nightGoal)
        {
            Win();
        }
    }

    public void RemoveLife()
    {
        life--;
        livesText.text = "Lives: " + life;
        if (life <= 0)
        {
            Lose();
        }
    }

    public void Win()
    {
        winScreen.SetActive(true);
    }

    public void Lose()
    {
        loseScreen.SetActive(true);
    }
}
