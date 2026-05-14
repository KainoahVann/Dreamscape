using UnityEngine;

public class EnvironmentSpriteYAxis : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 targetPos = transform.position + cam.transform.rotation * Vector3.forward;

        transform.LookAt(targetPos);
    }
}
