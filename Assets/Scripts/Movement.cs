using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] float thrust = 7f;
    [SerializeField] float sideThrust = 7F;
    [SerializeField] float rotMaxAngleHorizontal = 30F;
    [SerializeField] float rotMaxAngleVertical = 30F;
    [SerializeField] float rotSpeed = 4F;
    [SerializeField] float minX = -8F;
    [SerializeField] float maxX = 8F;
    [SerializeField] float minY = 5F;
    [SerializeField] float maxY = 13F;
    [SerializeField] float boundsForce = 10F;
    [SerializeField] float boostMax = 100F;
    [SerializeField] float boostUsageSpeed = 10F;
    [HideInInspector] public float boostCurrent = 100F;
    [SerializeField] ParticleSystem[] engineParticles;
    [SerializeField] AudioClip audioAccelerate;
    [SerializeField] AudioClip audioDecelerate;
    [SerializeField] AudioSource boostAudioSource;
    Rigidbody rb;
    Vector3 direction;
    Quaternion targetRot;
    float initialThrust;
    [HideInInspector] public bool isBoosting = false;
    float initialFOV;
    float targetFOV;
    [SerializeField] float smootnessFOV = 1F;
    Camera cam;
    bool isDoingFlip = false;
    float flipAngleDone = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialThrust = thrust;
        initialFOV = Camera.main.fieldOfView;
        boostCurrent = boostMax;
        cam = Camera.main.GetComponent<Camera>();
        boostUsageSpeed = 10 - PlayerPrefs.GetInt("moreboost", 0) * 0.4F;
    }

    void Update()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"), 0f);

        targetRot = Quaternion.Euler(rotMaxAngleVertical * Input.GetAxis("Vertical"), rb.transform.rotation.y, -rotMaxAngleHorizontal * Input.GetAxis("Horizontal"));
        
        if(Input.GetButtonDown("Boost") && boostCurrent > 0 && !isBoosting)
        {
            EnableBoost();
        }
        
        if(Input.GetButtonUp("Boost") && isBoosting)
        {
            DisableBoost();
        }

        if (isBoosting && boostCurrent > 0)
        {
            boostCurrent -= boostUsageSpeed * Time.deltaTime;
            UIMG.instance.UpdateBoostBar(boostCurrent / boostMax);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, smootnessFOV * Time.deltaTime);
        }
        if (isBoosting && boostCurrent <= 0)
        {
            DisableBoost();
        }
    }
    
    public void AddBoost(float x)
    {
        boostCurrent += 50;
        boostCurrent = Mathf.Clamp(boostCurrent, 0, 100);
        UIMG.instance.UpdateBoostBar(boostCurrent / boostMax);
    }

    void EnableBoost()
    {
        thrust *= 2F;
        isBoosting = true;
        targetFOV = initialFOV + 5F;
        for (int i = 0; i < engineParticles.Length; i++)
        {
            engineParticles[i].Play();
        }
        AudioSource.PlayClipAtPoint(audioAccelerate, transform.position);
        boostAudioSource.Play();
    }

    void DisableBoost()
    {
        thrust = initialThrust;
        isBoosting = false;
        targetFOV = initialFOV;
        for (int i = 0; i < engineParticles.Length; i++)
        {
            engineParticles[i].Stop();
        }
        AudioSource.PlayClipAtPoint(audioDecelerate, transform.position);
        boostAudioSource.Stop();
    }

    void FixedUpdate() 
    {

        // TEST
        if (Input.GetKeyDown("space"))
        {
            isDoingFlip = true;
            flipAngleDone = 0;
        }
        Debug.Log("Test code here!");

        // Forward movement
        rb.AddForce(Vector3.forward * thrust, ForceMode.Force);

        if (!isDoingFlip)
        {
            // Side movement
            rb.MoveRotation(Quaternion.Lerp(rb.transform.rotation, targetRot, Time.fixedDeltaTime * rotSpeed));
            rb.AddForce(direction * sideThrust, ForceMode.Force);
        }
        else
        {
            float flipSpeed = rotSpeed;
            
            if (flipAngleDone < 358)
            {
                flipAngleDone += flipSpeed * Time.deltaTime;
                Quaternion flipRotTarget = Quaternion.Euler(0, 0, 360);
                rb.MoveRotation(Quaternion.Lerp(rb.transform.rotation, flipRotTarget, Time.fixedDeltaTime * flipSpeed));
                rb.AddForce(new Vector3(1, 0, 0) * sideThrust, ForceMode.Force);
            }
            else
            {
                flipAngleDone = 0;
                isDoingFlip = false;
            }
        }

        // Bounds X
        Vector3 normalizingVector = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z);
        if (rb.transform.position.x > maxX)
        {
            float x = Mathf.Lerp(rb.transform.position.x, maxX, Time.fixedDeltaTime * boundsForce);
            normalizingVector = new Vector3(x, normalizingVector.y, normalizingVector.z);
            //targetRot = Quaternion.Euler(Mathf.Clamp(targetRot.x, 0, -1), targetRot.y, targetRot.z);
        }
        if (rb.transform.position.x < minX)
        {
            float x = Mathf.Lerp(rb.transform.position.x, minX, Time.fixedDeltaTime * boundsForce);
            normalizingVector = new Vector3(x, normalizingVector.y, normalizingVector.z);
            //targetRot = Quaternion.Euler(Mathf.Clamp(targetRot.x, 0, 1), targetRot.y, targetRot.z);
        }
        // Bounds Y
        if (rb.transform.position.y > maxY)
        {
            float y = Mathf.Lerp(rb.transform.position.y, maxY, Time.fixedDeltaTime * boundsForce);
            normalizingVector = new Vector3(normalizingVector.x, y, normalizingVector.z);
            //targetRot = Quaternion.Euler(Mathf.Clamp(targetRot.x, 0, 1), targetRot.y, targetRot.z);
        }
        if (rb.transform.position.y < minY)
        {
            float y = Mathf.Lerp(rb.transform.position.y, minY, Time.fixedDeltaTime * boundsForce);
            normalizingVector = new Vector3(normalizingVector.x, y, normalizingVector.z);
            //targetRot = Quaternion.Euler(Mathf.Clamp(targetRot.x, 0, 1), targetRot.y, targetRot.z);
        }
        rb.MovePosition(normalizingVector);
    }
}
