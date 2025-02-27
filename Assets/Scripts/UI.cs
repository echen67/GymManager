using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private DayManager dayManager;
    [SerializeField] private GameObject buildingSystemParent;
    [SerializeField] private PlacementSystem placementSystem;

    // General UI
    private UIDocument uiDocument;
    private Label moneyLabel;
    private ProgressBar dayProgressBar;
    private VisualElement dayCompletePanel;
    private Button dayCompleteOKButton;
    private Button beginNextDayButton;
    private VisualElement gameOverPanel;
    private Button restartButton;
    private VisualElement rating;

    // Shop UI
    private Button shopButton;
    private VisualElement shopPanel;
    private Label itemDetailTitle;
    private VisualElement itemDetailPortrait;
    private Label itemDetailCost;
    private Button shopCloseButton;
    private Button purchaseButton;
    private VisualElement equipmentItems;

    private int currentSelectedMachineID = -1;
    private int currentSelectedMachineCost = 0;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        moneyLabel = uiDocument.rootVisualElement.Q<Label>("MoneyLabel");
        dayProgressBar = uiDocument.rootVisualElement.Q<ProgressBar>("DayProgressBar");
        dayCompletePanel = uiDocument.rootVisualElement.Q<VisualElement>("DayEndContainer");
        gameOverPanel = uiDocument.rootVisualElement.Q<VisualElement>("GameOverContainer");
        dayCompleteOKButton = uiDocument.rootVisualElement.Q<Button>("DayCompleteOK");
        beginNextDayButton = uiDocument.rootVisualElement.Q<Button>("BeginNextDayButton");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        rating = uiDocument.rootVisualElement.Q<VisualElement>("RatingFill");

        dayCompleteOKButton.clicked += ClickedDayCompleteOK;
        beginNextDayButton.clicked += ClickedBeginNextDayButton;
        restartButton.clicked += ClickedRestartButton;

        shopButton = uiDocument.rootVisualElement.Q<Button>("ShopButton");
        shopPanel = uiDocument.rootVisualElement.Q<VisualElement>("ShopContainer");
        itemDetailTitle = uiDocument.rootVisualElement.Q<Label>("ItemDetailTitle");
        itemDetailPortrait = uiDocument.rootVisualElement.Q<VisualElement>("ItemDetailPortrait");
        itemDetailCost = uiDocument.rootVisualElement.Q<Label>("ItemDetailCost");
        shopCloseButton = uiDocument.rootVisualElement.Q<Button>("CloseShopButton");
        purchaseButton = uiDocument.rootVisualElement.Q<Button>("PurchaseButton");
        equipmentItems = uiDocument.rootVisualElement.Q<VisualElement>("EquipmentItems");

        shopButton.clicked += ClickedShopButton;
        purchaseButton.clicked += ClickedPurchaseButton;
        shopCloseButton.clicked += ClickedCloseShop;

        // Populate shop with shop items
        foreach (ObjectData objectData in database.objectsData)
        {
            equipmentItems.Add(newShopItem(objectData.Name, objectData.ID, objectData.Image, objectData.Cost));
        }
    }

    VisualElement newShopItem(string name, int ID, Texture2D image, int cost)
    {
        Color bgColor = new Color(0.2470588f, 0.4823529f, 0.8196079f, 1);
        Color highlightColor = new Color(0.1584406f, 0.3833255f, 0.7056604f, 1);

        VisualElement shopItem = new VisualElement();
        shopItem.style.backgroundColor = bgColor;
        shopItem.style.marginBottom = 16;
        shopItem.style.marginRight = 16;
        shopItem.style.width = 500;
        shopItem.style.height = 500;
        shopItem.style.borderBottomLeftRadius = 16;
        shopItem.style.borderBottomRightRadius = 16;
        shopItem.style.borderTopLeftRadius = 16;
        shopItem.style.borderTopRightRadius = 16;
        shopItem.style.alignItems = Align.Center;

        Label itemTitle = new Label(name);
        itemTitle.style.fontSize = 48;

        VisualElement itemImage = new VisualElement();
        itemImage.style.height = 300;
        itemImage.style.width = 300;
        itemImage.style.backgroundImage = image;
        itemImage.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;

        Label itemCost = new Label("Cost: " + cost.ToString());
        itemCost.style.fontSize = 32;
        
        shopItem.Add(itemTitle);
        shopItem.Add(itemImage);
        shopItem.Add(itemCost);
        
        shopItem.RegisterCallback<ClickEvent>(Clicked); // asset is the root visual element that will be closed
        void Clicked(ClickEvent evt)
        {
            Debug.Log($"Clicked {name}");
            // TODO: Populate detail panel with the rest of this item's details
            itemDetailTitle.text = name;
            itemDetailCost.text = "Cost: " + cost.ToString();
            itemDetailPortrait.style.backgroundImage = image;

            currentSelectedMachineID = ID;
            currentSelectedMachineCost = cost;
        }
        
        shopItem.RegisterCallback<MouseEnterEvent>(MouseEnter);
        void MouseEnter(MouseEnterEvent evt)
        {
            //shopItem.style.scale = new StyleScale(new Scale(new Vector2(1.1f, 1.1f)));
            shopItem.style.backgroundColor = highlightColor;
        }

        shopItem.RegisterCallback<MouseLeaveEvent>(MouseLeave);
        void MouseLeave(MouseLeaveEvent evt)
        {
            shopItem.style.backgroundColor = bgColor;
        }

        return shopItem;
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
    private void ClickedDayCompleteOK()
    {
        dayCompletePanel.style.display = DisplayStyle.None;
        beginNextDayButton.style.display = DisplayStyle.Flex;
        shopButton.style.display = DisplayStyle.Flex;
    }
    private void ClickedBeginNextDayButton()
    {
        beginNextDayButton.style.display = DisplayStyle.None;
        shopButton.style.display = DisplayStyle.None;
        //Time.timeScale = 1;
        dayManager.StartDay();
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.style.display = DisplayStyle.Flex;
    }
    private void ClickedRestartButton()
    {
        gameOverPanel.style.display = DisplayStyle.None;
        dayManager.StartDay();
        //Time.timeScale = 1;
    }
    public void SetRating(float ratingVal)
    {
        rating.style.scale = new StyleScale(new Scale(new Vector2(ratingVal, 1f)));
    }

    // Shop Methods
    public void ClickedShopButton()
    {
        shopPanel.style.display = DisplayStyle.Flex;
    }
    public void ClickedPurchaseButton()
    {
        if (moneyManager.HasEnoughMoney(currentSelectedMachineCost))
        {
            buildingSystemParent.SetActive(true);
            ClickedCloseShop();
            placementSystem.StartPlacement(currentSelectedMachineID, currentSelectedMachineCost);
        }
        Debug.Log("Don't have enough money to purchase this item!");
        // TODO: handle not having enough money
    }
    public void ClickedCloseShop()
    {
        shopPanel.style.display = DisplayStyle.None;
    }
}