using UnityEngine;

[CreateAssetMenu(menuName = "Survival/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public enum ResultKind
    {
        InventoryItem,
        EquipTool,
        PlaceObject     
    }

    public string displayName;

    public CraftingIngredient[] ingredients;

    public ResultKind resultKind;

    [Header("Inventory Item Result")]
    public ItemType resultItem;
    public int resultAmount = 1;

    [Header("Tool Result")]
    public GameObject toolPrefab;

    [Header("Placeable Result")]
    [Tooltip("The prefab that gets placed in the world. Needs a PlaceableObject component.")]
    public GameObject placementPrefab;
}