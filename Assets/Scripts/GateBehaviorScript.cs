using TMPro;
using UnityEngine;
// separate scripts reworked into one for reusability
public enum GateType
{
    NAND = 0,
    NOT = 1,
    NOR = 2,
    OR = 3,
    AND = 4,
    XOR = 6,
    XNOR = 7
}
public class GateBehaviorScript : MonoBehaviour
{
    public bool input1;
    public bool input2;
    public bool output;
    public GateType gateType = GateType.NAND;
    public TextMeshProUGUI message;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool PerformGateBehavior()
    {
        bool newOutput = false;
        switch (gateType)
        {
            case GateType.NAND:
                newOutput = CallNAND(input1, input2);
                break;
            case GateType.NOT:
                newOutput = CallNOT(input1);
                break;
            case GateType.NOR:
                newOutput = CallNOR(input1, input2);
                break;
            case GateType.OR:
                newOutput = CallOR(input1, input2);
                break;
            case GateType.AND:
                newOutput = CallAND(input1, input2);
                break;
            case GateType.XOR:
                newOutput = CallXOR(input1, input2);
                break;
            case GateType.XNOR:
                newOutput = CallXNOR(input1, input2);
                break;

        }
        return newOutput;
    }

    public bool CallAND(bool firstInput, bool secondInput)
    {
        return firstInput && secondInput;
    }

    public bool CallNAND(bool firstInput, bool secondInput)
    {
        Debug.Log("first input: " + firstInput);
        Debug.Log("second input: " + secondInput);
        Debug.Log("NAND: " + !(firstInput && secondInput));
        return !(firstInput && secondInput);
    }

    public bool CallNOR(bool firstInput, bool secondInput)
    {
        return !(firstInput || secondInput);
    }

    public bool CallNOT(bool newInput)
    {
        return !newInput;
    }

    public bool CallOR(bool firstInput, bool secondInput)
    {
        return firstInput || secondInput;
    }

    public bool CallXNOR(bool firstInput, bool secondInput)
    {
        return !((firstInput || secondInput) && !(firstInput && secondInput));
    }

    public bool CallXOR(bool firstInput, bool secondInput)
    {
        // optimize
        return (firstInput || secondInput) && !(firstInput && secondInput);
    }
}
