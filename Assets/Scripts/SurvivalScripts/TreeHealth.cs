using System.Collections;
using UnityEngine;

public class TreeHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] float maxHealth = 30f;
    float currentHealth;

    [Header("Hit Animation")]
    [SerializeField] float hitScaleAmount  = 0.12f;   
    [SerializeField] float hitAnimDuration = 0.12f;   

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip   hitSound;
    [SerializeField] AudioClip   breakSound;
    [Range(0f, 1f)] [SerializeField] float hitVolume   = 1f;
    [Range(0f, 1f)] [SerializeField] float breakVolume = 1f;

    [Header("Item Drops")]
    [SerializeField] ItemDrop[] drops;  
    Vector3         originalScale;
    Coroutine       hitRoutine;
    bool            isDead = false;


    void Awake()
    {
        currentHealth = maxHealth;
        originalScale = transform.localScale;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }


    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        PlayHitSound();
        TriggerHitAnimation();

        if (currentHealth <= 0f)
            Break();
    }


    void TriggerHitAnimation()
    {
        if (hitRoutine != null)
            StopCoroutine(hitRoutine);

        hitRoutine = StartCoroutine(HitPulse());
    }

    IEnumerator HitPulse()
    {
        float half = hitAnimDuration / 2f;

        float t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float lerp = t / half;
            transform.localScale = Vector3.Lerp(
                originalScale,
                originalScale * (1f + hitScaleAmount),
                lerp
            );
            yield return null;
        }

        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float lerp = t / half;
            transform.localScale = Vector3.Lerp(
                originalScale * (1f + hitScaleAmount),
                originalScale,
                lerp
            );
            yield return null;
        }

        transform.localScale = originalScale;
    }


    void Break()
    {
        isDead = true;

        if (breakSound != null)
            AudioSource.PlayClipAtPoint(breakSound, transform.position, breakVolume);

        SpawnDrops();
        Destroy(gameObject);
    }

    void SpawnDrops()
    {
 
        Vector3 groundPos = transform.position;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit ground, 20f))
            groundPos = ground.point;

        foreach (ItemDrop drop in drops)
        {
            if (drop.itemPrefab == null) continue;

            int count = Random.Range(drop.minAmount, drop.maxAmount + 1);
            for (int i = 0; i < count; i++)
            {
                Vector3 scatter = new Vector3(
                    Random.Range(-0.4f, 0.4f),
                    0.3f,
                    Random.Range(-0.4f, 0.4f)
                );

                GameObject item = Instantiate(drop.itemPrefab, groundPos + scatter, Quaternion.identity);

                
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb == null)
                    rb = item.AddComponent<Rigidbody>();

                rb.mass = 0.5f;
                rb.linearDamping = 8f;          
                rb.angularDamping = 10f;         
                rb.constraints = RigidbodyConstraints.FreezeRotationX
                               | RigidbodyConstraints.FreezeRotationZ; 
            }
        }
    }


    void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound, hitVolume);
    }
}


[System.Serializable]
public class ItemDrop
{
    public GameObject itemPrefab;
    [Min(1)] public int minAmount = 1;
    [Min(1)] public int maxAmount = 3;
}