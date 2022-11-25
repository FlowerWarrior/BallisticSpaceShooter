using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboAnimScript : MonoBehaviour
{
    Vector3 initialScale;
    Quaternion initialRot;
    Vector3 targetScale = new Vector3(1.2F, 1.2F, 1.2F);
    Vector3 targetRot = new Vector3(0, 0, -30);
    
    [SerializeField] TMPro.TextMeshProUGUI m_Text;
    float speed = 6F;

    // Start is called before the first frame update
    void Start()
    {
        //m_Text = GetComponent<TMPro.TextMeshProUGUI>(); 
        initialScale = transform.localScale;
        initialRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
       transform.localScale = Vector3.Lerp(transform.localScale, initialScale, Time.deltaTime * speed);       
    }

    public void UpdatedCombo()
    {
        transform.localScale = targetScale;
    }   

    public void LostCombo()
    {
        m_Text.text = "x0";
    }
}
