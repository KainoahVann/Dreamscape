using TMPro;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text amountText;

    public void Set(ItemType itemType, int amount)
    {
        if (itemNameText != null)
            itemNameText.text = itemType.ToString();

        if (amountText != null)
            amountText.text = "x" + amount;
    }
}
