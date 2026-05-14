using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;

    public Vector3 Delta { get; private set; }

    private Vector3 target;
    private Vector3 lastPosition;

    void Start()
    {
        target = pointB.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
            target = (target == pointA.position) ? pointB.position : pointA.position;

        Delta = transform.position - lastPosition;
        lastPosition = transform.position;
    }
}