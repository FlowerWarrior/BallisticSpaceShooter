using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileScript : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 10f;
    Rigidbody rb;
    [SerializeField] Transform enemyParticlesPrefab;
    [SerializeField] Transform enemyShootParticlePrefab;
    private float timer;
    [SerializeField] private float timeUntilProjectileDestroy = 1.5f;
    [SerializeField] float bulletExplosionPower = 130F;
    [SerializeField] AudioClip[] audioShoot;
    [SerializeField] AudioClip audioImpact;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool hasTarget = false;
    [HideInInspector] public bool isPlayerProjectile = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyTimeout());

        // Shoot sound
        int audioID = Random.Range(0, audioShoot.Length-1);
        AudioSource.PlayClipAtPoint(audioShoot[audioID], transform.position);
    }

    void FixedUpdate()
    {
        if (hasTarget)
        {
            if (target != null)
            {
                Debug.Log(Quaternion.LookRotation(target.position - rb.transform.position, Vector3.up));
                rb.MoveRotation(Quaternion.LookRotation(target.position - rb.transform.position, Vector3.up));
            }
            else
            {
                hasTarget = false;
            }
        }

        rb.MovePosition(rb.transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
    }
    IEnumerator DestroyTimeout()
    {
        yield return new WaitForSeconds(timeUntilProjectileDestroy);
        if (isPlayerProjectile)
        {
            //GM.instance.combo = 0;
            //GM.instance.UpdateCombo();    
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            if (isPlayerProjectile)
            {
                if (other.gameObject.GetComponent<AsteroidScript>().hp > 0)
                {
                    other.gameObject.GetComponent<Rigidbody>().AddExplosionForce
                    (bulletExplosionPower, other.gameObject.transform.position + new Vector3(0, 0, -1), 0.2F, 0F, ForceMode.Impulse);
                    other.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * bulletExplosionPower / 4, ForceMode.Impulse);
                    
                }
                //Instantiate(hitParticlesPrefab, transform.position, transform.rotation);
                GM.instance.OnBulletHitAsteroid(other, transform.position);
                AudioSource.PlayClipAtPoint(audioImpact, transform.position);
            }
            
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Enemy" && isPlayerProjectile)
        {
            Instantiate(enemyParticlesPrefab, transform.position, transform.rotation);
            GM.instance.OnBulletHitEnemy(other);
            AudioSource.PlayClipAtPoint(audioImpact, transform.position);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player" && !isPlayerProjectile)
        {
            Instantiate(enemyShootParticlePrefab, transform.position, transform.rotation);
            GM.instance.playerTakeBulletDamage();
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Boss" && isPlayerProjectile)
        {
            if (other.gameObject.GetComponent<BossController>().isTargetable)
            {
                Instantiate(enemyShootParticlePrefab, transform.position, transform.rotation);
                AudioSource.PlayClipAtPoint(audioImpact, transform.position);
                GM.instance.bossTakeBulletDamage(other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
