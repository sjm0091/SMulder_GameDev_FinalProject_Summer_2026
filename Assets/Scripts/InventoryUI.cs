using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventory;

    private bool isOpen = false;
    public void OnInventory(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        ToggleInventory();
        
    }

    private void ToggleInventory()
    {
        isOpen = !isOpen;
        inventory.SetActive(isOpen);
    }
}
