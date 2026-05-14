using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public PlayerHealthUI healthUI;
    public GameObject gameOverMenu;

    [Header("Disable On Death")]
    public MonoBehaviour[] scriptsToDisable;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip hitSound;
    [Range(0f, 1f)] public float hitVolume = 1f;

    public bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;

        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false);
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (healthUI != null)
        {
            healthUI.UpdateHealth(currentHealth, maxHealth);
            healthUI.ClearHitEffects();
        }

        Time.timeScale = 1f;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound, hitVolume);
        }

        if (healthUI != null)
        {
            healthUI.UpdateHealth(currentHealth, maxHealth);
            healthUI.PlayHitEffects();
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("PLAYER DIED - GAME OVER");

        if (healthUI != null)
        {
            healthUI.ClearHitEffects();
        }

        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Game Over Menu is not assigned on PlayerHealth.");
        }

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = false;
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
}