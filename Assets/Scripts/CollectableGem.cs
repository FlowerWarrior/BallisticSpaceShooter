using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableGem : MonoBehaviour
{
    [HideInInspector] public Transform playerT;
    [SerializeField] Transform gemCollectedParticles;
    [SerializeField] AudioClip audioGem;
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
            Instantiate(gemCollectedParticles, transform.position, transform.rotation);
            AudioSource.PlayClipAtPoint(audioGem, transform.position);

            GM.instance.PlayerCollectedGem();
            Destroy(gameObject);
        }
    }
}
