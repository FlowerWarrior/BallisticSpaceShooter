using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] Transform enemyParticlesPrefab;
    [SerializeField] Transform enemyShootParticlePrefab;
    [SerializeField] float damageFrequency = 2F;
    float timer = 0;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && timer <= 0)
        {
            Instantiate(enemyShootParticlePrefab, transform.position, transform.rotation);
            GM.instance.playerTakeBulletDamage();
            timer = 1;
        }

        timer -= Time.deltaTime * damageFrequency;
        Debug.Log("laser touching");
    }
}
