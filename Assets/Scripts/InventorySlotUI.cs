using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI stackText;

    public void SetSlot(ItemData item, int amount)
    {
        Debug.Log("set slot");
        Debug.Log("amount: " + amount);
        Debug.Log("item: " + item.itemName);
        
        itemIcon.sprite = item.itemIcon;
        itemIcon.gameObject.SetActive(true);
        stackText.text = amount.ToString();
        stackText.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        Debug.Log("clear slot");
        // if (itemIcon == null || stackT)
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);

        stackText.text = "";
        stackText.gameObject.SetActive(false);
       
        
    }
    
}
