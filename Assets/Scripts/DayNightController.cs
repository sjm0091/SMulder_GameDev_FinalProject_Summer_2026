using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DayNightController : MonoBehaviour
{
    public Camera mainCamera;
    public Camera nightCamera;
    public GameObject sun;
    public bool isDay = true;
    public TheKing theKing;
    List<CircuitNode> circuitNodes = new List<CircuitNode>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera.enabled = true;
        nightCamera.enabled = false;
        theKing = GetComponent<TheKing>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        nightCamera.enabled = true;
        mainCamera.enabled = false;

        isDay = false;
        sun.gameObject.SetActive(false);

        Debug.Log("Night Begun");

        StartCoroutine(WaitForCircuitToFinish());
    }

    public void StartDay() // TODO: implement when night routines end
    {
        mainCamera.enabled = true;
        nightCamera.enabled = false;

        isDay = true;
        sun.gameObject.SetActive(true);


    }

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

        

        theKing.AfterNight();
        
    }


}
