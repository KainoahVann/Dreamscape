using System.Collections;
using UnityEngine;

public class CampfireObject : MonoBehaviour, IInteractable
{
    [Header("Cooking")]
    [SerializeField] ItemType rawItem    = ItemType.RawMeat;
    [SerializeField] ItemType cookedItem = ItemType.CookedMeat;
    [SerializeField] float    cookTime   = 5f;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip   fireLoop;
    [SerializeField] AudioClip   sizzleSound;
    [SerializeField] float       maxHearDistance = 15f;  
    bool isCooking = false;

    CampfireCookingUI cookingUI;

    void Start()
    {
        cookingUI = Object.FindFirstObjectByType<CampfireCookingUI>();

        if (cookingUI == null)
            Debug.LogWarning("[Campfire] No CampfireCookingUI found in scene.");

        if (audioSource != null && fireLoop != null)
        {
            audioSource.spatialBlend  = 1f;                              
            audioSource.rolloffMode   = AudioRolloffMode.Linear;         
            audioSource.minDistance   = 2f;                              
            audioSource.maxDistance   = maxHearDistance;
            audioSource.clip          = fireLoop;
            audioSource.loop          = true;
            audioSource.Play();
        }
    }


    public void Interact(GameObject player)
    {
        if (cookingUI == null)
        {
            Debug.LogWarning("[Campfire] CampfireCookingUI not found.");
            return;
        }

        cookingUI.Open(this);
    }


    public void StartCooking(SurvivalInventory inventory, CampfireCookingUI ui)
    {
        if (isCooking) return;

        if (!inventory.HasItem(rawItem, 1))
        {
            Debug.Log("[Campfire] No raw meat.");
            return;
        }

        inventory.RemoveItem(rawItem, 1);

        if (audioSource != null && sizzleSound != null)
            audioSource.PlayOneShot(sizzleSound);

        StartCoroutine(CookRoutine(inventory, ui));
    }

    IEnumerator CookRoutine(SurvivalInventory inventory, CampfireCookingUI ui)
    {
        isCooking = true;
        ui.ShowProgress(cookTime);

        yield return new WaitForSeconds(cookTime);

        inventory.AddItem(cookedItem, 1);
        isCooking = false;
        ui.CookingFinished();
    }
}