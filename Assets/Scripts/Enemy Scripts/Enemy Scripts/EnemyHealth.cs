using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("Flash")]
    public Renderer enemyRenderer;
    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;

    private Color originalColor;
    private Coroutine flashRoutine;

    void Awake()
    {
        currentHealth = maxHealth;

        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponentInChildren<Renderer>();
        }

        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        Debug.Log("Enemy hit. Health: " + currentHealth);

        Flash();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Flash()
    {
        if (enemyRenderer == null) return;

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        enemyRenderer.material.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        enemyRenderer.material.color = originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}