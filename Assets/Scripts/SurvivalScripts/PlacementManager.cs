using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] LayerMask groundLayers;
    [SerializeField] Color validColor   = new Color(0.4f, 1f, 0.4f, 0.45f);
    [SerializeField] Color invalidColor = new Color(1f, 0.3f, 0.3f, 0.45f);

    public static bool IsPlacing { get; private set; }

    GameObject ghostInstance = null;
    GameObject prefabToPlace = null;
    Renderer[] ghostRenderers;
    float      heightOffset  = 0f;
    bool       positionValid = false;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!IsPlacing) return;

        UpdateGhostPosition();

        if (Input.GetMouseButtonDown(0) && positionValid)
            Place();
        else if (Input.GetMouseButtonDown(1))
            CancelPlacement();
    }


    public void BeginPlacement(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("PlacementManager: prefab is null.");
            return;
        }

        if (ghostInstance != null)
            Destroy(ghostInstance);

        prefabToPlace = prefab;
        IsPlacing     = true;

        ghostInstance  = Instantiate(prefab);
        ghostRenderers = ghostInstance.GetComponentsInChildren<Renderer>(true);

        Bounds bounds = new Bounds(ghostInstance.transform.position, Vector3.zero);
        foreach (Renderer r in ghostRenderers)
            bounds.Encapsulate(r.bounds);
        heightOffset = bounds.extents.y;

        SetGhostColor(validColor);

        foreach (MonoBehaviour mb in ghostInstance.GetComponentsInChildren<MonoBehaviour>())
            mb.enabled = false;
        foreach (Collider col in ghostInstance.GetComponentsInChildren<Collider>())
            col.enabled = false;
    }


    void UpdateGhostPosition()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, groundLayers))
        {
            ghostInstance.transform.position = hit.point + Vector3.up * heightOffset;
            ghostInstance.transform.rotation = Quaternion.identity;
            positionValid = true;
            SetGhostColor(validColor);
        }
        else
        {
            positionValid = false;
            SetGhostColor(invalidColor);
        }
    }


    void Place()
    {
  
        Vector3    pos      = ghostInstance.transform.position;
        Quaternion rot      = ghostInstance.transform.rotation;
        string     name     = prefabToPlace.name;
        GameObject prefab   = prefabToPlace;

        EndPlacement();

        Instantiate(prefab, pos, rot);
        Debug.Log($"[Placement] {name} placed.");
    }

    void CancelPlacement()
    {
        EndPlacement();
        Debug.Log("[Placement] Cancelled.");
    }

    void EndPlacement()
    {
        IsPlacing = false;

        if (ghostInstance != null)
        {
            Destroy(ghostInstance);
            ghostInstance = null;
        }

        prefabToPlace  = null;
        ghostRenderers = null;
    }


    void SetGhostColor(Color color)
    {
        if (ghostRenderers == null) return;

        foreach (Renderer r in ghostRenderers)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            r.GetPropertyBlock(block);
            block.SetColor("_BaseColor", color);
            block.SetColor("_Color",     color);
            r.SetPropertyBlock(block);
        }
    }
}