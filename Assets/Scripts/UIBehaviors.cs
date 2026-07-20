using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIBehaviors : MonoBehaviour
{
    public Menu gameManager;
    public GameObject storyPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
