using UnityEngine;

public class EnvironmentSprite : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 targetPos = transform.position + cam.transform.rotation * Vector3.forward;
        targetPos.y = transform.position.y;

        transform.LookAt(targetPos);
    }
}
