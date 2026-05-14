using UnityEngine;
using UnityEngine.AI;

public class ContactEnemy : MonoBehaviour
{
    [Header("Vision")]
    public Transform player;
    public float visionRange = 15f;
    public float visionAngle = 90f;
    public LayerMask visionBlockingLayers;

    [Header("Movement")]
    public float chaseSpeed = 3.5f;

    NavMeshAgent agent;
    bool hasSeenPlayer = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }

        if (agent != null)
        {
            agent.speed = chaseSpeed;
        }
    }

    void Update()
    {
        if (player == null || agent == null) return;

        if (!hasSeenPlayer && CanSeePlayer())
        {
            hasSeenPlayer = true;
        }

        if (hasSeenPlayer)
        {
            agent.SetDestination(player.position);
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > visionRange)
        {
            return false;
        }

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer > visionAngle * 0.5f)
        {
            return false;
        }

        Vector3 eyePosition = transform.position + Vector3.up * 1.5f;
        Vector3 targetPosition = player.position + Vector3.up * 1f;

        if (Physics.Linecast(eyePosition, targetPosition, visionBlockingLayers))
        {
            return false;
        }

        return true;
    }

    void OnDrawGizmosSelected()
{
    Vector3 eyePos = transform.position + Vector3.up * 1.5f;

    Gizmos.color = hasSeenPlayer ? Color.red : Color.yellow;
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
        Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;
        Vector3 nextPoint = eyePos + dir * visionRange;
        Gizmos.DrawLine(prevPoint, nextPoint);
        prevPoint = nextPoint;
    }

    if (player != null)
    {
        Gizmos.color = hasSeenPlayer ? Color.red : Color.green;
        Gizmos.DrawLine(eyePos, player.position + Vector3.up * 1f);
    }
}
}