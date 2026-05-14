using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    [Header("Hunger")]
    public float maxHunger     = 100f;
    public float currentHunger;

    [Header("Drain")]
    [Tooltip("Hunger lost per second")]
    public float drainRate     = 1f;

    [Header("Starvation")]
    [Tooltip("Health damage per second dealt when hunger hits zero")]
    public float starvationDamage = 2f;
    PlayerHealth playerHealth;

    [Header("UI")]
    public PlayerHungerUI hungerUI;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip eatSound;
    [Range(0f, 1f)] public float eatVolume = 1f;

    void Awake()
    {
        currentHunger = maxHunger;
        playerHealth  = GetComponent<PlayerHealth>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (hungerUI != null)
            hungerUI.UpdateHunger(currentHunger, maxHunger);
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.isDead) return;

        currentHunger -= drainRate * Time.deltaTime;
        currentHunger  = Mathf.Clamp(currentHunger, 0f, maxHunger);

        if (hungerUI != null)
            hungerUI.UpdateHunger(currentHunger, maxHunger);

        if (currentHunger <= 0f && playerHealth != null)
            playerHealth.TakeDamage(starvationDamage * Time.deltaTime);
    }

    public void Feed(float amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0f, maxHunger);

        if (hungerUI != null)
            hungerUI.UpdateHunger(currentHunger, maxHunger);

        if (audioSource != null && eatSound != null)
            audioSource.PlayOneShot(eatSound, eatVolume);
    }
}