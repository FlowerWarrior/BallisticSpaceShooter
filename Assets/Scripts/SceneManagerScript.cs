using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
        Time.timeScale = 1.0f;
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadScene(string name)
    {
        if (name != null)
        {
            SceneManager.LoadScene(name);
        }
    }
}
