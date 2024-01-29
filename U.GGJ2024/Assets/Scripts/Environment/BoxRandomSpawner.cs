using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxRandomSpawner : MonoBehaviour
{
    [SerializeField] GameObject mysteryBox;

    [Header("Random Spawning Time")]
    [SerializeField] float minSpawnTime;
    [SerializeField] float maxSpawnTime;

    [Header("SpawnPoints")]
    [SerializeField] Transform[] spawnPoints;

    [Header("Object Pool")]
    [SerializeField] int numOfObjects;
    [SerializeField] List<GameObject> boxes;

    private void Awake()
    {
        InitPool();
    }

    private void OnEnable()
    {
        StartCoroutine(EndlessSpawningOfBox());
    }

    void InitPool()
    {
        for (int i = 0; i < numOfObjects; i++)
        {
            GameObject box = Instantiate(mysteryBox, transform.position, Quaternion.identity);
            boxes.Add(box);
            box.SetActive(false);
        }
    }

    IEnumerator EndlessSpawningOfBox()
    {
        while (true)
        {
            float randomTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(randomTime);

            int spawnIndex = Random.Range(0, spawnPoints.Length);
            SetActiveBox(spawnPoints[spawnIndex]);
        }
    }

    void SetActiveBox(Transform spawnPosition)
    {
        if (boxes.Count > 0)
        {
            foreach (GameObject box in boxes)
            {
                if (!box.activeSelf)
                {
                    box.transform.position = spawnPosition.position;
                    box.SetActive(true);
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnPoints.Length <= 0) { return; }
        Gizmos.color = Color.green;
        foreach (Transform points in spawnPoints)
        {
            Gizmos.DrawSphere(points.position, .5f);
        }
    }
}
