using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip gainMoneySFX;
    [SerializeField] AudioClip loseMoneySFX;

    private int currentMoney;
    private int rentCost = 1;

    public int GetMoney()
    {
        return currentMoney;
    }
    public bool HasEnoughMoney(int cost)
    {
        return currentMoney >= cost;
    }
    public void AddMoney(int money)
    {
        currentMoney += money;
        audioSource.clip = gainMoneySFX;
        audioSource.Play();
    }
    public void RemoveMoney(int money)
    {
        currentMoney -= money;
        audioSource.clip = loseMoneySFX;
        audioSource.Play();
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
