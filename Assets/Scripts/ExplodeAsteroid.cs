using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAsteroid : MonoBehaviour
{
    [SerializeField] Rigidbody[] partsRb;
    [SerializeField] float explosionPower = 3F;
    [SerializeField] float explosionRadius = 3F;
    [SerializeField] Renderer[] renderers;
    public Vector3 explosionPos;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < partsRb.Length; i++)
        {
            partsRb[i].AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);
        }
    }

    public void PartExitedScreen()
    {
        bool areVisible = false;
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].isVisible)
            {
                areVisible = true;
            }
        }

        Debug.Log("exited screen");

        if (!areVisible)
        {
            Destroy(gameObject);
        }
    }
}
