using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Compatibility;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionScript : MonoBehaviour
{
    public GateBehaviorScript character;
    private float minDistance = 5f;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI promptText;
    public string currentCharMessage = "Talk [E]";
    public bool talking;
    public InteractableObject currentItem;

    public CharacterBehaviorScript1 currentChar;
    public Inventory inventory;
    private bool isInteracting = false;
    private bool charInteraction = false;
    private bool isCharInteracting = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // inventory.gameObject.SetActive(true);
        // inventory.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // character = FindNearestCharacter();
        // if (character == null)
        // {
        //     if (messageText != null)
        //     {
        //         messageText.gameObject.SetActive(false);
        //     }
        //     return;
        // }
        // Debug.Log("FOUND");
        // messageText = character.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        // messageText.gameObject.SetActive(true);

        FindNearbyItem();
        FindNearbyItem(true);


    }

    // public void FindNearbyCharacter()
    // {
    // }

    public void FindNearbyItem(bool character = false)
    {
        // Debug.Log("find nearest item triggered ");
        Collider[] hits = Physics.OverlapSphere(transform.position, minDistance);
        // Debug.Log(hits);
        // Debug.Log(hits.Length);
        InteractableObject closestItem = null;
        CharacterBehaviorScript1 closestChar = null;
        float closestDistance = Mathf.Infinity;

        foreach(Collider hit in hits)
        {
            // Debug.Log("Hit: " + hit.gameObject.name);
            CharacterBehaviorScript1 charFound;
            InteractableObject item;
            if (character)
            {
                charFound = null;
                item = currentItem;
            }
            else
            {
                item = null;
                charFound = currentChar;
            }
            
            if (!character)
            {
                // Debug.Log("not character");
                item = hit.gameObject.GetComponent<InteractableObject>();
                if (item == null)
                {
                    continue;
                }
                charInteraction = false;
            } else
            {
                // Debug.Log("character");
                charFound = hit.gameObject.GetComponent<CharacterBehaviorScript1>();
                if (charFound == null)
                {
                    isCharInteracting = false;
                    continue;
                }
                charInteraction = true;
                
            }

            // Debug.Log("yaya!");
            // Debug.Log("character bool: " + character);

            
            

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            
            if (distance < closestDistance && !character)
            {
                // Debug.Log("!character set");
                closestItem = item;
                closestDistance = distance;
            }
            else if (distance < closestDistance && character)
            {
                // Debug.Log("character set");
                closestChar = charFound;
                closestDistance = distance;
            }
        }
        if (character)
        {
            currentChar = closestChar;
        }
        else
        {
            currentItem = closestItem;
        }
        
        

        if (promptText == null)
        {
            // Debug.Log("promptText is NULL");
            return;
        }

        // Debug.Log("currentItem: " + currentItem);
        // Debug.Log("currentChar: " + currentChar);
        // Debug.Log("isInteracting: " + isInteracting);
        if (currentItem != null && !isInteracting)
        {
            // Debug.Log("current Item");
            promptText.text = "Press V To Interact";
            
            promptText.gameObject.SetActive(true);
            // Debug.Log(promptText.text);

        } 
        else if (currentChar != null && !isCharInteracting)
        {
            
            // Debug.Log("current Char");
            // if (promptText.text != currentChar.promptText && promptText.text != currentChar.happyText && promptText.text != currentChar.waitingText && promptText.text != currentChar.queryingText)
            // {
            if (!talking) {
                promptText.text = currentChar.promptText;
                promptText.gameObject.SetActive(true);
            } else
            {
                promptText.text = currentCharMessage;
            }
            
            
        }
        else if (currentChar == null && currentItem == null && !isCharInteracting)
        {
            promptText.gameObject.SetActive(false);
            talking = false;
            Debug.Log("curr char is null & curr item is null: setting prompt text false");
        }
        
    }

    public GateBehaviorScript FindNearestCharacter()
    {
        // Debug.Log("finding nearest char");
        Collider[] hits = Physics.OverlapSphere(transform.position, minDistance);

        GateBehaviorScript closestCharacter = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            // if (!hit.gameObject.CompareTag("Character"))
            // {
            //     Debug.Log("tag not correct");
            //     break;
            // }
            GateBehaviorScript script = hit.GetComponent<GateBehaviorScript>();

            Debug.Log("hit: " + hit);

            if (script == null)
            {
                Debug.Log("tag not correct");
                break;
            }
            Debug.Log("hit: " + hit);

            messageText = script.message;

            GameObject hitChar = hit.gameObject;
            float distance = Vector3.Distance(transform.position, hitChar.transform.position);

            if (distance < closestDistance)
            {
                closestCharacter = hitChar.GetComponent<GateBehaviorScript>();
            }
        }

        return closestCharacter;
    }

    public void OnInteract(InputValue value)
    {
        Debug.Log("On Interact triggered");

        if (!value.isPressed)
        {
            // Debug.Log("value was not pressed");
            return;
        }

        if ((currentChar == null && currentItem == null) || isInteracting)
        {
            // Debug.Log("current item is null or isInteracting");
            return;
        }

        
        

        if(charInteraction)
        {
            Debug.Log("char interaction routine");
            charInteraction = false;
            StartCoroutine(CharInteractRoutine());
        } else
        {
            StartCoroutine(ItemInteractRoutine());
        }

        

        Debug.Log("interact triggered with item");
        

        
        // messageText = "" TODO: implement random wants message

    }

    public void OnGive(InputValue value)
    {
        Debug.Log("Give triggered");
        if (!value.isPressed)
        {
            // Debug.Log("value was not pressed");
            return;
        }

        if (currentChar == null)
        {
            return;
        }

        if (currentChar.interactionMode != InteractionMode.Waiting)
        {
            return;
        }
        
        List<ItemData> itemList = new List<ItemData>();

        

        Dictionary<ItemData, int> myDict = inventory.GetItemsInInventory();
        
        foreach (ItemData key in myDict.Keys)
        {
            for (int j = 0; j < myDict[key]; j++)
            {
                itemList.Add(key);
            }
        }

        ItemData[] toSend = new ItemData[itemList.Count];
        for(int i = 0; i < itemList.Count; i++)
        {
            toSend[i] = itemList[i];
            Debug.Log("item added: " + toSend[i]);
        }

        foreach(ItemData item in toSend)
        {
            Debug.Log("item in toSend: " + item);
        }

        Debug.Log("toSend: " + toSend);
        bool isGiven = currentChar.GiveGift(toSend, promptText);
        if (isGiven)
        {
            currentCharMessage = currentChar.currText;
            Debug.Log("items wanted: " + currentChar.itemsWanted.Count);
            for (int i = 0; i < currentChar.itemsWanted.Count; i++)
            {
                for (int j = 0; j < currentChar.itemAmountsWanted[i]; j++)
                {
                    inventory.RemoveItem(currentChar.itemsWanted[i]);
                }
                
            }
            inventory.AddItem(currentChar.characterSpot, 1);
        }
    }

    private IEnumerator CharInteractRoutine()
    {
        talking = true;
        isCharInteracting = true;
        if (promptText != null)
        {
            Debug.Log("prompt text is not null");
            // promptText.gameObject.SetActive(false);
        }
        currentItem = null;
        currentCharMessage = currentChar.currText;
        currentChar.Interact(promptText);
        currentChar = null;
        

        yield return null;
    }

    private IEnumerator ItemInteractRoutine()
    {
        Debug.Log("Interact routine started");
        isInteracting = true;

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
            Debug.Log("item interact routine set active false");
        }

    
        currentChar = null;
        Debug.Log("currentItem = " + currentItem.name);
        currentItem.Interact(inventory);
        currentItem = null;
        
        

        yield return new WaitForSeconds(0.3f);

        isInteracting = false;
        
    }
}
