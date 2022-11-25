using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] Transform bossPrefab;
    [SerializeField] float spawnDistanceZ = 200F;

    [HideInInspector] public Rigidbody playerRb;

    public void SpawnBoss()
    {
        // Instantiate transform
        Vector3 prefabPos = new Vector3(0, 5, playerRb.transform.position.z + spawnDistanceZ);
        Quaternion prefabRot = Quaternion.Euler(0, 0, 0);

        GameObject boss = Instantiate(bossPrefab, prefabPos, prefabRot).gameObject;
        boss.GetComponent<BossController>().playerRb = playerRb;
        boss.transform.parent = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
