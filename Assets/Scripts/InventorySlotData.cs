using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class InventorySlotData
{
    public InteractableObject item;
    public ItemData itemData;
    public int amount;

    public InventorySlotData(InteractableObject item, int amount) {
        this.item = item;
        this.amount = amount;
        this.itemData = item.itemData;
    }

}
