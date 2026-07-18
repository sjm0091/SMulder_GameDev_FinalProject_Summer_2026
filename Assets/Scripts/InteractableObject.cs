using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public string resource = "flower";
    public ItemData itemData;
    public int amountPerCollet = 1;
    private int usesRemaining = 1;
    private bool destroyOnCollect = true;
    public ResourceCounter resourceCounter;
    public Transform player;
    public float maxDistance = 3f;
    public int numWants = 3;
    public int currentlyHave = 0;
    public Sprite itemIcon;
    public int maxStackAmount = 5;
    public Inventory inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // resource
        resourceCounter = FindFirstObjectByType<ResourceCounter>();
        // PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();
        // player = playerMovement.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {

        // if (Keyboard.current.vKey.isPressed && !isResource)
        // {
        //     PlayerInteractionScript playerScript = player.GetComponent<PlayerInteractionScript>();
        //     playerScript.OnInteract();
        // }

        // if (currentlyHave >= numWants)
        // {
        //     promptText.text = "I have "+ numWants +" " +wants + "! Now I will do as you ask";
        // }

    }

    public void Interact(Inventory inventory) //TODO change to input system
    {
        // float distance = Vector3.Distance(transform.position, .position);
        Debug.Log("interact triggered");
        

        if (usesRemaining <= 0)
        {
            return;
        }
        usesRemaining--;
        
        // resourceCounter.AddResource(resource);
        inventory.AddItem(this, amountPerCollet);
        
        if (destroyOnCollect && usesRemaining <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
