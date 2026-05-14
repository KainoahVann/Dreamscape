using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeButtonUI : MonoBehaviour
{
    [SerializeField] TMP_Text recipeNameText;
    [SerializeField] TMP_Text requirementsText;
    [SerializeField] Button button;

    CraftingRecipe recipe;
    CraftingMenuUI menu;

    public void Setup(CraftingRecipe newRecipe, CraftingMenuUI newMenu)
    {
        recipe = newRecipe;
        menu = newMenu;

        recipeNameText.text = recipe.displayName;

        string requirements = "";

        foreach (CraftingIngredient ingredient in recipe.ingredients)
        {
            requirements += ingredient.itemType + " x" + ingredient.amount + "\n";
        }

        requirementsText.text = requirements;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            menu.Craft(recipe);
        });
    }

    public void SetCanCraft(bool canCraft)
    {
        button.interactable = canCraft;
    }
}