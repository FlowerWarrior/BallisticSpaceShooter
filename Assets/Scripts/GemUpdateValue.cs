using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemUpdateValue : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI gemText;

    public static GemUpdateValue instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGemCount();
    }

    public void UpdateGemCount()
    {
        gemText.text = PlayerPrefs.GetInt("gems", 0).ToString();
    }
}
