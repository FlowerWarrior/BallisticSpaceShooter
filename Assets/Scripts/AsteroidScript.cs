using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    public int hp = 2;
    public float moveSpeed = 1F;
    [SerializeField] float rotSpeed = 1F;
    [SerializeField] float cutOffDistance = 14F; // when part is gone from screen
    [HideInInspector] public Transform playerT = null;
    Rigidbody rb;
    Vector3 rotVector;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.inertiaTensor = new Vector3(1, 1, 1); // Fix after targeting collider inertia

        float varRot = Random.Range(-1F, 1F);
        rotVector = new Vector3(varRot, varRot, varRot) * rotSpeed;
    }
    void FixedUpdate()
    {
        // Idle rotation
        rb.AddTorque(rotVector, ForceMode.Force);

        // Move asteroid
        rb.AddForce(new Vector3(0, 0, -moveSpeed), ForceMode.Force);

        if (playerT != null)
        {
            if (transform.position.z < playerT.position.z - cutOffDistance)
            {
                GM.instance.combo = 0;
                GM.instance.UpdateCombo();
                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.position.z < -cutOffDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}

