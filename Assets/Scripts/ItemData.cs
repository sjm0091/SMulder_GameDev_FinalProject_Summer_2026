using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string gateName = "null";
    public Sprite itemIcon;
    public int maxStackSize;
}
