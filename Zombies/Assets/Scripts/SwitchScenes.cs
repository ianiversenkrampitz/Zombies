using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Iversen-Krampitz, Ian 
//12/7/2023
//Controls the start and end screens. 

public class SwitchScenes : MonoBehaviour
{
    public GameObject quit;
    public GameObject start;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit game.");
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
