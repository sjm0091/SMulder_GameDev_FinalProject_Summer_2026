using System.Collections;
using JetBrains.Annotations;
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
    public InteractableObject currentItem;
    public Inventory inventory;
    private bool isInteracting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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


    }

    public void FindNearbyItem()
    {
        // Debug.Log("find nearest item triggered ");
        Collider[] hits = Physics.OverlapSphere(transform.position, minDistance);
        // Debug.Log(hits);
        // Debug.Log(hits.Length);
        InteractableObject closestItem = null;
        float closestDistance = Mathf.Infinity;

        foreach(Collider hit in hits)
        {
            // Debug.Log("Hit: " + hit.gameObject.name);
            InteractableObject item = hit.gameObject.GetComponent<InteractableObject>();
            if (item == null)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance < closestDistance)
            {
                closestItem = item;
                closestDistance = distance;
            }
        }

        currentItem = closestItem;

        if (promptText == null)
        {
            return;
        }

        if (currentItem != null && !isInteracting)
        {
            promptText.text = "Press V To Interact";
            promptText.gameObject.SetActive(true);
        } else
        {
            promptText.gameObject.SetActive(false);
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

        if (currentItem == null || isInteracting)
        {
            // Debug.Log("current item is null or isInteracting");
            return;
        }

        StartCoroutine(InteractRoutine());

        Debug.Log("interact triggered with item");
        

        
        // messageText = "" TODO: implement random wants message

    }

    private IEnumerator InteractRoutine()
    {
        Debug.Log("Interact routine started");
        isInteracting = true;

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }

        if (currentItem != null)
        {
            currentItem.Interact(inventory);
        }

        yield return new WaitForSeconds(0.3f);

        isInteracting = false;
    }
}
