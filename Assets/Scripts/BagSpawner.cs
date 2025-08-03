using UnityEngine;

public class BagSpawner : MonoBehaviour
{
    [Header("Bag Settings")]
    [SerializeField] private GameObject bagPrefab;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private float bagSpeed = 2f;

    [Header("Appearance Settings")]
    [SerializeField] private Sprite[] bagSprites;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private bool autoSpawn = true;

    private float spawnTimer;

    void Update()
    {
        if (!autoSpawn || bagPrefab == null) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnBag();
            spawnTimer = 0f;
        }
    }

    public void SpawnBag()
    {
        if (bagPrefab == null)
        {
            Debug.LogWarning("BagSpawner: bagPrefab is not assigned.");
            return;
        }

        if (waypoints == null || waypoints.Length < 3)
        {
            Debug.LogWarning("BagSpawner: Not enough waypoints assigned.");
            return;
        }

        GameObject bag = Instantiate(bagPrefab, waypoints[2].position, Quaternion.identity);
        AirportBag bagScript = bag.GetComponent<AirportBag>();

        if (bagScript != null)
        {
            int label = 0;
            Sprite assignedSprite = null;

            if (bagSprites != null && bagSprites.Length > 0)
            {
                label = Random.Range(0, bagSprites.Length);
                assignedSprite = bagSprites[label];
            }
            else
            {
                Debug.LogWarning("BagSpawner: bagSprites array is empty.");
            }

            bagScript.enabled = false;
            bagScript.speed = bagSpeed;
            bagScript.SetPath(waypoints);
            bagScript.SetPivot(pivotPoint);
            bagScript.SetLabel(label, assignedSprite);
            bagScript.enabled = true;
        }
    }
}
