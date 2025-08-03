using UnityEngine;

public class AirportBag : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;         // Path to follow
    [SerializeField] private Transform pivotPointTransform; // Used for curved rotation
    [SerializeField] public float speed = 2f;              // Movement speed

    private int currentWaypointIndex = 0;
    private bool isRotatingAroundPivot = false;
    private float rotationProgress = 0f;
    private float arcRadius;
    private Vector3 pivotPoint;

    public void SetPath(Transform[] path) => waypoints = path;
    public void SetPivot(Transform pivot) => pivotPointTransform = pivot;

    private int label;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetLabel(int labelIndex, Sprite sprite)
    {
        label = labelIndex;
        if (spriteRenderer != null && sprite != null)
            spriteRenderer.sprite = sprite;
    }

    public int GetLabel()
    {
        return label;
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

        if (currentWaypointIndex == 0) transform.rotation = Quaternion.Euler(0, 0, 0); // reset rotation

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        float distanceToWaypoint = Vector3.Distance(transform.position, target.position);
        if (distanceToWaypoint < 0.05f)
        {
            transform.position = target.position;

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
        transform.rotation = Quaternion.Euler(0, 0, rotationProgress); // Absolute rotation
    }
}
