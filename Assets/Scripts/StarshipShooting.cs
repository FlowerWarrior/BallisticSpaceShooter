using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarshipShooting : MonoBehaviour
{
    // Skrypt podczepiamy pod statek
    // Odpowiada on za wystrzelenie pocisku
    [SerializeField] Transform projectilePrefab;
    [HideInInspector] float shootFrequency = 4F;
    [SerializeField] float bulletSpeed = 1f;
    [SerializeField] Transform shootPosition;
    private int baseMagazineVal;
    bool canShoot = true;
    Rigidbody playerRb;

    UIMG m_UIMG;
    Vector3 targetPos;
    bool hasTarget = false;
    Transform target;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        m_UIMG = UIMG.instance;
        shootFrequency = PlayerPrefs.GetInt("firerate", 4);
    }
    void Update()
    {
        // Mouse targeting
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer_mask = LayerMask.GetMask("TargetingLayer");
        int distance = 160;

        m_UIMG.HideTargetingIndicator();
        if (Physics.Raycast(ray, out hit, distance, layer_mask)) 
        {
            if (hit.transform.gameObject.tag == "Asteroid")
            {
                target = hit.transform.gameObject.transform;
                hasTarget = true;
                targetPos = hit.transform.gameObject.transform.position;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(hit.transform.gameObject.transform.position);
                m_UIMG.UpdateTargetingIndicator(screenPos, targetPos, 0);
            }
            if (hit.transform.gameObject.tag == "Enemy")
            {
                target = hit.transform.gameObject.transform;
                hasTarget = true;
                targetPos = hit.transform.gameObject.transform.position;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(hit.transform.gameObject.transform.position);
                m_UIMG.UpdateTargetingIndicator(screenPos, targetPos, 1);
            }
            if (hit.transform.gameObject.tag == "Boss")
            {
                if (hit.transform.gameObject.GetComponent<BossController>().isTargetable)
                {
                    target = hit.transform.gameObject.transform;
                    hasTarget = true;
                    targetPos = hit.transform.gameObject.transform.position;
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(hit.transform.gameObject.transform.position);
                    m_UIMG.UpdateTargetingIndicator(screenPos, targetPos, 2);
                }
            }
        }
        else
        {
            target = null;
            hasTarget = false;
            //targetPos = new Vector3(0, 0, 10) + transform.position;
        }

        Shoot();
    }
    
    void Shoot()
    {
        if (Input.GetButton("Fire1")){
            if (canShoot)
            {
                SpawnBullet();
                StartCoroutine(ShootDelay());
            }
        }
    }

    void SpawnBullet()
    {
        if (target == null)
        {
            // Mouse targeting
            RaycastHit hit2;
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layer_mask2 = LayerMask.GetMask("BigRaycastPlane");
            int distance2 = 223;

            if (Physics.Raycast(ray2, out hit2, distance2, layer_mask2)) 
            {
                targetPos = hit2.point;
            }
        }

        Quaternion bulletRot = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
        Debug.Log(bulletRot);
        if (!hasTarget)
        {
           bulletRot = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
        }
        
        Transform bulletT = Instantiate(projectilePrefab, shootPosition.position, bulletRot);
        bulletT.gameObject.GetComponent<ProjectileScript>().moveSpeed = bulletSpeed + (playerRb.velocity.z / playerRb.drag);
        bulletT.gameObject.GetComponent<ProjectileScript>().target = target;
        bulletT.gameObject.GetComponent<ProjectileScript>().hasTarget = hasTarget;
    }
    
    private IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(1 / shootFrequency);
        canShoot = true;
    }
}
