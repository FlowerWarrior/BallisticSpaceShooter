using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [SerializeField] public float bossHP = 100F;
    float initialHP;
    [SerializeField] Transform bulletPrefab;
    [SerializeField] Transform meshT;
    [SerializeField] Transform laserForm1T;
    [SerializeField] Slider healthbar;
    [SerializeField] float attackTime = 20F;
    [SerializeField] float laserRotSpeed = 30F;
    [SerializeField] float rotSpeed = 60F;
    [SerializeField] float adjustZSpeed = 12F;
    [SerializeField] float playerDistanceZ = 50F;
    [SerializeField] float moveSpeed = 4F;
    [SerializeField] float hoverY = 14F;
    [SerializeField] AudioSource m_AudioSource;
    [HideInInspector] public Rigidbody playerRb;
    [HideInInspector] public bool isTargetable = false;
    Rigidbody thisRb;
    int currentPhase = 0;
    Vector3 randomPos;
    bool isPlayingAudio = false;

    // Start is called before the first frame update
    void Start()
    {
        thisRb = GetComponent<Rigidbody>();
        bossHP = bossHP - PlayerPrefs.GetInt("easierboss", 0) * 5;
        initialHP = bossHP;
        StartCoroutine(DisableLasersAfter());
    }

    public void UpdateHealthBar()
    {
        healthbar.value = bossHP / initialHP;
    }

    IEnumerator DisableLasersAfter()
    {
        isTargetable = false;
        yield return new WaitForSeconds(attackTime);
        laserForm1T.gameObject.SetActive(false);
        playerDistanceZ += 24F;
        currentPhase = 1;
        m_AudioSource.Stop();
        StartCoroutine(RandomPositions());
    }

    IEnumerator DisableRandomMoveAfter()
    {
        yield return new WaitForSeconds(attackTime);
        isPlayingAudio = false;
        StopCoroutine(RandomPositions());
        laserForm1T.gameObject.SetActive(true);
        playerDistanceZ -= 24F;
        currentPhase = 0;
        StartCoroutine(DisableLasersAfter());
    }

    IEnumerator RandomPositions()
    {
        StartCoroutine(DisableRandomMoveAfter());
        isTargetable = true;
        while(true)
        {
            randomPos = new Vector3(Random.Range(-8F, 8F), Random.Range(7F, 13F), 0);
            yield return new WaitForSeconds(0.5F);
        }
    }

    void FixedUpdate()
    {
        // Idle mesh rotation
        meshT.transform.Rotate(new Vector3(0, -1, 0) * rotSpeed * Time.fixedDeltaTime, Space.World);

        // Laser rotation
        laserForm1T.transform.Rotate(new Vector3(0, 0, 1) * laserRotSpeed * Time.fixedDeltaTime, Space.World);

        // Center the boss
        Vector3 moveDir = new Vector3(0, hoverY, 0) - thisRb.transform.position;

        // Move to random positions
        if (currentPhase == 1)
        {
            moveDir = randomPos - thisRb.transform.position;
        }
        
        // Keep distance from player
        float z = 0;
        float targetZ = playerRb.transform.position.z + playerDistanceZ;

        if (targetZ < thisRb.transform.position.z)
        {
            z = -adjustZSpeed;
        }
        else if (targetZ > thisRb.transform.position.z)
        {
            z = adjustZSpeed;

            if(!isPlayingAudio)
            {
                laserForm1T.gameObject.SetActive(true);
                m_AudioSource.Play();
                isPlayingAudio = true;
            }
        }
        moveDir = new Vector3(moveDir.x, moveDir.y , z);
        
        thisRb.MovePosition(thisRb.transform.position + (moveDir * moveSpeed * Time.fixedDeltaTime) );
    }

    void ShootBullet()
    {
        Vector3 pos = transform.position;
        Transform bulletT = Instantiate(bulletPrefab, pos, Quaternion.Euler(0, 180, 0));
        bulletT.gameObject.GetComponent<ProjectileScript>().isPlayerProjectile = false;
    }
}
