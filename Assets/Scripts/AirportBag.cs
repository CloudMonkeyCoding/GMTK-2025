using UnityEngine;

public class AirportBag : PathFollower
{
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

    protected override void Update()
    {
        if (currentWaypointIndex == 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        base.Update();
    }

    protected override void RotateAroundPoint(Vector3 pivot, float angleDegrees)
    {
        Vector3 dir = transform.position - pivot;
        dir = Quaternion.Euler(0, 0, angleDegrees) * dir;
        transform.position = pivot + dir;
        transform.rotation = Quaternion.Euler(0, 0, rotationProgress); // Absolute rotation
    }
}
