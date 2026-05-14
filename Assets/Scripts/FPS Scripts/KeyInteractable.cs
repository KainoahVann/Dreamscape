using UnityEngine;

public class KeyInteractable : MonoBehaviour, IInteractable
{
    [Header("Sound")]
    public AudioClip pickupSound;
    [Range(0f, 1f)] public float pickupVolume = 1f;

    public void Interact(GameObject player)
    {
        PlayerInventory inventory = player.GetComponentInParent<PlayerInventory>();

        if (inventory == null)
        {
            Debug.LogWarning("Player has no PlayerInventory component.");
            return;
        }

        inventory.GiveKey();

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);
        }

        Destroy(gameObject);
    }
}