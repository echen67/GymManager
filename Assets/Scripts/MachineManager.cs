using UnityEngine;
using System.Collections.Generic;

public class MachineManager : MonoBehaviour
{
    [SerializeField] private List<Machine> machines;

    public Machine FindFreeMachine()
    {
        foreach(Machine machine in machines)
        {
            if (machine.IsAvailable())
            {
                return machine;
            }
        }
        return null;
    }

    public void ResetMachines()
    {
        foreach(Machine machine in machines)
        {
            machine.ResetMachine();
        }
    }
}
