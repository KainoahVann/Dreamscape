using System.Collections;
using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float damage = 25f;
    [SerializeField] float range = 50f;

    [Header("Recoil")]
    [SerializeField] Transform weaponVisual;
    [SerializeField] float recoilBack = 0.08f;
    [SerializeField] float recoilUp = 6f;
    [SerializeField] float recoilReturnSpeed = 12f;

    [Header("Alternate Visual")]
    [SerializeField] GameObject primaryVisual;    
    [SerializeField] GameObject altVisual;        
    [SerializeField] float altVisualDuration = 0.1f;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shootSound;

    [SerializeField] float fireRate = 0.2f;
    
    float nextTimeToFire = 0f;

    StarterAssetsInputs starterAssetsInputs;

    Vector3 originalLocalPosition;
    Quaternion originalLocalRotation;
    Vector3 targetLocalPosition;
    Quaternion targetLocalRotation;

    void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();

        if (weaponVisual == null)
            weaponVisual = transform;

        originalLocalPosition = weaponVisual.localPosition;
        originalLocalRotation = weaponVisual.localRotation;
        targetLocalPosition = originalLocalPosition;
        targetLocalRotation = originalLocalRotation;

        if (altVisual != null)
            altVisual.SetActive(false);
    }

    void Update()
    {
        HandleRecoilReturn();

        if (starterAssetsInputs.shoot && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            FireWeapon();
            starterAssetsInputs.ShootInput(false);
        }
    }

    void FireWeapon()
    {
        RaycastHit hit;
        if (Physics.Raycast(
            Camera.main.transform.position,
            Camera.main.transform.forward,
            out hit,
            range))
        {
            Health health = hit.collider.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(damage);
        }

        PlayShootSound();
        ApplyRecoil();
        StartCoroutine(ShowAltVisual());
    }

    IEnumerator ShowAltVisual()
    {
        if (primaryVisual != null) primaryVisual.SetActive(false);
        if (altVisual != null) altVisual.SetActive(true);

        yield return new WaitForSeconds(altVisualDuration);

        if (altVisual != null) altVisual.SetActive(false);
        if (primaryVisual != null) primaryVisual.SetActive(true);
    }

    void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }

    void ApplyRecoil()
    {
        targetLocalPosition += new Vector3(0f, 0f, -recoilBack);
        targetLocalRotation *= Quaternion.Euler(-recoilUp, 0f, 0f);
    }

    void HandleRecoilReturn()
    {
        weaponVisual.localPosition = Vector3.Lerp(
            weaponVisual.localPosition,
            targetLocalPosition,
            Time.deltaTime * recoilReturnSpeed
        );

        weaponVisual.localRotation = Quaternion.Lerp(
            weaponVisual.localRotation,
            targetLocalRotation,
            Time.deltaTime * recoilReturnSpeed
        );

        targetLocalPosition = Vector3.Lerp(
            targetLocalPosition,
            originalLocalPosition,
            Time.deltaTime * recoilReturnSpeed
        );

        targetLocalRotation = Quaternion.Lerp(
            targetLocalRotation,
            originalLocalRotation,
            Time.deltaTime * recoilReturnSpeed
        );
    }
}