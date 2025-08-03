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
        GameObject bag = Instantiate(bagPrefab, waypoints[2].position, Quaternion.identity);
        AirportBag bagScript = bag.GetComponent<AirportBag>();

        if (bagScript != null)
        {
            int label = Random.Range(0, bagSprites.Length);
            Sprite assignedSprite = bagSprites[label];

            bagScript.enabled = false;
            bagScript.speed = bagSpeed;
            bagScript.SetPath(waypoints);
            bagScript.SetPivot(pivotPoint);
            bagScript.SetLabel(label, assignedSprite);
            bagScript.enabled = true;
        }
    }
}
