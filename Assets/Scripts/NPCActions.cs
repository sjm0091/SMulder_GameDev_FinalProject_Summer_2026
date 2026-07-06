using System.Collections;
using UnityEditor;
using UnityEngine;

public class NPCActions : MonoBehaviour
{
    public GameObject partner; // if a 2 person gate (most of them)
    public GateBehaviorScript nandScript;
    public Vector3 nightModePosition; // to be set by player
    public float maxDist = 3f;

    public bool giver;

    // Referenced by other scripts (other npcs)
    public bool receivedGift;
    public GameObject receiverNPC;

    // Referenced by GameManager
    public bool NightMode { get; set; }

    /** ---ACTIONS---
    Check if interaction => bool
    Attempt interaction => void
    Give gift => void
    Calculate if gift give  => bool
    */



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nandScript = this.gameObject.GetComponent<GateBehaviorScript>();
        giver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (NightMode)
        {
            transform.position = nightModePosition;
            StartCoroutine(NightModeActions());
        }
    }

    public void OnPlayerInteraction()
    {
        
    }

    /* Activated during the night. Will person their gate's tasks with any inputs */
    IEnumerator NightModeActions()
    {
        if (CheckIfInteraction())
        {
            AttemptInteraction();
        }
        yield return null;
    }

    public bool CheckIfInteraction()
    {
        // Check if npc in front of them within distance
        // Get hit information and assign self as reference to that npc so can trade
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDist))
        {
            //incomplete
            if (hit.collider.gameObject.CompareTag("Townsfolk_NPC"))
            {
                return true;
            }
        }
        return false;
    }

    public void AttemptInteraction()
    {
        if (giver)
        {
            bool giveGift = CalculateIfGiftGive();
            StartCoroutine(WaitForReceiverReferenceAssignment());
            GiveGift(receiverNPC, giveGift);
            return;
        }
        // Check if npc in front of them within distance
        // wait until reference is assigned
        // Give gift or Give nothing
       


    }

    IEnumerator WaitForReceiverReferenceAssignment()
    {
        if (receiverNPC != null)
        {
            yield return true;
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(WaitForReceiverReferenceAssignment());
            yield return true;
        }
    }

    public void GiveGift(GameObject otherNPC, bool giveGift)
    {
        // get reference to other NPC's script
        // set their recieve value to giveGift
        NPCActions otherScript = otherNPC.GetComponent<NPCActions>();
        otherScript.receivedGift = giveGift;
    }

    public bool CalculateIfGiftGive()
    {
        // call function from gate script to determine
        return false;
    }

}
