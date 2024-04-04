using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject successMenu;
    public GameObject failMenu;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void OpenSuccessMenu()
    {
        pauseMenu.SetActive(false);
        failMenu.SetActive(false);
        successMenu.SetActive(true);
    }

    public void OpenFailMenu()
    {
        pauseMenu.SetActive(false);
        successMenu.SetActive(false);
        failMenu.SetActive(true);
    }

    public void OpenPauseMenu()
    {
        successMenu.SetActive(false);
        failMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void LoadNextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        Debug.Log("Restart");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
