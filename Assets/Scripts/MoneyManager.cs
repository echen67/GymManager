using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private int currentMoney;

    public int GetMoney()
    {
        return currentMoney;
    }
    public void AddMoney(int money)
    {
        currentMoney += money;
    }
    public void RemoveMoney(int money)
    {
        currentMoney -= money;
    }
}
