using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Tooltip("How close the player must be to press E and interact.")]
    public float interactRange = 2.5f;

    public Camera playerCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryInteract();
    }

    void TryInteract()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("PlayerInteractor: no camera assigned.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>()         ??
                hit.collider.GetComponentInParent<IInteractable>() ??
                hit.collider.GetComponentInChildren<IInteractable>();

            if (interactable != null)
                interactable.Interact(gameObject);
            else
                Debug.Log($"[Interactor] '{hit.collider.name}' has no IInteractable.");
        }
        else
        {
            Debug.Log("[Interactor] Nothing in range to interact with.");
        }
    }
}