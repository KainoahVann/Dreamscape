using UnityEngine;

public class InventoryFoodUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SurvivalInventory inventory;
    [SerializeField] PlayerHunger      playerHunger;
    [SerializeField] GameObject        inventoryPanel;
    [SerializeField] Transform         foodRowParent;
    [SerializeField] GameObject        foodRowPrefab;

    [Header("Food Definitions")]
    [SerializeField] FoodDefinition[] foodDefinitions;

    [Header("Input")]
    [SerializeField] KeyCode toggleKey = KeyCode.I;

    void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && !PlacementManager.IsPlacing)
            SetOpen(!inventoryPanel.activeSelf);
    }

    void SetOpen(bool open)
    {
        bool wasOpen = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(open);

        if (open && !wasOpen)  MenuCursor.OnOpen();
        if (!open && wasOpen)  MenuCursor.OnClose();

        if (open) Rebuild();
    }

    void Rebuild()
    {
        foreach (Transform child in foodRowParent)
            Destroy(child.gameObject);

        foreach (FoodDefinition food in foodDefinitions)
        {
            int count = inventory.GetCount(food.itemType);
            if (count <= 0) continue;

            GameObject row = Instantiate(foodRowPrefab, foodRowParent);
            InventoryFoodRowUI rowUI = row.GetComponent<InventoryFoodRowUI>();
            rowUI.Setup(food, count, this);
        }
    }

    public void EatFood(FoodDefinition food)
    {
        if (!inventory.HasItem(food.itemType, 1)) return;

        inventory.RemoveItem(food.itemType, 1);
        playerHunger.Feed(food.hungerRestore);
        Rebuild();
    }
}