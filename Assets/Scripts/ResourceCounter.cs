using TMPro;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    public TextMeshProUGUI flowerText;
    public int numFlowers = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddResource(string resource)
    {
        Debug.Log("Add Resource");
        if (resource == "flowers")
        {
            numFlowers++;
            flowerText.text = "Flowers: " + numFlowers;
        }
        
    }

    public void RemoveResource(string resource)
    {
        Debug.Log("Remove Resource");
        if (resource == "flowers")
        {
            numFlowers--;
            flowerText.text = "Flowers: " + numFlowers;
        }
    }
}
