using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    [SerializeField] private Image healthUI;
    [SerializeField] private Image cleanUI;
    private MoneyManager moneyManager;

    private bool isOccupied = false;
    private float speed = 2f;

    [SerializeField] private float currentHealth = 10f;
    private float maxHealth = 10f;
    private float targetHealth = 10f;

    [SerializeField] private float currentCleanliness = 10f;
    private float maxCleanliness = 10f;
    private float targetCleanliness = 10f;

    public void ResetMachine()
    {
        isOccupied = false;
        currentHealth = maxHealth;
        targetHealth = maxHealth;
        healthUI.fillAmount = maxHealth;
        currentCleanliness = maxCleanliness;
        targetCleanliness = maxCleanliness;
        cleanUI.fillAmount = maxCleanliness;
}
    public bool IsAvailable()
    {
        return !isOccupied && currentHealth > 0 && currentCleanliness > 0;
    }
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

    public void UpdateHealth()
    {
        targetHealth = currentHealth / maxHealth;
    }
    public void DamageMachine(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealth();
    }
    public void RepairMachine(float repairAmount, int repairCost)
    {
        // Only repair if machine has damage and player has enough money
        if (moneyManager.HasEnoughMoney(repairCost) && currentHealth < maxHealth)
        {
            moneyManager.RemoveMoney(repairCost);
            currentHealth += repairAmount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            UpdateHealth();
        }
    }

    public void UpdateCleanliness()
    {
        targetCleanliness = currentCleanliness / maxCleanliness;
    }
    public void DirtyMachine(float dirty)
    {
        currentCleanliness -= dirty;
        currentCleanliness = Mathf.Max(currentCleanliness, 0);
        UpdateCleanliness();
    }
    public void CleanMachine(float clean, int cleanCost)
    {
        if (moneyManager.HasEnoughMoney(cleanCost) && currentCleanliness < maxCleanliness)
        {
            moneyManager.RemoveMoney(cleanCost);
            currentCleanliness += clean;
            currentCleanliness = Mathf.Min(currentCleanliness, maxCleanliness);
            UpdateCleanliness();
        }   
    }
    private void Start()
    {
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
    }
    private void Update()
    {
        healthUI.fillAmount = Mathf.MoveTowards(healthUI.fillAmount, targetHealth, speed * Time.deltaTime);
        cleanUI.fillAmount = Mathf.MoveTowards(cleanUI.fillAmount, targetCleanliness, speed * Time.deltaTime);
    }
}
