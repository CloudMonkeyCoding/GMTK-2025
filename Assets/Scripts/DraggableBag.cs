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
        Debug.Log($"DraggableBag initialized on {name}");
    }

    private void OnMouseDown()
    {
        Debug.Log($"DraggableBag {name} OnMouseDown");
        isDragging = true;
        if (bag != null)
        {
            bag.enabled = false;
            Debug.Log($"Disabled AirportBag on {name}");
        }
        offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;
        Vector3 targetPosition = GetMouseWorldPosition() + offset;
        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        Debug.Log($"Dragging {name} to {transform.position}");
    }

    private void OnMouseUp()
    {
        Debug.Log($"DraggableBag {name} OnMouseUp");
        isDragging = false;
        if (bag != null)
        {
            bag.enabled = true;
            Debug.Log($"Re-enabled AirportBag on {name}");
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
