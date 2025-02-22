using UnityEngine;
using System.Collections.Generic;

public class MachineManager : MonoBehaviour
{
    [SerializeField] private List<Machine> machines;

    public Machine FindFreeMachine()
    {
        foreach(Machine machine in machines)
        {
            if (!machine.GetIsOccupied())
            {
                return machine;
            }
        }
        return null;
    }
}
