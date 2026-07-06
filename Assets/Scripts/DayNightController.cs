using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public GameObject sun;
    public bool day = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        day = false;
        sun.gameObject.SetActive(false);
    }

    public void StartDay() // TODO: implement when night routines end
    {
        day = true;
        sun.gameObject.SetActive(true);
    }


}
