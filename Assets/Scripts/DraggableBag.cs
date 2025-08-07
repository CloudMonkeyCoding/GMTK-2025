using UnityEngine;

[RequireComponent(typeof(AirportBag))]
public class DraggableBag : MonoBehaviour
{
    private AirportBag bag;
    private bool isDragging = false;
    private Vector3 offset;

    private void Awake()
    {
        bag = GetComponent<AirportBag>();
    }

    private void OnMouseDown()
    {
        isDragging = true;
        if (bag != null)
        {
            bag.enabled = false;
        }
        offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;
        Vector3 targetPosition = GetMouseWorldPosition() + offset;
        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (bag != null)
        {
            bag.enabled = true;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Camera cam = Camera.main;
        Vector3 mousePos = Input.mousePosition;
        if (cam == null)
        {
            mousePos.z = 0f;
            return mousePos;
        }
        mousePos.z = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(mousePos);
    }
}
