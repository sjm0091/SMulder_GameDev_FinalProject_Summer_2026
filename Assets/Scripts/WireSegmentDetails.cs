using UnityEngine;

public enum WirePorts
{
    None = 0,
    Front = 1 << 0,
    Right = 1 << 1,
    Back = 1 << 2,
    Left = 1 << 3
}
public class WireSegmentDetails : MonoBehaviour
{
    public WirePorts wirePorts;
    public WirePorts openPort;
    public WirePorts closedPort;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasPorts(WirePorts port)
    {
        return (wirePorts & port) != 0;
    }

    public bool HasOpenPort(WirePorts port)
    {
        return (openPort & port) != 0;
    }

    public bool HasClosedPort(WirePorts port)
    {
        return (closedPort & port) != 0;
    }
}
