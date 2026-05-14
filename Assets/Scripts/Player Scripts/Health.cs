using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] float maxHealth = 100f;

    [Header("Score")]
    [SerializeField] int scoreOnDeath = 10;

    float currentHealth;
    enemyvisuals enemyvisuals;
    bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        enemyvisuals = GetComponent<enemyvisuals>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            if (enemyvisuals != null)
            {
                enemyvisuals.PlayHitSound();
                enemyvisuals.FlashHitColor();
            }
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        PlayerScore playerScore = FindFirstObjectByType<PlayerScore>();

        if (playerScore != null)
        {
            playerScore.AddScore(scoreOnDeath);
        }

        if (enemyvisuals != null)
        {
            enemyvisuals.Die();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}