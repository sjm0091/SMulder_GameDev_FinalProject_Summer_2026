using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionScript : MonoBehaviour
{
    public GateBehaviorScript character;
    private float minDistance = 20f;
    public TextMeshProUGUI messageText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        character = FindNearestCharacter();
        if (character == null)
        {
            if (messageText != null)
            {
                messageText.gameObject.SetActive(false);
            }
            return;
        }
        Debug.Log("FOUND");
        messageText = character.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        messageText.gameObject.SetActive(true);


    }

    public GateBehaviorScript FindNearestCharacter()
    {
        Debug.Log("finding nearest char");
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
            GateBehaviorScript script = hit.GetComponentInParent<GateBehaviorScript>();
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

    public void OnInteract()
    {
        Debug.Log("interact triggered with character");
        if (messageText == null)
        {
            return;
        }
        
        // messageText = "" TODO: implement random wants message

    }
}
