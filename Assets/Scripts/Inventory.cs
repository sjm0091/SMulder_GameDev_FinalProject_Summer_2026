using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using NUnit.Framework.Constraints;

public class Inventory : MonoBehaviour
{
    public InventorySlotData[] slotDataList;
    public InventorySlotUI[] slotUIList; // assign in editor
    public GameObject inventoryUI;
    public InteractableObject forTesting;
    public WirePlaceMode wireScript;
    private List<string> gateNames = new List<string>();
    public List<Button> buttons = new List<Button>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gateNames.Add("NANDSpot");
        gateNames.Add("ANDSpot");
        gateNames.Add("ORSpot");
        gateNames.Add("NORSpot");
        gateNames.Add("XORSpot");
        gateNames.Add("XNORSpot");
        gateNames.Add("NOTSpot");
        slotDataList = new InventorySlotData[slotUIList.Length];

        // Set slotData for each slot to null
        for (int i = 0; i < slotDataList.Length; i++)
        {
            slotDataList[i] = null;
        }
        inventoryUI.SetActive(false);
        UpdateUI();

        //For testing
        // AddItem(forTesting, 1);
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
    public void AddItem(InteractableObject item, int amount, GameObject charPrefab = null)
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
        
        Debug.Log("gateNames.Contains(item.itemData.itemName): " + gateNames.Contains(item.itemData.itemName));
        if (gateNames.Contains(item.itemData.itemName))
        {
            foreach(Button button in buttons)
            {
                if (button.name.Contains(item.itemData.gateName))
                {
                    button.gameObject.SetActive(true);
                    if (charPrefab != null)
                    {
                        Debug.Log("Adding char prefab");
                        wireScript.charSpotPrefab = charPrefab;
                    }
                    
                    
                }
            }

        }
        //         List<TMP_Dropdown.OptionData> dropdownData = new List<TMP_Dropdown.OptionData>();
        //         if (wireScript.dropdown.options != null)
        //         {
        //             foreach (TMP_Dropdown.OptionData option in wireScript.dropdown.options)
        //         {
        //             dropdownData.Add(option);
        //         }
        //         }
                
        //         TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData(item.itemData.itemName);
        //         dropdownData.Add(newOption);
        //         wireScript.dropdown.AddOptions(dropdownData);
        //         Debug.Log("add options completed");
        //     }

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
