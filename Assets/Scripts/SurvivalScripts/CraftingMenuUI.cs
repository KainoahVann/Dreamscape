using UnityEngine;

public class CraftingMenuUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SurvivalInventory inventory;
    [SerializeField] PlayerToolManager toolManager;
    [SerializeField] PlacementManager  placementManager;
    [SerializeField] GameObject        craftingPanel;
    [SerializeField] Transform         recipeParent;
    [SerializeField] GameObject        recipeButtonPrefab;

    [Header("Recipes")]
    [SerializeField] CraftingRecipe[] recipes;

    [Header("Input")]
    [SerializeField] KeyCode toggleKey = KeyCode.C;

    CraftingRecipeButtonUI[] recipeButtons;

    void Start()
    {
        BuildRecipeButtons();

        if (craftingPanel != null)
            craftingPanel.SetActive(false);

        RefreshButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && !PlacementManager.IsPlacing)
            SetOpen(!craftingPanel.activeSelf);
    }

    void SetOpen(bool open)
    {
        bool wasOpen = craftingPanel.activeSelf;
        craftingPanel.SetActive(open);

        if (open && !wasOpen)   MenuCursor.OnOpen();
        if (!open && wasOpen)   MenuCursor.OnClose();

        if (open) RefreshButtons();
    }

    void BuildRecipeButtons()
    {
        foreach (Transform child in recipeParent)
            Destroy(child.gameObject);

        recipeButtons = new CraftingRecipeButtonUI[recipes.Length];

        for (int i = 0; i < recipes.Length; i++)
        {
            GameObject buttonObject = Instantiate(recipeButtonPrefab, recipeParent);
            CraftingRecipeButtonUI buttonUI = buttonObject.GetComponent<CraftingRecipeButtonUI>();
            buttonUI.Setup(recipes[i], this);
            recipeButtons[i] = buttonUI;
        }
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (CraftingIngredient ingredient in recipe.ingredients)
        {
            if (!inventory.HasItem(ingredient.itemType, ingredient.amount))
                return false;
        }
        return true;
    }

    public void Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe)) return;

        foreach (CraftingIngredient ingredient in recipe.ingredients)
            inventory.RemoveItem(ingredient.itemType, ingredient.amount);

        switch (recipe.resultKind)
        {
            case CraftingRecipe.ResultKind.InventoryItem:
                inventory.AddItem(recipe.resultItem, recipe.resultAmount);
                break;

            case CraftingRecipe.ResultKind.EquipTool:
                toolManager.EquipTool(recipe.toolPrefab);
                break;

            case CraftingRecipe.ResultKind.PlaceObject:

                SetOpen(false);
                placementManager.BeginPlacement(recipe.placementPrefab);
                break;
        }

        RefreshButtons();
    }

    void RefreshButtons()
    {
        if (recipeButtons == null) return;

        for (int i = 0; i < recipes.Length; i++)
            recipeButtons[i].SetCanCraft(CanCraft(recipes[i]));
    }
}