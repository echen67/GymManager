using System;
using UnityEngine;
using Unity.AI.Navigation;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMesh;
    [SerializeField] private MachineManager machineManager;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private GameObject buildingSystemParent;

    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;
    private int selectedObjectCost = 0;
    [SerializeField] private GameObject gridVisualization;

    private void Start()
    {
        //StopPlacement();
        //StartPlacement(2);
    }

    public void StartPlacement(int ID, int cost)
    {
        //StopPlacement();
        Debug.Log("In StartPlacement");
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if(selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

        selectedObjectCost = cost;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);

        machineManager.AddMachine(newObject.GetComponentInChildren<Machine>());
        moneyManager.RemoveMoney(selectedObjectCost);
        navMesh.BuildNavMesh();
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        buildingSystemParent.SetActive(false);
    }

    private void Update()
    {
        if (selectedObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
