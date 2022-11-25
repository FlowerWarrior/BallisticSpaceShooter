using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPart : MonoBehaviour
{
    void OnBecameInvisible() 
    {
        transform.parent.gameObject.GetComponent<ExplodeAsteroid>().PartExitedScreen();
    }
}
