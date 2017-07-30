using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private int score = 0;

    public Text scoreText;

    void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        scoreText.text = "Score: " + score;
    }

    public void AddPoint()
    {
        score++;
        PlayerPrefs.SetInt("score", score);
    }
}
