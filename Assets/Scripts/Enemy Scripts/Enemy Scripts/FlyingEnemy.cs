using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Vision")]
    public float visionRange = 15f;
    public float visionAngle = 90f;
    public LayerMask visionBlockingLayers;

    [Header("Movement")]
    public float flySpeed = 3.5f;
    public float stoppingDistance = 2f;

    [Header("Hover")]
    public float hoverAmplitude = 0.3f;
    public float hoverFrequency = 1.5f;

    private Transform _player;
    private bool _hasSeenPlayer = false;
    private bool _isDead = false;
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;

        GameObject found = GameObject.FindGameObjectWithTag("Player");
        if (found != null) _player = found.transform;
    }

    public void OnDeath()
    {
        _isDead = true;
    }

    void Update()
    {
        if (_isDead || _player == null) return;

        if (!_hasSeenPlayer && CanSeePlayer())
            _hasSeenPlayer = true;

        if (_hasSeenPlayer)
        {
            MoveTowardPlayer();
            FacePlayer();
        }
        else
        {
            transform.position = _startPosition + Vector3.up * Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        }
    }

    void MoveTowardPlayer()
    {
        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance > stoppingDistance)
        {
            Vector3 dir = (_player.position - transform.position).normalized;
            transform.position += dir * flySpeed * Time.deltaTime;
            transform.position += Vector3.up * Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude * Time.deltaTime;
        }
    }

    void FacePlayer()
    {
        Vector3 dir = _player.position - transform.position;
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    bool CanSeePlayer()
    {
        Vector3 dir = _player.position - transform.position;

        if (dir.magnitude > visionRange) return false;

        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > visionAngle * 0.5f) return false;

        Vector3 eyePos = transform.position + Vector3.up * 0.5f;
        Vector3 targetPos = _player.position + Vector3.up * 1f;

        if (Physics.Linecast(eyePos, targetPos, visionBlockingLayers))
            return false;

        return true;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 eyePos = transform.position + Vector3.up * 0.5f;

        Gizmos.color = _hasSeenPlayer ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(eyePos, visionRange);

        float halfAngle = visionAngle * 0.5f;
        Vector3 leftDir = Quaternion.Euler(0, -halfAngle, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, halfAngle, 0) * transform.forward;

        Gizmos.DrawLine(eyePos, eyePos + leftDir * visionRange);
        Gizmos.DrawLine(eyePos, eyePos + rightDir * visionRange);

        int segments = 20;
        Vector3 prevPoint = eyePos + leftDir * visionRange;
        for (int i = 1; i <= segments; i++)
        {
            float t = (float)i / segments;
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);
            Vector3 d = Quaternion.Euler(0, angle, 0) * transform.forward;
            Vector3 nextPoint = eyePos + d * visionRange;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        if (_player != null)
        {
            Gizmos.color = _hasSeenPlayer ? Color.red : Color.green;
            Gizmos.DrawLine(eyePos, _player.position + Vector3.up * 1f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}