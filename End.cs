using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End : MonoBehaviour {

    public Text endScore;

    void OnEnable()
    {
        endScore.text = "Final Score: " + PlayerPrefs.GetInt("score");
    }
}
