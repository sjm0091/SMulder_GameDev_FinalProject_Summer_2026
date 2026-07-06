using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public string resource = "flowers";
    public string wants = "flowers";
    public int amountPerCollet = 1;
    private int usesRemaining = 1;
    public TextMeshProUGUI promptText;
    private bool destroyOnCollect = true;
    public bool isResource = true;
    public ResourceCounter resourceCounter;
    public Transform player;
    public float maxDistance = 3f;
    public int numWants = 3;
    public int currentlyHave = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // resource
        resourceCounter = FindFirstObjectByType<ResourceCounter>();
        PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();
        player = playerMovement.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < maxDistance)
        {
            promptText.gameObject.SetActive(true);
            // Debug.Log("in distance");
        }
        else
        {
            return;
        }
        
        // else
        // {
        //     promptText.gameObject.SetActive(false);
        //     Debug.Log("not in distance");
        // }
        if (Keyboard.current.vKey.isPressed && isResource)
        {
            OnInteract();
        }

        if (Keyboard.current.vKey.isPressed && !isResource)
        {
            PlayerInteractionScript playerScript = player.GetComponent<PlayerInteractionScript>();
            playerScript.OnInteract();
        }

        if (Keyboard.current.gKey.isPressed && !isResource && resourceCounter.numFlowers > 0)
        {
            resourceCounter.RemoveResource(resource);
            currentlyHave++;
        }
        if (currentlyHave >= numWants)
        {
            promptText.text = "I have "+ numWants +" " +wants + "! Now I will do as you ask";
        }

    }

    public void OnInteract() //TODO change to input system
    {
        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log("interact triggered");
        

        if (usesRemaining <= 0)
        {
            return;
        }
        usesRemaining--;
        
        resourceCounter.AddResource(resource);
        
        if (destroyOnCollect && usesRemaining <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
