using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject menu;
    public GameObject howTo;

	public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void ToHowTo()
    {
        menu.SetActive(false);
        howTo.SetActive(true);        
    }

    public void ToMenu()
    {
        howTo.SetActive(false);
        menu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
