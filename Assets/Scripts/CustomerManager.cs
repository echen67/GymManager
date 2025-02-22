using UnityEngine;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    private List<GameObject> customerQueue;

    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject customerPrefab;

    void Start()
    {
        InvokeRepeating("SpawnCustomer", 2.0f, 10f);
    }

    void SpawnCustomer()
    {
        Instantiate(customerPrefab, spawnLocation.position, spawnLocation.rotation);
    }
}
