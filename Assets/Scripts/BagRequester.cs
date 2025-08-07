using System;
using UnityEngine;

public class BagRequester : MonoBehaviour
{
    [SerializeField] private SpriteRenderer requestRenderer;
    [SerializeField] private Sprite[] bagSprites;

    private int desiredLabel = -1;
    public event Action BagDelivered;

    private void Start()
    {
        RequestRandomBag();
    }

    public void RequestRandomBag()
    {
        if (bagSprites != null && bagSprites.Length > 0)
        {
            desiredLabel = UnityEngine.Random.Range(0, bagSprites.Length);
            if (requestRenderer != null)
            {
                requestRenderer.sprite = bagSprites[desiredLabel];
            }
        }
        else
        {
            desiredLabel = -1;
        }
    }

    public void ClearRequest()
    {
        desiredLabel = -1;
        if (requestRenderer != null)
        {
            requestRenderer.sprite = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AirportBag bag = collision.GetComponent<AirportBag>();
        if (bag != null)
        {
            if (bag.GetLabel() == desiredLabel)
            {
                ScoreManager.Instance?.AddScore();
                Destroy(collision.gameObject);
                BagDelivered?.Invoke();
                ClearRequest();
            }
        }
    }
}
