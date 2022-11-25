using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = "Highscore: " + PlayerPrefs.GetInt("highscore").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
