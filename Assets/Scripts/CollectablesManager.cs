using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    [SerializeField] float spawnDistance = 200F;
    [SerializeField] Transform collectableBoost;
    [SerializeField] Transform collectableGem;
    [SerializeField] float minX = -8F;
    [SerializeField] float maxX = 8F;
    [SerializeField] float minY = 5F;
    [SerializeField] float maxY = 10F;
    [SerializeField] float spacing = 400F;
    [HideInInspector] public Rigidbody playerRb;
    float nextCollectableZ;

    void Start()
    {
        nextCollectableZ = playerRb.transform.position.z + spacing;
    }

    void SpawnBoost()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(x, y, playerRb.transform.position.z + spawnDistance);
        Transform newObject = Instantiate(collectableBoost, pos, Quaternion.identity);
        newObject.gameObject.GetComponent<CollectableBoost>().playerT = playerRb.transform;
    }

    void SpawnGem()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(x, y, playerRb.transform.position.z + spawnDistance);
        Transform newObject = Instantiate(collectableGem, pos, Quaternion.identity);
        newObject.gameObject.GetComponent<CollectableGem>().playerT = playerRb.transform;
    }

    void Update()
    {
        if (playerRb.transform.position.z > nextCollectableZ)
        {
            int x = Random.Range(0, 2);
            if (x == 0)
            {
                SpawnBoost();
            }
            else if (x == 1)
            {
                SpawnGem();
            }
            
            nextCollectableZ = playerRb.transform.position.z + spacing;
        }
    }
}
