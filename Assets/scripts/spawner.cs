using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Префаби")]
    public GameObject goodPrefab;
    public GameObject starPrefab;
    public GameObject heartPrefab;

    [Header("Точки спавну")]
    public Transform[] spawnPoints;

    [Header("Інтервал між спавнами (с)")]
    public float interval = 1.2f;

    // Затримка перед першим спавном
    public float initialDelay = 0.5f;

    private GameSceneManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameSceneManager>();
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        // Ось ця затримка перед першою ітерацією
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            GameObject toSpawn;
            float rnd = Random.value;

            if (gm.Score >= 100 && rnd < 0.11f)
                toSpawn = heartPrefab;
            else
                toSpawn = (Random.value < 0.5f) ? goodPrefab : starPrefab;

            int idx = Random.Range(0, spawnPoints.Length);
            Instantiate(toSpawn, spawnPoints[idx].position, Quaternion.identity);

            yield return new WaitForSeconds(interval);
        }
    }
}
