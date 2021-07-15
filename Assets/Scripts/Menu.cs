using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour{

    public PhysicsMaterial2D pM2D;
    public GameObject player;

    public Text gameName, score;

    private void Start() {
        pM2D.bounciness = 1;
        player.GetComponent<Collider2D>().sharedMaterial = pM2D;
        if (Player.endScore != 0) {
            gameName.text = "Game over";
            score.text = Player.endScore.ToString();
        } 
        else {
            gameName.text = "Jump game";
            score.text = "";
        }
        Player.endScore = 0;


    }

    public void Play() {
        SceneManager.LoadScene("Game");
    }
    public void Records() {
        SceneManager.LoadScene("Records");
    }
}
