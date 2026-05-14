using UnityEngine;

public class PlayerToolManager : MonoBehaviour
{
    [SerializeField] private Transform toolHolder;
    [SerializeField] private GameObject startingTool;

    private GameObject currentTool;

    void Start()
    {
        currentTool = startingTool;
    }

    public void EquipTool(GameObject toolPrefab)
    {
        if (toolPrefab == null)
        {
            Debug.LogWarning("Tool prefab missing.");
            return;
        }

        if (currentTool != null)
            Destroy(currentTool);

        GameObject newTool = Instantiate(toolPrefab);

        newTool.transform.SetParent(toolHolder, false);

        currentTool = newTool;

        FistWeapon weapon = currentTool.GetComponentInChildren<FistWeapon>();
        if (weapon != null)
            weapon.RefreshReferences();
    }
}