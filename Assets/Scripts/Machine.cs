using UnityEngine;

public class Machine : MonoBehaviour
{
    private bool isOccupied = false;

    public bool GetIsOccupied()
    {
        return isOccupied;
    }
    public void OccupyMachine()
    {
        isOccupied = true;
    }
    public void FreeMachine()
    {
        isOccupied = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
