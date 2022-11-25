using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damageScreenAnim : MonoBehaviour
{
    [SerializeField] byte visibility = 80;
    [SerializeField] float speed = 1F;
    Image thisImg;

    // Start is called before the first frame update
    void Start()
    {
        thisImg = GetComponent<Image>();
    }

    public void ShowEffect()
    {
        Color32 col = thisImg.color;
        col.a = visibility;
        thisImg.color = col;
    }

    // Update is called once per frame
    void Update()
    {
        Color32 col = thisImg.color;
        col.a = System.Convert.ToByte(Mathf.Lerp(System.Convert.ToSingle(thisImg.color.a), 0F, Time.deltaTime * speed) * 255);
        thisImg.color = col;
    }
}
