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
    List<GameObject> characters = new List<GameObject>();
    public WirePlaceMode wireScript;
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
        if (wireScript.finalNode == null)
        {
            return;
        }

        nightCamera.enabled = true;
        mainCamera.enabled = false;

        isDay = false;
        sun.gameObject.SetActive(false);

        Debug.Log("Night Begun");

        StartCoroutine(WaitForCircuitToFinish());
    }

    public void StartDay() 
    {
        // theKing.ready = null;
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

        Debug.Log("the king wants: " + theKing.wantsGift);
        // Debug.Log("final output: " + );

        

        theKing.AfterNight();
        
    }


}
