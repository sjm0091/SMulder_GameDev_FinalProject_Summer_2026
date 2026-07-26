using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
// using UnityEngine.UIElements;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public WirePlaceMode wireScript;

    public List<GameObject> totalCharPrefabs = new List<GameObject>();
    public List<GameObject> availableCharacters = new List<GameObject>(); // starts with NAND. Added by NightTimeGameManager
    public List<Transform> characterSpots = new List<Transform>(); // set in editor
    public List<GameObject> spawnedCharacters = new List<GameObject>();
    public List<int> filledSpots = new List<int>();

    // UI
    public GameObject wirePanel;
    public List<Button> createCharSpotButtons = new List<Button>();
    public Canvas mapCanvas;
    public Camera canvasCamera;

    public void Start()
    {
        foreach(Button button in createCharSpotButtons)
        {
            button.gameObject.SetActive(false);
        }

        if (createCharSpotButtons.Count != totalCharPrefabs.Count)
        {
            Debug.LogError("create char spot buttons and total char prefabs must have same length!!!!");
        }
    }
    
    public void SpawnAvailableCharacters()
    {
        foreach (GameObject character in availableCharacters)
        {
            bool found = false;
            while (!found)
            {
                int index = Random.Range(0, characterSpots.Count - 1);
                if (!filledSpots.Contains(index))
                {
                    found = true;
                    GameObject newChar = Instantiate(character, characterSpots[index]);
                    newChar.GetComponentInChildren<CharacterBehaviorScript1>().mapCanvas = mapCanvas;
                    newChar.GetComponentInChildren<CharacterBehaviorScript1>().canvasCamera = canvasCamera;
                    newChar.GetComponentInChildren<CharacterBehaviorScript1>().SetMapIcon();
                    spawnedCharacters.Add(newChar);
                    newChar.transform.position = new Vector3(newChar.transform.position.x, newChar.transform.position.y + 2f, newChar.transform.position.z);

                    filledSpots.Add(index);
                }
            }
        }
    }

    public void AddCharSpotButton(string characterGateName) // called by wire
    {
        Debug.Log("Add char spot button triggered");
        int index = -1;
        switch(characterGateName)
        {
            case "NOR":
            Debug.Log("NOR added");
                index = 0;
                break;
            case "AND":
            Debug.Log("AND added");
                index = 1;
                break;
            case "XNOR":
            Debug.Log("XNOR added");
                index = 2;
                break;
            case "XOR":
            Debug.Log("XOR added");
                index = 3;
                break;
            case "OR":
            Debug.Log("OR addedd");
                index = 4;
                break;
            case "NAND":
            Debug.Log("NAND added");
                index = 5;
                break;
            case "NOT":
            Debug.Log("NOT added");
                index = 6;
                break;

        }
        Debug.Log("index: " + index);
        Button button = createCharSpotButtons[index];
        button.gameObject.SetActive(true);
    }

    public void AddCharacter(GameObject character)
    {
        availableCharacters.Add(character);
    }

    public bool UnlockedCharacter(GameObject character)
    {
        return availableCharacters.Contains(character);
    }

    public void ClearAfterDay()
    {
        List<GameObject> toDestroy = new List<GameObject>();
        foreach (GameObject character in spawnedCharacters)
        {
            toDestroy.Add(character);
            filledSpots = new List<int>();
        } 
        for (int i = 0; i < toDestroy.Count; i++)
        {
            spawnedCharacters.Remove(toDestroy[i]);
            Destroy(toDestroy[i]);
        }
        
    }
}
