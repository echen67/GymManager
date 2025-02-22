using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private DayManager dayManager;

    private UIDocument uiDocument;
    private Label moneyLabel;
    private ProgressBar dayProgressBar;
    private VisualElement dayCompletePanel;
    private Button nextDayButton;
    private VisualElement gameOverPanel;
    private Button restartButton;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        moneyLabel = uiDocument.rootVisualElement.Q<Label>("MoneyLabel");
        dayProgressBar = uiDocument.rootVisualElement.Q<ProgressBar>("DayProgressBar");
        dayCompletePanel = uiDocument.rootVisualElement.Q<VisualElement>("DayEndContainer");
        gameOverPanel = uiDocument.rootVisualElement.Q<VisualElement>("GameOverContainer");
        nextDayButton = uiDocument.rootVisualElement.Q<Button>("NextDayButton");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");

        nextDayButton.clicked += ClickedNextDayButton;
        restartButton.clicked += ClickedRestartButton;
    }

    void Update()
    {
        int money = moneyManager.GetMoney();
        moneyLabel.text = money.ToString();

        dayProgressBar.value = dayManager.GetTime();
    }

    public void ShowDayCompletePanel()
    {
        dayCompletePanel.style.display = DisplayStyle.Flex;
    }
    private void ClickedNextDayButton()
    {
        dayCompletePanel.style.display = DisplayStyle.None;
        Time.timeScale = 1;
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.style.display = DisplayStyle.Flex;
    }
    private void ClickedRestartButton()
    {
        gameOverPanel.style.display = DisplayStyle.None;
        Time.timeScale = 1;
    }
}
