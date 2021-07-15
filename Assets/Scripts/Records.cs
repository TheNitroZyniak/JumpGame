using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Records : MonoBehaviour{
    int[] bestResults = new int[5];

    public Text[] results;
    void Start(){
        bestResults[0] = PlayerPrefs.GetInt("First");
        bestResults[1] = PlayerPrefs.GetInt("Second");
        bestResults[2] = PlayerPrefs.GetInt("Third");
        bestResults[3] = PlayerPrefs.GetInt("Fourth");
        bestResults[4] = PlayerPrefs.GetInt("Fifth");

        for(int i = 0; i < bestResults.Length; i++) {
            if (bestResults[i] != 0)
                results[i].text = i + 1 + " - " + bestResults[i].ToString();
            else
                results[i].text = i + 1 + " - ...";

        }
    }


    public void Back() {
        SceneManager.LoadScene("Menu");
    }
}
