using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIControl : MonoBehaviour
{
    public GameObject selectMenu;
    public GameObject mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSelectMenu()
    {
        selectMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void OpenMainMenu()
    {
        selectMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenGame(int i)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game"+i);
    }
}
