using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int hp = 3;
    public float moveSpeed = 0.8F;
    public float fireRate = 2F;
    public float cutoffZ = -24F;
    public bool canShoot = false;
    public float distanceZ;
    [HideInInspector] public Rigidbody playerRb;
    [SerializeField] Transform projectilePrefab;
    [SerializeField] Transform destructionParticles;
    [SerializeField] float timeToKamikaze = 8F;
    [SerializeField] public float kamikazeSpeed = 8F;
    [SerializeField] float explosionRadius = 3F;
    [SerializeField] float correctionSpeedZ = 100F;
    Rigidbody thisRb;
    int secPassed = 0;
    bool isKamikaze = false;
    bool shootCoroutineRunning = false;

    void Start()
    {
        thisRb = GetComponent<Rigidbody>();
        StartCoroutine(CountSeconds());
    }

    public void OnCrashed()
    {
        Instantiate(destructionParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    IEnumerator ShootLoop()
    {
        shootCoroutineRunning = true;
        while (canShoot)
        {
            Shoot();
            yield return new WaitForSeconds(1 / fireRate);
        }
    }

    IEnumerator CountSeconds()
    {
        Debug.Log(timeToKamikaze);

        while (secPassed < timeToKamikaze)
        {
            secPassed++;
            yield return new WaitForSeconds(1);
        }
        
        StopCoroutine(ShootLoop());
        isKamikaze = true;
        Debug.Log("Done");
    }
    
    void FixedUpdate()
    {
        Vector3 moveDir = playerRb.transform.position - transform.position;
        float speed = kamikazeSpeed; // often overriten in next if

        if (!isKamikaze)
        {
            speed = moveSpeed;
            float z = 0;
            if (playerRb.transform.position.z + distanceZ < thisRb.transform.position.z)
            {
                z = -correctionSpeedZ;
            }
            else if (playerRb.transform.position.z + distanceZ > thisRb.transform.position.z)
            {
                z = correctionSpeedZ;
                if (!shootCoroutineRunning)
                {
                    canShoot = true;
                    StartCoroutine(ShootLoop());
                }
            }
            moveDir = new Vector3(moveDir.x, moveDir.y , z);
        }
        
        thisRb.MovePosition(thisRb.transform.position + moveDir * speed * Time.fixedDeltaTime);

        if (thisRb.transform.position.z < playerRb.transform.position.z)
        {
            OnCrashed();
            if (Vector3.Distance(thisRb.transform.position,playerRb.transform.position) < explosionRadius)
            {
                GM.instance.playerExplosionHit();
                OnCrashed();
            }
        }
        
    }

    void Shoot()
    {
        Vector3 pos = transform.position + transform.forward;
        Transform bulletT = Instantiate(projectilePrefab, pos, transform.rotation);
        bulletT.gameObject.GetComponent<ProjectileScript>().isPlayerProjectile = false;
    }
}
