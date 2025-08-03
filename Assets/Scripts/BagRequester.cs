using UnityEngine;

public class BagRequester : MonoBehaviour
{
    [SerializeField] private SpriteRenderer requestRenderer;
    [SerializeField] private Sprite[] bagSprites;

    private int desiredLabel = -1;

    private void Start()
    {
        if (bagSprites != null && bagSprites.Length > 0)
        {
            desiredLabel = Random.Range(0, bagSprites.Length);
            Debug.Log($"{name} requesting bag with label {desiredLabel}");
            if (requestRenderer != null)
            {
                requestRenderer.sprite = bagSprites[desiredLabel];
            }
        }
        else
        {
            Debug.LogWarning($"{name} has no bag sprites assigned");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("RGRA+FKLFKJFKJLDSHJKF");
        AirportBag bag = collision.GetComponent<AirportBag>();
        if (bag != null)
        {
            Debug.Log($"{name} saw bag with label {bag.GetLabel()}");
            if (bag.GetLabel() == desiredLabel)
            {
                Debug.Log($"Correct bag delivered to {name}");
                Destroy(collision.gameObject);
            }
        }
        else
        {
            Debug.Log($"{name} triggered by non-bag object {collision.name}");
        }
    }
}
