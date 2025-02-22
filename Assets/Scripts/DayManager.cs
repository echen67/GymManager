using UnityEngine;

public class DayManager : MonoBehaviour
{
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private MachineManager machineManager;
    [SerializeField] private CustomerManager customerManager;
    [SerializeField] private UI uiScript;

    [SerializeField] private float timeSpeed = 1f;
    private float currentTime = 0f;
    private float maxTime = 100f;

    public float GetTime()
    {
        return currentTime;
    }

    void Update()
    {
        currentTime += timeSpeed * Time.deltaTime;

        // End the day
        if (currentTime >= maxTime)
        {
            bool paidRent = moneyManager.PayRent();
            if (paidRent)
            {
                uiScript.ShowDayCompletePanel();
            } else
            {
                uiScript.ShowGameOverPanel();
                moneyManager.ResetMoney();
            }

            // in both cases, pause the game, reset machines and customers, and reset time
            currentTime = 0;
            Time.timeScale = 0;
            customerManager.ResetCustomers();
            machineManager.ResetMachines();
        }
    }
}
