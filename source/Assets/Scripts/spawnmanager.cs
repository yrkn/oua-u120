using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn; 
    public LayerMask spawnLayer; 
    public int spawnCount = 10; 
    public DayNightManager dayNightManager; 

    private float[] spawnTimes = { 60f, 80f, 120f, 140f, 160f };
    private bool[] hasSpawned; 
    private int lastCheckedDay = -1; 

    void Start()
    {
        hasSpawned = new bool[spawnTimes.Length];
    }

    void Update()
    {
        float currentTime = dayNightManager.currentTime;
        int currentDay = dayNightManager.dayCount; 

        if (currentDay != lastCheckedDay)
        {
            ResetSpawnStatus();
            lastCheckedDay = currentDay;
        }

        for (int i = 0; i < spawnTimes.Length; i++)
        {
            if (Mathf.Abs(currentTime - spawnTimes[i]) < Time.deltaTime && !hasSpawned[i])
            {
                SpawnObjects();
                hasSpawned[i] = true;
            }
        }
    }

    void ResetSpawnStatus()
    {
        for (int i = 0; i < hasSpawned.Length; i++)
        {
            hasSpawned[i] = false;
        }
    }

    void SpawnObjects()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnLayer(spawnLayer);
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionOnLayer(LayerMask layer)
    {
        float xMin = -10f, xMax = 10f;
        float zMin = -10f, zMax = 10f;

        Vector3 position;
        RaycastHit hit;

        do
        {
            float x = Random.Range(xMin, xMax);
            float z = Random.Range(zMin, zMax);
            position = new Vector3(x, 10f, z);

        } while (!Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity, layer));

        return hit.point;
    }
}
