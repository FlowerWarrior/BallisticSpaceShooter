    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthUIAnim : MonoBehaviour
{
    Vector3 initialPos;
    [SerializeField] Vector3 deltaPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, initialPos, 6F * Time.deltaTime);
    }

    public void AnimateDamageTaken()
    {
        transform.position = initialPos - deltaPos;
    }
}
