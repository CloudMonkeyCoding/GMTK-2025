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
        if (bagPrefab == null || waypoints == null || waypoints.Length == 0)
            return;

        Vector3 spawnPosition = waypoints[2].position;
        GameObject bag = Instantiate(bagPrefab, spawnPosition, Quaternion.identity);
        AirportBag bagScript = bag.GetComponent<AirportBag>();

        if (bagScript != null)
        {
            bagScript.enabled = false;
            bagScript.speed = bagSpeed;
            bagScript.SetPath(waypoints);
            bagScript.SetPivot(pivotPoint);

            if (bagSprites != null && bagSprites.Length > 0)
            {
                int label = Random.Range(0, bagSprites.Length);
                Sprite assignedSprite = bagSprites[label];
                bagScript.SetLabel(label, assignedSprite);
            }

            bagScript.enabled = true;
        }
    }
}
