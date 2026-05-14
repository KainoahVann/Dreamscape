using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public float damage = 10f;
    public float damageCooldown = 1f;

    [Header("Audio")]
    public AudioClip damageSound;
    public float volume = 1f;

    private float nextDamageTime;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        TryDamage(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDamage(other.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        TryDamage(collision.gameObject);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        TryDamage(hit.gameObject);
    }

    private void TryDamage(GameObject target)
    {
        if (Time.time < nextDamageTime) return;

        IDamageable damageable =
            target.GetComponent<IDamageable>() ??
            target.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            nextDamageTime = Time.time + damageCooldown;
            Debug.Log("Contact damage dealt.");

            if (damageSound != null)
                _audioSource.PlayOneShot(damageSound, volume);
        }
    }
}