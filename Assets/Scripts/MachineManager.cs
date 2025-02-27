using UnityEngine;
using System.Collections.Generic;

public class MachineManager : MonoBehaviour
{
    [SerializeField] private List<Machine> machines;

    public void AddMachine(Machine newMachine)
    {
        machines.Add(newMachine);
    }
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
