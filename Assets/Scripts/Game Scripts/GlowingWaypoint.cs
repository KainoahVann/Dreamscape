using UnityEngine;

public class GlowingWaypoint : MonoBehaviour
{
    [Header("Pulse")]
    public float minIntensity = 1.5f;
    public float maxIntensity = 4f;
    public float pulseSpeed = 2f;

    [Header("Bob")]
    public float bobHeight = 0.3f;
    public float bobSpeed = 1.5f;

    private Light _light;
    private Vector3 _startPosition;

    void Start()
    {
        _light = GetComponent<Light>();
        _startPosition = transform.position;
    }

    void Update()
    {
        if (_light != null)
        {
            _light.intensity = Mathf.Lerp(minIntensity, maxIntensity,
                (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f);
        }

        transform.position = _startPosition +
            Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobHeight;
    }
}