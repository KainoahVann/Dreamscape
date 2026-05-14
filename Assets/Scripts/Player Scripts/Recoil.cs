using UnityEngine;

public class Recoil : MonoBehaviour
{
    Vector3 originalPos;
    Quaternion originalRot;

    [Header("Recoil Settings")]
    public float recoilBack = 0.1f;
    public float recoilUp = 5f;
    public float returnSpeed = 10f;

    Vector3 targetPos;
    Quaternion targetRot;

    void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;

        targetPos = originalPos;
        targetRot = originalRot;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * returnSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * returnSpeed);

        targetPos = Vector3.Lerp(targetPos, originalPos, Time.deltaTime * returnSpeed);
        targetRot = Quaternion.Lerp(targetRot, originalRot, Time.deltaTime * returnSpeed);
    }

    public void Fire()
    {
        targetPos += new Vector3(0, 0, -recoilBack);

        targetRot *= Quaternion.Euler(-recoilUp, 0, 0);
    }
}