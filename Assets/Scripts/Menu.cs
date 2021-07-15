using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour{

    public PhysicsMaterial2D pM2D;
    public GameObject player;
    private void Start() {
        pM2D.bounciness = 1;
        player.GetComponent<Collider2D>().sharedMaterial = pM2D;
    }

    public void Play() {
        SceneManager.LoadScene("Game");
    }
    public void Records() {
        SceneManager.LoadScene("Records");
    }
}
