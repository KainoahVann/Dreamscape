using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash")]
    public float dashSpeed = 18f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.75f;

    [Header("Direction")]
    public Transform cameraTransform;
    public bool dashWhereCameraLooks = true;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip dashSound;
    [Range(0f, 1f)] public float dashVolume = 1f;

    [Header("Screen Effect")]
    public Image speedEffectImage;
    public float effectAlpha = 0.35f;
    public float effectDuration = 0.2f;

    CharacterController controller;
    float dashTimer;
    float cooldownTimer;
    Vector3 dashDirection;
    Coroutine effectRoutine;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        HideEffect();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && cooldownTimer <= 0f && !controller.isGrounded)
        {
            StartDash();
        }

        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
        }
    }

void StartDash()
{
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");

    Vector3 inputDirection =
        transform.right * horizontal +
        transform.forward * vertical;

    if (inputDirection.sqrMagnitude < 0.01f)
    {
        Transform directionSource = dashWhereCameraLooks && cameraTransform != null
            ? cameraTransform
            : transform;

        inputDirection = directionSource.forward;
    }

    inputDirection.y = 0f;
    dashDirection = inputDirection.normalized;

    dashTimer = dashDuration;
    cooldownTimer = dashCooldown;

    if (audioSource != null && dashSound != null)
        audioSource.PlayOneShot(dashSound, dashVolume);

    PlayEffect();
}

    void PlayEffect()
    {
        if (speedEffectImage == null) return;

        if (effectRoutine != null)
            StopCoroutine(effectRoutine);

        effectRoutine = StartCoroutine(EffectRoutine());
    }

    IEnumerator EffectRoutine()
    {
        Color c = speedEffectImage.color;
        c.a = effectAlpha;
        speedEffectImage.color = c;

        yield return new WaitForSeconds(effectDuration);

        HideEffect();
    }

    void HideEffect()
    {
        if (speedEffectImage == null) return;

        Color c = speedEffectImage.color;
        c.a = 0f;
        speedEffectImage.color = c;
    }
}