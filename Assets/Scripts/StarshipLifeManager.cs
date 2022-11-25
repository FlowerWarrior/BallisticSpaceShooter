using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarshipLifeManager : MonoBehaviour
{
    [HideInInspector] public int starshipHP = 10;
    [HideInInspector] public int asteroidDamage = 10;
    [HideInInspector] public int enemyDamage = 15;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] AudioClip audioPlayerDamaged;
    Movement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<Movement>();
        starshipHP = PlayerPrefs.GetInt("health", 10);
        UIMG.instance.InitiateHealthBar(starshipHP);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            if (!playerMovement.isBoosting)
            {
                TakeDamage(GM.instance.asteroidDamage);
                GM.instance.combo = 0;
            }
            else
            {
                GM.instance.combo += 1;
            }
            
            GM.instance.UpdateCombo();
            GM.instance.PlayerDestroyedAsteroid(collision.gameObject.transform, transform.position);
        }
        else if(collision.gameObject.tag == "Enemy")
        {
            TakeDamage(GM.instance.enemyDealDamage);
            collision.gameObject.GetComponent<EnemyController>().OnCrashed();
        }
    }

    public void TakeDamage(int dmg)
    {
        starshipHP -= dmg;
        UIMG.instance.UpdatePlayerHealth();
        AudioSource.PlayClipAtPoint(audioPlayerDamaged, transform.position);

        if (starshipHP <= 0)
        {
            OnPlayerDied();
        }
    }

    void OnPlayerDied()
    {
        GM.instance.AfterPlayerDied();
        UIMG.instance.hideInGameUI();
        Instantiate(deathParticles, transform.position, transform.rotation);
        gameObject.SetActive(false);
        UIMG.instance.showFinalScore();
    }
}
