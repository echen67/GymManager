using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private AudioSource audioSource;

    private int currentMoney;

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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
