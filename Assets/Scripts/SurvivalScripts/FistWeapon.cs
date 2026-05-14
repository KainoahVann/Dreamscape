using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class FistWeapon : MonoBehaviour
{
    [SerializeField] float damage   = 10f;
    [SerializeField] float range    = 1.8f;

    [Header("Punch Animation")]
    [SerializeField] Transform weaponVisual;
    [SerializeField] float punchForward     = 0.08f;
    [SerializeField] float punchReturnSpeed = 12f;

    [Header("Alternate Visual")]
    [SerializeField] GameObject primaryVisual;
    [SerializeField] GameObject altVisual;
    [SerializeField] GameObject hoverVisual;
    [SerializeField] float altVisualDuration = 0.1f;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip punchSound;

    [SerializeField] float attackRate = 0.4f;

    float nextTimeToAttack = 0f;
    bool  isPunching       = false;

    StarterAssetsInputs starterAssetsInputs;

    Vector3 originalLocalPosition;
    Vector3 targetLocalPosition;


    Renderer[] fistRenderers;

void Awake()
{
    RefreshReferences();

    if (altVisual != null) altVisual.SetActive(false);
    if (hoverVisual != null) hoverVisual.SetActive(false);
}

    void BuildFistRendererList()
    {
        Renderer[] all = GetComponentsInChildren<Renderer>(includeInactive: true);

        HashSet<Renderer> exclude = new HashSet<Renderer>();

        if (hoverVisual != null)
            foreach (Renderer r in hoverVisual.GetComponentsInChildren<Renderer>(true))
                exclude.Add(r);

        if (altVisual != null)
            foreach (Renderer r in altVisual.GetComponentsInChildren<Renderer>(true))
                exclude.Add(r);

        List<Renderer> fist = new List<Renderer>();
        foreach (Renderer r in all)
            if (!exclude.Contains(r))
                fist.Add(r);

        fistRenderers = fist.ToArray();
        Debug.Log($"[FistWeapon] Found {fistRenderers.Length} fist renderer(s).");
    }

    void Update()
    {
        HandlePunchReturn();
        HandleHoverVisual();

        if (starterAssetsInputs.shoot && Time.time >= nextTimeToAttack)
        {
            nextTimeToAttack = Time.time + attackRate;
            Attack();
            starterAssetsInputs.ShootInput(false);
        }
    }

void Attack()
{
    RaycastHit hit;
    if (Physics.Raycast(
        Camera.main.transform.position,
        Camera.main.transform.forward,
        out hit,
        range))
    {
        IDamageable damageable = hit.collider.GetComponent<IDamageable>()
                              ?? hit.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
            damageable.TakeDamage(damage);
    }

    PlayPunchSound();
    ApplyPunch();
    StartCoroutine(ShowAltVisual());
}
    void ApplyPunch()
    {
        targetLocalPosition += new Vector3(0f, 0f, punchForward);
    }

    void HandlePunchReturn()
    {
        weaponVisual.localPosition = Vector3.Lerp(
            weaponVisual.localPosition,
            targetLocalPosition,
            Time.deltaTime * punchReturnSpeed
        );

        targetLocalPosition = Vector3.Lerp(
            targetLocalPosition,
            originalLocalPosition,
            Time.deltaTime * punchReturnSpeed
        );
    }

    IEnumerator ShowAltVisual()
    {
        isPunching = true;

        SetFistVisible(false);
        if (hoverVisual != null) hoverVisual.SetActive(false);
        if (altVisual   != null) altVisual.SetActive(true);

        yield return new WaitForSeconds(altVisualDuration);

        if (altVisual != null) altVisual.SetActive(false);

        isPunching = false;
    }

    void HandleHoverVisual()
    {
        if (isPunching) return;

        bool lookingAtItem = false;

        RaycastHit hover;
        if (Physics.Raycast(
            Camera.main.transform.position,
            Camera.main.transform.forward,
            out hover,
            5f))
        {
            lookingAtItem = hover.collider.GetComponent<DroppedItem>() != null
                         || hover.collider.GetComponentInParent<DroppedItem>() != null;
        }

        SetFistVisible(!lookingAtItem);
        if (hoverVisual != null) hoverVisual.SetActive(lookingAtItem);
    }

    void SetFistVisible(bool visible)
    {
        foreach (Renderer r in fistRenderers)
            r.enabled = visible;
    }

    void PlayPunchSound()
    {
        if (audioSource != null && punchSound != null)
            audioSource.PlayOneShot(punchSound);
    }

public void RefreshReferences()
{
    starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();

    if (weaponVisual == null)
        weaponVisual = transform;

    originalLocalPosition = weaponVisual.localPosition;
    targetLocalPosition = originalLocalPosition;

    BuildFistRendererList();
}
}