using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemyvisuals : MonoBehaviour
{
    NavMeshAgent agent;

    [Header("Death")]
    [SerializeField] Material deathMaterial;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip deathSound;

    [Header("Ambient Loop")]
    [SerializeField] AudioSource ambientAudioSource;
    [SerializeField] AudioClip ambientSound;
    [SerializeField] float ambientMinDistance = 1f;   // full volume within this distance
    [SerializeField] float ambientMaxDistance = 20f;  // silent beyond this distance

    [Header("Hit Flash")]
    [SerializeField] Color hitFlashColor = Color.red;
    [SerializeField] float hitFlashDuration = 0.2f;

    Renderer[] renderers;
    Collider[] colliders;

    Color[] originalColors;
    Coroutine flashRoutine;
    bool isDead = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].material.color;
    }

    void Start()
    {
        if (ambientSound != null)
        {
            if (ambientAudioSource == null)
            {
                ambientAudioSource = gameObject.AddComponent<AudioSource>();
            }

            ambientAudioSource.clip = ambientSound;
            ambientAudioSource.loop = true;
            ambientAudioSource.spatialBlend = 1f;        // full 3D so distance affects volume
            ambientAudioSource.rolloffMode = AudioRolloffMode.Linear;
            ambientAudioSource.minDistance = ambientMinDistance;
            ambientAudioSource.maxDistance = ambientMaxDistance;
            ambientAudioSource.Play();
        }
    }

    public void PlayHitSound()
    {
        if (isDead) return;

        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }

    public void FlashHitColor()
    {
        if (isDead) return;

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = hitFlashColor;

        yield return new WaitForSeconds(hitFlashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
                renderers[i].material.color = originalColors[i];
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (ambientAudioSource != null)
            ambientAudioSource.Stop();

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
            Destroy(gameObject, deathSound.length);
        }
        else
        {
            Destroy(gameObject);
        }

        if (deathMaterial != null)
        {
            foreach (Renderer rend in renderers)
                rend.material = deathMaterial;
        }

        foreach (Collider col in colliders)
            col.enabled = false;
    }
}