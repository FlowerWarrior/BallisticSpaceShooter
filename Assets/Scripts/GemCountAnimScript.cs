using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCountAnimScript : MonoBehaviour
{
    Vector3 initialScale;
    Vector3 targetScale = new Vector3(1.5F, 1.5F, 1.5F);
    Vector3 targetRot = new Vector3(0, 0, -30);
    [SerializeField] Transform parentT;

    [SerializeField] TMPro.TextMeshProUGUI m_Text;
    float speed = 6F;

    // Start is called before the first frame update
    void Awake()
    {
        //m_Text = GetComponent<TMPro.TextMeshProUGUI>(); 
        initialScale = parentT.localScale;
    }

    // Update is called once per frame
    void Update()
    {
       parentT.localScale = Vector3.Lerp(parentT.localScale, initialScale, Time.deltaTime * speed);       
    }

    public void UpdateGemText(string text)
    {
        parentT.localScale = targetScale;
        m_Text.text = text;
    } 
}
