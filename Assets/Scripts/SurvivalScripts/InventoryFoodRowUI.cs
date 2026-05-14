using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryFoodRowUI : MonoBehaviour
{
    [SerializeField] TMP_Text foodNameText;
    [SerializeField] TMP_Text quantityText;
    [SerializeField] Button   eatButton;

    FoodDefinition    food;
    InventoryFoodUI   menu;

    public void Setup(FoodDefinition newFood, int quantity, InventoryFoodUI newMenu)
    {
        food = newFood;
        menu = newMenu;

        foodNameText.text = food.displayName;
        quantityText.text = "x" + quantity;

        eatButton.onClick.RemoveAllListeners();
        eatButton.onClick.AddListener(() => menu.EatFood(food));
    }
}