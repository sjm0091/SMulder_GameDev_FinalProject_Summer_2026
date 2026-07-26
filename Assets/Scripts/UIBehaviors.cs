using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIBehaviors : MonoBehaviour
{
    public Menu gameManager;
    public GameObject storyPanel;

    public GameObject helpPanel;

    public List<GameObject> introMessages = new List<GameObject>();
    public int introMessagesIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNextIntroMessage()
    {
        if (introMessagesIndex > introMessages.Count)
        {
            introMessages[introMessagesIndex - 1].SetActive(false);
            return;
        }
        if (introMessagesIndex == 0)
        {
            introMessages[introMessagesIndex].SetActive(true);
            introMessagesIndex++;
            return;
        }
        introMessages[introMessagesIndex - 1].SetActive(false);
        introMessages[introMessagesIndex].SetActive(true);

        introMessagesIndex++;
    }

    public void OnClickClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void OnClickTogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }



    public void OnClickHelp()
    {
        helpPanel.SetActive(!helpPanel.activeSelf);
    }

    public void OnMenu(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        gameManager.OnMenuOpen();
    }

    public void OnTeleport(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        TeleportController tpController = gameManager.GetComponent<TeleportController>();
        tpController.Teleport();
    }

    public void ToggleStoryText()
    {
        storyPanel.SetActive(!storyPanel.activeSelf);
    }
}
