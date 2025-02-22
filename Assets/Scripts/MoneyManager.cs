using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private AudioSource audioSource;

    private int currentMoney;
    private int rentCost = 1;

    public int GetMoney()
    {
        return currentMoney;
    }
    public void AddMoney(int money)
    {
        currentMoney += money;
        audioSource.Play();
    }
    public void RemoveMoney(int money)
    {
        currentMoney -= money;
    }
    public void ResetMoney()
    {
        currentMoney = 0;
    }

    // Returns whether you were able to pay rent or not
    public bool PayRent()
    {
        if (currentMoney >= rentCost)
        {
            RemoveMoney(rentCost);
            return true;
        }
        return false;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
