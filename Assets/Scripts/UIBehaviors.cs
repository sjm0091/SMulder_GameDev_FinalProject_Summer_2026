using UnityEngine;
using UnityEngine.InputSystem;

public class UIBehaviors : MonoBehaviour
{
    public Menu gameManager;
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
}
