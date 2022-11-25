using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Camera Information
    [SerializeField] Transform m_Transform;
    private Quaternion orignalPos;

    // Shake Parameters
    [SerializeField] float shakeDuration = 2f;
    [SerializeField] float shakeAmount = 0.7f;

    private bool canShake = false;
    private float _shakeTimer;
    [SerializeField] float shakeCount = 3F;
    [SerializeField] float shakeSpeed = 1F;
    Quaternion targetPos;
    bool isShaking = false;

    // Start is called before the first frame update
    void Start()
    {
        orignalPos = m_Transform.localRotation;
        targetPos = Quaternion.Euler(orignalPos.eulerAngles + Random.insideUnitSphere * shakeAmount);
    }

    IEnumerator ShakePosUpdater()
    {
        for (int i=0; i<shakeCount; i++)
        {
            targetPos = Quaternion.Euler(orignalPos.eulerAngles + Random.insideUnitSphere * shakeAmount);
            yield return new WaitForSeconds(shakeDuration / shakeCount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canShake)
        {
            StartCameraShakeEffect();
        }
        else
        {
            m_Transform.localRotation = Quaternion.Lerp(m_Transform.localRotation, orignalPos, Time.deltaTime * shakeSpeed);
        }
    }

    public void ShakeCamera()
    {
        StartCoroutine(ShakePosUpdater());
        canShake = true;
        isShaking = true;
        _shakeTimer = shakeDuration;
    }

    public void StartCameraShakeEffect()
    {
        if (_shakeTimer > 0)
        {
            m_Transform.localRotation = Quaternion.Lerp(m_Transform.localRotation, targetPos, Time.deltaTime * shakeSpeed);
            _shakeTimer -= Time.deltaTime;
        }
        else
        {
            _shakeTimer = 0f;
            canShake = false;
            isShaking = false;
        }
    }
}
