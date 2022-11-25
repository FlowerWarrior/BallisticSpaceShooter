using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBoost : MonoBehaviour
{
    [HideInInspector] public Transform playerT;
    [SerializeField] Transform boostCollectedParticles;

    [SerializeField] AudioClip audioBoost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < playerT.position.z - 16F)
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(audioBoost, transform.position);
            Instantiate(boostCollectedParticles, transform.position, transform.rotation);
            GM.instance.PlayerCollectedBoost();
            Destroy(gameObject);
        }
    }
}
