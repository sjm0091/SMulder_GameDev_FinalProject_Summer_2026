using UnityEngine;

public class NOTCharacter : MonoBehaviour
{
    // NAND Gate
    public bool input;
    public bool output;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool NOT(bool newInput)
    {
        return !newInput;
    }
}
