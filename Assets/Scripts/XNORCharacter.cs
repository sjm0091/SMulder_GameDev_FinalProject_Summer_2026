using UnityEngine;

public class XNORCharacter : MonoBehaviour
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

    public bool XNOR(bool firstInput, bool secondInput)
    {
        return !((firstInput || secondInput) && !(firstInput && secondInput));
    }
}
