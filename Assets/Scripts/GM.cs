using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    // GameManager
    // trzeba stworzy� empty object na mapie kt�ry b�dzie GMem i podpi�� ten skrypt tam
    // linijki tworz�ce singleton
    public static GM instance;
    StarshipLifeManager playerLifeMgr;
    Movement playerMovement;
    [SerializeField] Transform[] shipVariants;
    [SerializeField] Transform shipSpawnT;
    [SerializeField] CameraFollow m_CameraFollow;
    [SerializeField] TunnelManager m_TunnelManager;
    [SerializeField] float enemySpacing = 1000F;
    [SerializeField] AudioClip audioExplosion;
    [SerializeField] EnemiesManager m_EnemiesManager;
    [SerializeField] AsteroidManager m_AsteroidManager;
    [SerializeField] BossManager m_BossManager;
    [SerializeField] CollectablesManager m_CollectablesManager;
    [SerializeField] public int enemyDealDamage = 1;
    [SerializeField] public int asteroidDamage = 1;
    [SerializeField] int enemyDamage = 1;
    [SerializeField] Transform asteroidExplodePrefab;
    [SerializeField] Transform boss1ExplodeParticles;
    [HideInInspector] GameObject player;
    [SerializeField] float bossSpacing = 3500F;
    [SerializeField] Transform enemyExplosionParticles;
    [SerializeField] ParticleSystem boostCollParticlesUI;
    float lastBossZ = 0;
    float score = 0;
    bool canSpawnEnemies = true;
    [HideInInspector] public float scoreMultiplier = 1;
    public int combo = 0;
    float normalTunnelSpeed;
    float playerLastZ;
    float lastEnemyZ = 0;
    int enemySpawnedPlayerHP;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        int index = PlayerPrefs.GetInt("SelectedShip", 0);
        player = Instantiate(shipVariants[index], shipSpawnT.position, shipSpawnT.rotation).gameObject;

        playerLifeMgr = player.GetComponent<StarshipLifeManager>();
        m_EnemiesManager.playerRb = player.GetComponent<Rigidbody>();
        m_TunnelManager.playerRb = player.GetComponent<Rigidbody>();
        m_BossManager.playerRb = player.GetComponent<Rigidbody>();
        m_AsteroidManager.playerRb = player.GetComponent<Rigidbody>();
        m_CollectablesManager.playerRb = player.GetComponent<Rigidbody>();

        m_CameraFollow.target = player.transform;
        UIMG.instance.player = player.transform;
        
        playerLastZ = player.transform.position.z;
        playerMovement = player.GetComponent<Movement>();

        playerLifeMgr.asteroidDamage = asteroidDamage;
        playerLifeMgr.enemyDamage = enemyDamage;

        normalTunnelSpeed = m_TunnelManager.speed;

        UIMG.instance.UpdateMultiplier(scoreMultiplier);
    }

    public void PlayerCollectedBoost()
    {
        playerMovement.AddBoost(50F);
        boostCollParticlesUI.Play();
        Debug.Log("collected boost");
    }
    
    public void playerExplosionHit()
    {
        playerLifeMgr.TakeDamage(enemyDamage * 2);
        AudioSource.PlayClipAtPoint(audioExplosion, playerLifeMgr.gameObject.transform.position);
    }
    
    //Skrypt do niszczenia asteroid
    public void OnBulletHitAsteroid(Collider other, Vector3 bulletPos)
    {
        // Asteroid to skrypt kt�ry jest przyczepiony to prefabu asteroidu, skrypt asteroidy musi mie� w sobie: public int hp
        other.GetComponent<AsteroidScript>().hp -= 1;
        if (other.gameObject.GetComponent<AsteroidScript>().hp <= 0)
        {
            AudioSource.PlayClipAtPoint(audioExplosion, other.gameObject.transform.position);

            Transform explosionAsteroidT = Instantiate(asteroidExplodePrefab, other.gameObject.transform.position, other.gameObject.transform.rotation);
            explosionAsteroidT.GetComponent<ExplodeAsteroid>().explosionPos = bulletPos;
            Destroy(other.gameObject);
            
            scoreMultiplier += 0.1F;
            combo++;
            UpdateCombo();
            UIMG.instance.UpdateMultiplier(scoreMultiplier);
        }
    }

    public void bossTakeBulletDamage(GameObject boss)
    {
        boss.GetComponent<BossController>().bossHP -= 1;
        boss.GetComponent<BossController>().UpdateHealthBar();
        if(boss.GetComponent<BossController>().bossHP <= 0)
        {
            Destroy(boss.gameObject);
            AudioSource.PlayClipAtPoint(audioExplosion, boss.transform.position);
            Instantiate(boss1ExplodeParticles, boss.gameObject.transform.position, boss.gameObject.transform.rotation);

            UIMG.instance.AddBonus(10000, 3);
            if (enemySpawnedPlayerHP == GetPlayerHP())
            {
                UIMG.instance.AddBonus(6000, 1);
            }

            // Make game harder
            enemySpacing *= 0.8F;
            m_AsteroidManager.spawnRate += 1;
        }
    }

    public void PlayerCollectedGem()
    {
        int previousGemCount = PlayerPrefs.GetInt("gems", 0);
        PlayerPrefs.SetInt("gems", previousGemCount + 1);
        UIMG.instance.UpdateGemCount(PlayerPrefs.GetInt("gems", previousGemCount + 1));
    }

    public void PlayerDestroyedAsteroid(Transform asteroidT, Vector3 playerPos)
    {
        Transform explosionAsteroidT = Instantiate(asteroidExplodePrefab, asteroidT.position, asteroidT.rotation);
        AudioSource.PlayClipAtPoint(audioExplosion, asteroidT.position);
        explosionAsteroidT.GetComponent<ExplodeAsteroid>().explosionPos = playerPos;
        Destroy(asteroidT.gameObject);
    }

    public void UpdateCombo()
    {
        UIMG.instance.UpdateCombo(combo, (int) score);
    }

    public float getScore() {  
        return score;
    }

    public void AddToScore(int x)
    {
        score += x;
    }

    public int GetPlayerHP()
    {
        return playerLifeMgr.starshipHP;
    }

    void Update()
    {
        score += (player.transform.position.z - playerLastZ) * scoreMultiplier;
        playerLastZ = player.transform.position.z;
        UIMG.instance.UpdateScore(score);

        // playerT.position.z <-- meters travelled
        if (player.transform.position.z > lastEnemyZ && canSpawnEnemies)
        {
            lastEnemyZ += enemySpacing;
            m_EnemiesManager.SpawnEnemy(lastEnemyZ);
            enemySpawnedPlayerHP = GetPlayerHP();
        }

        if (player.transform.position.z > lastBossZ + bossSpacing)
        {
            canSpawnEnemies = false;
            if (m_EnemiesManager.gameObject.transform.childCount == 0 && m_BossManager.gameObject.transform.childCount == 0)
            {
                lastBossZ += bossSpacing;
                m_BossManager.SpawnBoss();
            }
        }
    }
    
    public void playerTakeBulletDamage()
    {
        playerLifeMgr.TakeDamage(enemyDealDamage);
    }
    public void OnBulletHitEnemy(Collider other)
    {
        other.GetComponent<EnemyController>().hp -= 1;
        if(other.GetComponent<EnemyController>().hp <= 0)
        {
            Destroy(other.gameObject);
            Instantiate(enemyExplosionParticles, other.gameObject.transform.position, other.gameObject.transform.rotation);
            AudioSource.PlayClipAtPoint(audioExplosion, other.gameObject.transform.position);
            UIMG.instance.AddBonus(800, 2);

            if (enemySpawnedPlayerHP == GetPlayerHP())
            {
                UIMG.instance.AddBonus(400, 1);
            }

            UIMG.instance.UpdateScore(score);
        }
    }
    public void AfterPlayerDied()
    {
        m_EnemiesManager.StopShooting();
        
        if ((int)score > PlayerPrefs.GetInt("highscore", 0))
        {
            PlayerPrefs.SetInt("highscore", (int)score);
        }
    }
}
