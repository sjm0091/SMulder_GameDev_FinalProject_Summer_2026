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

    public void PerformGateBehavior()
    {
        switch (gateType)
        {
            case GateType.NAND:
                CallNAND(input1, input2);
                break;
            case GateType.NOT:
                CallNOT(input1);
                break;
            case GateType.NOR:
                CallNOR(input1, input2);
                break;
            case GateType.OR:
                CallOR(input1, input2);
                break;
            case GateType.AND:
                CallAND(input1, input2);
                break;
            case GateType.XOR:
                CallXOR(input1, input2);
                break;
            case GateType.XNOR:
                CallXNOR(input1, input2);
                break;

        }
    }

    public bool CallAND(bool firstInput, bool secondInput)
    {
        return firstInput && secondInput;
    }

    public bool CallNAND(bool firstInput, bool secondInput)
    {
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
