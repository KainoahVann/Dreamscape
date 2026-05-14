using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Owner")]
    public GameObject owner;

    [Header("Damage")]
    public float damage = 10f;
    public float lifetime = 5f;

    [Header("Sound")]
    public AudioClip impactSound;
    [Range(0f, 1f)] public float impactVolume = 1f;

    bool hasHit = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (owner != null && other.transform.IsChildOf(owner.transform))
            return;

        TryHit(other.gameObject, other.isTrigger);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        if (owner != null && collision.transform.IsChildOf(owner.transform))
            return;

        TryHit(collision.gameObject, false);
    }

    void TryHit(GameObject hitObject, bool isTrigger)
    {
        if (isTrigger) return;

        hasHit = true;

        IDamageable damageable =
            hitObject.GetComponent<IDamageable>() ??
            hitObject.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        PlayImpactSound();
        Destroy(gameObject);
    }

    void PlayImpactSound()
    {
        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(
                impactSound,
                transform.position,
                impactVolume
            );
        }
    }
}