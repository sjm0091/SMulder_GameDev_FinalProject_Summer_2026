using UnityEngine;

public class ORCharacter : MonoBehaviour
{
    // NAND Gate
    public bool input1;
    public bool input2;
    public bool output;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool OR(bool firstInput, bool secondInput)
    {
        return firstInput || secondInput;
    }
}
