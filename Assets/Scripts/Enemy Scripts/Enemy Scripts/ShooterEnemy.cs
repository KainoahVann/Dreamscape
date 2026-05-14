using UnityEngine;
using UnityEngine.AI;

public class ShooterEnemy : MonoBehaviour
{
    [Header("Vision")]
    public Transform player;
    public float visionRange = 20f;
    public float visionAngle = 100f;
    public LayerMask visionBlockingLayers;

    [Header("Movement")]
    public NavMeshAgent agent;
    public float chaseSpeed = 3f;
    public float stoppingDistance = 8f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float secondsBetweenShots = 2f;
    public float projectileSpeed = 12f;

    [Header("Detection")]
    [Tooltip("Player must move this far away before the enemy loses sight and stops shooting")]
    public float loseSightRange = 28f;   // should be bigger than visionRange

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootVolume = 1f;

    bool hasSeenPlayer = false;
    float shotTimer = 0f;

    void Awake()
    {
        if (agent == null)       agent       = GetComponent<NavMeshAgent>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        // Make shoot sound 3D so the min/max distance on the AudioSource actually works
        if (audioSource != null)
        {
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode  = AudioRolloffMode.Linear;
        }
    }

    void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null) player = foundPlayer.transform;
        }

        if (agent != null)
        {
            agent.speed            = chaseSpeed;
            agent.stoppingDistance = stoppingDistance;
        }
    }

    void Update()
    {
        if (player == null) return;

        if (!hasSeenPlayer)
        {
            if (CanSeePlayer())
            {
                hasSeenPlayer = true;
                shotTimer = 0f;
            }
            else
            {
                return;
            }
        }

        // Lose sight if player gets far enough away — stops infinite-range shooting
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (distToPlayer > loseSightRange)
        {
            hasSeenPlayer = false;
            if (agent != null && agent.isOnNavMesh) agent.ResetPath();
            return;
        }

        FacePlayer();
        MoveTowardPlayer();

        shotTimer -= Time.deltaTime;

        if (shotTimer <= 0f)
        {
            Shoot();
            shotTimer = secondsBetweenShots;
        }
    }

    void MoveTowardPlayer()
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stoppingDistance)
            agent.SetDestination(player.position);
        else
            agent.ResetPath();
    }

    bool CanSeePlayer()
    {
        Vector3 dir = player.position - transform.position;

        if (dir.magnitude > visionRange) return false;

        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > visionAngle * 0.5f) return false;

        Vector3 eyePos    = transform.position + Vector3.up * 1.5f;
        Vector3 targetPos = player.position + Vector3.up * 1f;

        if (Physics.Linecast(eyePos, targetPos, visionBlockingLayers))
            return false;

        return true;
    }

    void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound, shootVolume);

        Vector3 targetPos = player.position + Vector3.up * 1f;
        Vector3 dir       = (targetPos - firePoint.position).normalized;

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position + dir * 1f,
            Quaternion.LookRotation(dir)
        );

        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
        if (enemyProjectile != null)
            enemyProjectile.owner = gameObject;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity    = false;
            rb.isKinematic   = false;
            rb.linearVelocity = dir * projectileSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 eyePos = transform.position + Vector3.up * 1.5f;

        Gizmos.color = hasSeenPlayer ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(eyePos, visionRange);

        // Show lose-sight range in orange
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
        Gizmos.DrawWireSphere(eyePos, loseSightRange);

        float halfAngle = visionAngle * 0.5f;
        Vector3 leftDir  = Quaternion.Euler(0, -halfAngle, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0,  halfAngle, 0) * transform.forward;

        Gizmos.color = hasSeenPlayer ? Color.red : Color.yellow;
        Gizmos.DrawLine(eyePos, eyePos + leftDir  * visionRange);
        Gizmos.DrawLine(eyePos, eyePos + rightDir * visionRange);

        int segments = 20;
        Vector3 prevPoint = eyePos + leftDir * visionRange;
        for (int i = 1; i <= segments; i++)
        {
            float t     = (float)i / segments;
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}