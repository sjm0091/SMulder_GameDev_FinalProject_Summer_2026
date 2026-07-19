using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlotData[] slotDataList;
    public InventorySlotUI[] slotUIList; // assign in editor
    public GameObject inventoryUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slotDataList = new InventorySlotData[slotUIList.Length];

        // Set slotData for each slot to null
        for (int i = 0; i < slotDataList.Length; i++)
        {
            slotDataList[i] = null;
        }
        inventoryUI.SetActive(false);
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slotUIList.Length; i++)
        {
            if (slotDataList[i] != null)
            {
                Debug.Log("item = " + slotDataList[i]);
                slotUIList[i].SetSlot(slotDataList[i].item.itemData, slotDataList[i].amount);
            }
            else
            {
                slotUIList[i].ClearSlot();
                slotDataList[i] = null;
            }
        }

    }

    // add item to inventory slot(s)
    public void AddItem(InteractableObject item, int amount)
    {
        bool found = false;
        // check if item already in inventory
        for (int i = 0; i < slotUIList.Length; i++)
        {
            // if empty slot
            if (slotDataList == null || slotDataList[i] == null)
            {
                continue;
            }
            
            // if same item
            if (slotDataList[i].itemData == item.itemData)
            {
                found = true;
                break;
            }
        }

        // if item already in inventory, add to current stacks
        if (found)
        {
            Debug.Log("found");
            for (int i = 0; i < slotUIList.Length; i++)
            {
                if (slotDataList[i] != null && slotDataList[i].itemData == item.itemData && slotDataList[i].amount < item.maxStackAmount)
                {
                    int available = item.maxStackAmount - slotDataList[i].amount;
                    
                    // amountToSet = current amount + added amount because setslot sets number (not adds)
                    int amountToSet = available > amount ? amount + slotDataList[i].amount : available + slotDataList[i].amount;

                    slotDataList[i].amount = amountToSet;

                    amount = available > amount ? 0 : amount - available;
                    
                    // Done adding
                    if (amount <= 0)
                    {
                        UpdateUI();
                        return;
                    }
                }
            }
        } 
        Debug.Log("slotDataList: " + slotDataList);
        // if no non-full slots with item (put in next empty slot)
        for (int i = 0; i < slotDataList.Length; i++)
        {
            if (slotDataList[i] != null)
            {
                continue;
            }

            int amountToSet = item.maxStackAmount > amount ? amount : item.maxStackAmount;

            // add slot data
            InventorySlotData newSlotData = new InventorySlotData(item, amountToSet);
            slotDataList[i] = newSlotData;
            Debug.Log(slotDataList[i].item);
            // slotDataList[i].itemData = item.itemData;


            // slotDataList[i].amount = amountToSet;

            amount = item.maxStackAmount > amount ? 0 : amount - item.maxStackAmount;

            // Done adding
            if (amount <= 0)
            {
                UpdateUI();
                return;
            }
        }

        UpdateUI();
        
        
    }

    public void RemoveItem(ItemData itemData)
    {
        Debug.Log("Remove Item triggered");
        for (int i = slotDataList.Length - 1; i >= 0; i--)
        {
            if (slotDataList[i] == null)
            {
                Debug.Log("slotDataList[i] == null: " + i);
                continue;
            }
            Debug.Log("Remove itemData name?: " + slotDataList[i].item.itemData.name);
            if (slotDataList[i].item.itemData == itemData)
            {
                slotDataList[i].amount--;
                if (slotDataList[i].amount <= 0)
                {
                    slotDataList[i] = null;
                }
            }
        }
    }

    public Dictionary<ItemData, int> GetItemsInInventory()
    {
        Debug.Log("Get items in inventory triggered.");
        Dictionary<ItemData, int> myDict = new Dictionary<ItemData, int>();
        for (int i = 0; i < slotDataList.Length; i++)
        {
            if (slotDataList[i] == null)
            {
                Debug.Log(i + ": slot data is null");
                continue;
            }
            if (myDict.Keys.Contains(slotDataList[i].itemData))
            {
                myDict[slotDataList[i].itemData] += slotDataList[i].amount;
                Debug.Log("exists: myDict[" + slotDataList[i].itemData.name + "]: " + slotDataList[i].amount);
            } else
            {
                myDict[slotDataList[i].itemData] = slotDataList[i].amount;
                Debug.Log("!exists: myDict[" + slotDataList[i].itemData.name + "]: " + slotDataList[i].amount);
            }
        }
        
        return myDict;
    }
}
