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
            if (requestRenderer != null)
            {
                requestRenderer.sprite = bagSprites[desiredLabel];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AirportBag bag = collision.GetComponent<AirportBag>();
        if (bag != null && bag.GetLabel() == desiredLabel)
        {
            Destroy(collision.gameObject);
        }
    }
}
