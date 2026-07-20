using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public Transform target;
    public Transform gateAreaPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        target.position = gateAreaPosition.position;
    }
}
