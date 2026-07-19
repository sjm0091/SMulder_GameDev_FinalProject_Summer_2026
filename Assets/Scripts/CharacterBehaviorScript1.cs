using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public enum InteractionMode
{
    Querying = 0,
    Waiting = 1,
    Happy = 2
}
public class CharacterBehaviorScript1 : MonoBehaviour
{
    // public TextMeshProUGUI messageText;
    public string queryingText;
    public string waitingText;
    public string happyText;
    public string promptText = "Talk [E]";
    // public ItemData characterSpot;
    public InteractableObject characterSpot;

    public List<ItemData> itemsWanted = new List<ItemData>(); 
    public List<int> itemAmountsWanted = new List<int>(); // corresponds to itemsWanted
    public InteractionMode interactionMode;
    public string currText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionMode = InteractionMode.Querying;
        if (itemsWanted.Count != itemAmountsWanted.Count)
        {
            Debug.LogError("lengths of items and item amounts must match for playability");
        }

        currText = queryingText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ItemData ChangeMode()
    {
        if (interactionMode == InteractionMode.Querying)
        {
            interactionMode = InteractionMode.Waiting;
            currText = waitingText;
            return null;
        }
        if (interactionMode == InteractionMode.Waiting)
        {
            interactionMode = InteractionMode.Happy;
            currText = happyText;
            return characterSpot.itemData;
        }
        return null;
    }

    public void Interact(TextMeshProUGUI messageText)
    {
        Debug.Log("char interact called");
        messageText.gameObject.SetActive(true);
        switch(interactionMode)
        {
            case InteractionMode.Querying:
                Debug.Log("case: Querying");
                messageText.text = queryingText;
                ChangeMode();
                break;
            case InteractionMode.Waiting:
                Debug.Log("case: Waiting");
                messageText.text = waitingText;
                break;
            case InteractionMode.Happy:
                Debug.Log("case: Happy");
                messageText.text = happyText;
                break;
            default:
                break;
        }

        Debug.Log("messageText: " + messageText.gameObject.name);
        messageText.gameObject.SetActive(true);
    }

    public bool GiveGift(ItemData[] items, TextMeshProUGUI messageText)
    {
        Debug.Log("Give Gift triggered");
        Debug.Log("itemsWanted: " + itemsWanted);
        foreach(ItemData item in itemsWanted)
        {
            Debug.Log("item: " + item.name);
        }
        Debug.Log("itemswantedCounts: " + itemAmountsWanted);
        Debug.Log("itemAmountsWanted count: " + itemAmountsWanted.Count);
        foreach(int count in itemAmountsWanted)
        {
            Debug.Log("count: " + count);
        }
        bool enough = true;
        for (int i = 0; i < itemsWanted.Count; i++)
        {
            Debug.Log("item: "+itemsWanted[i]);
            Debug.Log("amountWanted: "+itemAmountsWanted[i]);
            if (!HasNumberItems(items, itemsWanted[i], itemAmountsWanted[i]))
            {
                Debug.Log("does not have enough of item: " + itemsWanted[i]);
                enough = false;
            }
        }

        if (enough)
        {
            ChangeMode();
            messageText.text = happyText;
        }

        return enough;
    }

    private bool HasNumberItems(ItemData[] itemList, ItemData item, int amount)
    {
        int count = 0;
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] == item)
            {
                count++;
            }
        }
        Debug.Log("actual count: "+count);

        return count == amount;
    }
}
