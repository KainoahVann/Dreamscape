using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Health Display")]
    public Image healthImage;
    public TMP_Text healthPercentText;

    [Header("Damage Flash")]
    public Image redFlashImage;
    public float flashDuration = 0.15f;
    public float flashAlpha = 0.45f;

    [Header("Pulse")]
    public RectTransform imageToPulse;
    public float pulseScale = 1.2f;
    public float pulseDuration = 0.15f;

    Vector3 originalScale;
    Coroutine flashRoutine;
    Coroutine pulseRoutine;

    void Awake()
    {
        if (imageToPulse != null)
        {
            originalScale = imageToPulse.localScale;
        }

        if (redFlashImage != null)
        {
            Color c = redFlashImage.color;
            c.a = 0f;
            redFlashImage.color = c;
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        float percent = currentHealth / maxHealth;

        if (healthImage != null)
        {
            healthImage.fillAmount = percent;
        }

        if (healthPercentText != null)
        {
            healthPercentText.text = Mathf.RoundToInt(percent * 100f) + "%";
        }
    }

    public void PlayHitEffects()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
        }

        flashRoutine = StartCoroutine(FlashRed());
        pulseRoutine = StartCoroutine(PulseImage());
    }

    IEnumerator FlashRed()
    {
        if (redFlashImage == null) yield break;

        Color c = redFlashImage.color;
        c.a = flashAlpha;
        redFlashImage.color = c;

        yield return new WaitForSeconds(flashDuration);

        c.a = 0f;
        redFlashImage.color = c;
    }

    IEnumerator PulseImage()
    {
        if (imageToPulse == null) yield break;

        imageToPulse.localScale = originalScale * pulseScale;

        yield return new WaitForSeconds(pulseDuration);

        imageToPulse.localScale = originalScale;
    }

    public void ClearHitEffects()
{
    if (redFlashImage != null)
    {
        Color c = redFlashImage.color;
        c.a = 0f;
        redFlashImage.color = c;
    }

    if (imageToPulse != null)
    {
        imageToPulse.localScale = originalScale;
    }
}
}