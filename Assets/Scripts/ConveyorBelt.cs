using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;  // Waypoints to follow
    [SerializeField] private Transform pivotPointTransform;
    [SerializeField] private float speed = 2f;       // Movement speed
    [SerializeField] private int startWaypointIndex = 0;

    private int currentWaypointIndex = 0;
    private bool isRotatingAroundPivot = false;
    private float rotationProgress = 0f;
    private float arcRadius;
    private Vector3 pivotPoint;

    private void Start()
    {
        currentWaypointIndex = startWaypointIndex;
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        if (isRotatingAroundPivot)
        {
            float angleStep = (speed / (2 * Mathf.PI * arcRadius)) * 360f * Time.deltaTime; // degrees/frame
            float remaining = 180f - rotationProgress;
            float actualStep = Mathf.Min(angleStep, remaining);

            RotateAroundPoint(pivotPoint, actualStep); // Clockwise
            rotationProgress += actualStep;

            if (rotationProgress >= 180f)
            {
                isRotatingAroundPivot = false;
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }

            return;
        }

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        float distanceToWaypoint = Vector3.Distance(transform.position, target.position);
        if (distanceToWaypoint < 0.05f)
        {
            transform.position = target.position;

            // If we just reached waypoint[1], start rotating around it
            if (currentWaypointIndex == 0)
            {
                isRotatingAroundPivot = true;
                rotationProgress = 0f;
                pivotPoint = pivotPointTransform.position;
                arcRadius = Vector3.Distance(transform.position, pivotPoint);
            }
            else
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    private void RotateAroundPoint(Vector3 pivot, float angleDegrees)
    {
        Vector3 dir = transform.position - pivot;
        dir = Quaternion.Euler(0, 0, angleDegrees) * dir;
        transform.position = pivot + dir;
        transform.rotation *= Quaternion.Euler(0, 0, angleDegrees);
    }
}
