using System;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalInventory : MonoBehaviour
{
    Dictionary<ItemType, int> items = new Dictionary<ItemType, int>();

    public event Action OnInventoryChangedEvent;

    public void AddItem(ItemType type, int amount = 1)
    {
        if (amount <= 0) return;

        if (items.ContainsKey(type))
            items[type] += amount;
        else
            items[type] = amount;

        Debug.Log($"[Inventory] +{amount} {type} (total: {items[type]})");
        OnInventoryChanged();
    }

    public bool RemoveItem(ItemType type, int amount = 1)
    {
        if (amount <= 0) return true;

        if (!HasItem(type, amount))
        {
            Debug.Log($"[Inventory] Not enough {type} (need {amount}, have {GetCount(type)})");
            return false;
        }

        items[type] -= amount;

        if (items[type] <= 0)
            items.Remove(type);

        OnInventoryChanged();
        return true;
    }

    public bool HasItem(ItemType type, int amount = 1)
    {
        return items.TryGetValue(type, out int count) && count >= amount;
    }

    public int GetCount(ItemType type)
    {
        return items.TryGetValue(type, out int count) ? count : 0;
    }

    public Dictionary<ItemType, int> GetAllItems()
    {
        return new Dictionary<ItemType, int>(items);
    }

    void OnInventoryChanged()
    {
        OnInventoryChangedEvent?.Invoke();
    }
}
