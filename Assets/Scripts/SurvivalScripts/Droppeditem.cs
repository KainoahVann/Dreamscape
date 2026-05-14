using UnityEngine;
public class DroppedItem : MonoBehaviour, IInteractable
{
    [Header("Item Info")]
    public ItemType itemType;          
    [Min(1)] public int amount = 1;   

    [Header("Audio")]
    public AudioClip pickupSound;
    [Range(0f, 1f)] public float pickupVolume = 1f;


    public void Interact(GameObject player)
    {
        SurvivalInventory inventory = player.GetComponentInParent<SurvivalInventory>()
                                   ?? player.GetComponent<SurvivalInventory>();

        if (inventory == null)
        {
            Debug.LogWarning("DroppedItem: player has no SurvivalInventory.");
            return;
        }

        inventory.AddItem(itemType, amount);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);

        Destroy(gameObject);
    }
}