using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SurvivalInventory inventory;
    [SerializeField] GameObject inventoryRoot;
    [SerializeField] Transform itemListParent;
    [SerializeField] InventorySlotUI itemSlotPrefab;

    [Header("Controls")]
    [SerializeField] KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] bool pauseGameWhenOpen = false;

    bool isOpen;

    void Awake()
    {
        if (inventory == null)
            inventory = FindFirstObjectByType<SurvivalInventory>();

        SetOpen(false);
    }

    void OnEnable()
    {
        if (inventory != null)
            inventory.OnInventoryChangedEvent += Refresh;
    }

    void OnDisable()
    {
        if (inventory != null)
            inventory.OnInventoryChangedEvent -= Refresh;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && !PlacementManager.IsPlacing)
            SetOpen(!isOpen);
    }

    public void SetOpen(bool open)
    {
        bool wasOpen = isOpen;
        isOpen = open;

        if (inventoryRoot != null)
            inventoryRoot.SetActive(isOpen);

        if (pauseGameWhenOpen)
            Time.timeScale = isOpen ? 0f : 1f;

        if (isOpen && !wasOpen)  MenuCursor.OnOpen();
        if (!isOpen && wasOpen)  MenuCursor.OnClose();

        if (isOpen) Refresh();
    }

    public void Refresh()
    {
        if (inventory == null || itemListParent == null || itemSlotPrefab == null)
            return;

        foreach (Transform child in itemListParent)
            Destroy(child.gameObject);

        foreach (var pair in inventory.GetAllItems())
        {
            InventorySlotUI slot = Instantiate(itemSlotPrefab, itemListParent);
            slot.Set(pair.Key, pair.Value);
        }
    }
}