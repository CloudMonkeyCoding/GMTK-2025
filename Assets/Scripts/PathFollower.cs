using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] protected Transform[] waypoints;         // Path to follow
    [SerializeField] protected Transform pivotPointTransform; // Used for curved rotation
    [SerializeField] public float speed = 2f;                 // Movement speed

    protected int currentWaypointIndex = 0;
    protected bool isRotatingAroundPivot = false;
    protected float rotationProgress = 0f;
    protected float arcRadius;
    protected Vector3 pivotPoint;

    public void SetPath(Transform[] path) => waypoints = path;
    public void SetPivot(Transform pivot) => pivotPointTransform = pivot;

    protected virtual void Start() { }

    protected virtual bool OnWaypointReached(int waypointIndex)
    {
        return true;
    }

    protected virtual void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

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
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        float distanceToWaypoint = Vector3.Distance(transform.position, target.position);
        if (distanceToWaypoint < 0.05f)
        {
            transform.position = target.position;

            if (!OnWaypointReached(currentWaypointIndex)) return;

            if (currentWaypointIndex == 0)
            {
                if (pivotPointTransform != null)
                {
                    isRotatingAroundPivot = true;
                    rotationProgress = 0f;
                    pivotPoint = pivotPointTransform.position;
                    arcRadius = Vector3.Distance(transform.position, pivotPoint);
                    const float epsilon = 0.001f;
                    if (arcRadius <= epsilon)
                    {
                        Debug.LogWarning($"{gameObject.name} arcRadius too small, adjusting position to avoid division by zero.", this);
                        transform.position += Vector3.right * epsilon;
                        arcRadius = epsilon;
                    }
                }
                else
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                }
            }
            else
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    protected virtual void RotateAroundPoint(Vector3 pivot, float angleDegrees)
    {
        Vector3 dir = transform.position - pivot;
        dir = Quaternion.Euler(0, 0, angleDegrees) * dir;
        transform.position = pivot + dir;
        transform.rotation *= Quaternion.Euler(0, 0, angleDegrees);
    }
}
