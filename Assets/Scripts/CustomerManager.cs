using UnityEngine;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private MachineManager machineManager;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject customerPrefab;

    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private List<Customer> customerQueue;
    [SerializeField] private List<Customer> remainingCustomers;
    private int maxCustomersInQueue = 5;

    public void ResetCustomers()
    {
        foreach(Customer customer in customerQueue)
        {
            Destroy(customer.gameObject);
        }
        foreach(Customer customer in remainingCustomers)
        {
            Destroy(customer.gameObject);
        }
        customerQueue = new List<Customer>();
        remainingCustomers = new List<Customer>();
    }

    public void RemoveCustomer(GameObject customer)
    {
        remainingCustomers.Remove(customer.GetComponent<Customer>());
    }

    void Start()
    {
        InvokeRepeating("SpawnCustomer", 0f, timeBetweenSpawn);
    }

    void Update()
    {
        // Try to find unoccupied machine for first customer in the queue
        if (customerQueue.Count > 0)
        {
            Machine freeMachine = machineManager.FindFreeMachine();
            if (freeMachine)
            {
                // Remove first customer from queue and have them walk to machine
                freeMachine.OccupyMachine();
                Customer firstCustomer = customerQueue[0].GetComponent<Customer>();
                firstCustomer.CustomerWalkToMachine(freeMachine);
                customerQueue.Remove(firstCustomer);
                remainingCustomers.Add(firstCustomer);

                // Shift remaining customers up
                for(int i = 0; i < customerQueue.Count; i++)
                {
                    Vector3 newPos = new Vector3(spawnLocation.position.x + i, spawnLocation.position.y, spawnLocation.position.z);
                    customerQueue[i].WalkToLocation(newPos);
                }
            }
        }
    }

    void SpawnCustomer()
    {
        if (customerQueue.Count >= maxCustomersInQueue) return;

        Vector3 pos = new Vector3(spawnLocation.position.x + customerQueue.Count, spawnLocation.position.y, spawnLocation.position.z);
        GameObject spawnedCustomer = Instantiate(customerPrefab, pos, spawnLocation.rotation);
        customerQueue.Add(spawnedCustomer.GetComponent<Customer>());
    }
}
