using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Lv_01", LoadSceneMode.Single);

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }
    public void WinScreen()
    {
        SceneManager.LoadScene("Win", LoadSceneMode.Single);
    }
    
    
    
    public void Quit()
    {
        Application.Quit();
        Debug.Log("application quit");

    }
}
