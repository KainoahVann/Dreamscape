using System.Collections;
using UnityEngine;


public class AnimalWander : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed    = 1.5f;
    [SerializeField] float wanderRadius = 6f;     

    [Header("Idle")]
    [SerializeField] float idleTimeMin  = 2f;
    [SerializeField] float idleTimeMax  = 5f;

    [Header("Arrival")]
    [SerializeField] float arrivalDistance = 0.25f;   

    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [Tooltip("If true, sprite faces RIGHT when moving right (default for most animal sprites)")]
    [SerializeField] bool defaultFacingRight = true;

    Vector3           spawnPoint;
    Vector3           destination;
    CharacterController cc;

    public enum State { Idle, Walking }
    public State CurrentState { get; private set; } = State.Idle;

    void Awake()
    {
        spawnPoint = transform.position;
        cc         = GetComponent<CharacterController>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(WanderLoop());
    }


    IEnumerator WanderLoop()
    {
        while (true)
        {
            CurrentState = State.Idle;
            float idleTime = Random.Range(idleTimeMin, idleTimeMax);
            yield return new WaitForSeconds(idleTime);

            destination = PickDestination();
            CurrentState       = State.Walking;

            while (XZDistance(transform.position, destination) > arrivalDistance)
            {
                MoveToward(destination);
                UpdateSpriteFacing();
                yield return null;
            }
        }
    }


    void MoveToward(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        direction.y = 0f;

        Vector3 move = direction * moveSpeed * Time.deltaTime;

        if (cc != null)
        {
            move.y = -9.81f * Time.deltaTime;
            cc.Move(move);
        }
        else
        {
            transform.position += move;
        }
    }


    Vector3 PickDestination()
    {
        float   angle     = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float   radius    = Random.Range(wanderRadius * 0.2f, wanderRadius * 0.8f);
        Vector3 candidate = spawnPoint + new Vector3(
            Mathf.Cos(angle) * radius,
            0f,
            Mathf.Sin(angle) * radius
        );

        if (Physics.Raycast(candidate + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 20f))
            candidate.y = hit.point.y;

        return candidate;
    }

    float XZDistance(Vector3 a, Vector3 b)
    {
        float dx = a.x - b.x;
        float dz = a.z - b.z;
        return Mathf.Sqrt(dx * dx + dz * dz);
    }


    void UpdateSpriteFacing()
    {
        if (spriteRenderer == null) return;

        float horizontal = destination.x - transform.position.x;
        if (Mathf.Abs(horizontal) < 0.05f) return;

        bool movingRight = horizontal > 0f;

        spriteRenderer.flipX = defaultFacingRight ? !movingRight : movingRight;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.9f, 0.4f, 0.3f);
        Gizmos.DrawSphere(Application.isPlaying ? spawnPoint : transform.position, wanderRadius);
    }
}