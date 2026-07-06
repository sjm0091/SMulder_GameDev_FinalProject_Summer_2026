using UnityEngine;

public class XORCharacter : MonoBehaviour
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

    public bool XOR(bool firstInput, bool secondInput)
    {
        // optimize
        return (firstInput || secondInput) && !(firstInput && secondInput);
    }
}
