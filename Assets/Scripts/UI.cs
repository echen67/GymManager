using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    [SerializeField] private MoneyManager moneyManager;

    private UIDocument uiDocument;
    private Label moneyLabel;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        moneyLabel = uiDocument.rootVisualElement.Q<Label>("MoneyLabel");
    }

    // Update is called once per frame
    void Update()
    {
        int money = moneyManager.GetMoney();
        moneyLabel.text = money.ToString();
    }
}
