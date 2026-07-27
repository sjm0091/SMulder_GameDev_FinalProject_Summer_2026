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
    public List<CircuitNode> circuitNodes = new List<CircuitNode>();
    List<GameObject> characters = new List<GameObject>();
    public WirePlaceMode wireScript;
    public NightBehavior nightBehavior;
    public bool finalOutput;
    public JudgementController judgementController;
    public AudioSource audioSource;
    public AudioClip ButtonClickClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera.enabled = true;
        nightCamera.enabled = false;
        theKing = GetComponent<TheKing>();
        nightBehavior = GetComponent<NightBehavior>();
        judgementController = GetComponent<JudgementController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        audioSource.PlayOneShot(ButtonClickClip);
        if (wireScript.finalNode == null)
        {
            return;
        }

        nightCamera.enabled = true;
        mainCamera.enabled = false;

        isDay = false;
        sun.gameObject.SetActive(false);

        Debug.Log("Night Begun");

        foreach (CircuitNode node in circuitNodes)
        {
            node.StartNight();
        }

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
        theKing.kingsMessage.SetActive(true);
        // judgementController.CalculateEndResults(finalOutput);

        // theKing.AfterNight();
        
    }

    public void ClearCircuit()
    {
        List<CircuitNode> toRemove = new List<CircuitNode>();
        foreach (CircuitNode node in circuitNodes)
        {
            node.ClearNode();

        }
        foreach (CircuitNode gate in nightBehavior.gateList)
        {
            gate.ClearNode();
        }

        foreach (CircuitNode node in toRemove)
        {
            if (circuitNodes.Contains(node))
            {
                circuitNodes.Remove(node);
            }
            else if (nightBehavior.gateList.Contains(node))
            {
                nightBehavior.gateList.Remove(node);
            }
        }

    }


}
