using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseUI;
    private void Awake()
    {
        pauseUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("escaped");
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
