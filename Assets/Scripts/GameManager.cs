using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GameManager : MonoBehaviour{

    public GameObject player, bulletPrefab, forUpText, bestScore;
    public GameObject platform, textObject, gameName;   

    public GameObject[] platformSides;

    public Camera _camera;
    public AudioClip[] audioClips;

    public Text scoreText;
    int currentLevel;

    bool isPlatformMoving;

    //private FirebaseApp app;

    public Sprite[] forms;

    public Transform[] background;
    int last = 1;
    public static int levelHeight = 6;
    Vector3 vr1, vr2;

    public Levels levels;

    private void Start() {
        levelHeight = 6;
        currentLevel = 1;
        levelHeight = levels.CreateLevel(levelHeight);
    }

    private void Update() {

        if (player.transform.position.y >= _camera.transform.position.y - 2f && player.GetComponent<Rigidbody2D>().velocity.y >= 0 && !isPlatformMoving && !Player.death) {
            _camera.transform.position = new Vector3(0, player.transform.position.y + 2f, -10);
        }

        GetComponent<Collider2D>().transform.position = new Vector3(0, _camera.transform.position.y, -2);

        if (player.GetComponent<Player>().isContact) {
            isPlatformMoving = false;
            player.GetComponent<Player>().isContact = false;
        }

        scoreText.text = ((int)(_camera.transform.position.y * 10)).ToString();

        if (_camera.transform.position.y > background[last].position.y) {
            if (last == 0) last = 1;
            else last = 0;
            background[last].position = new Vector3(0, background[last].position.y + 20.8f, background[last].position.z);
        }

        if(_camera.transform.position.y + 6 > levelHeight) {
            levelHeight = levels.CreateLevel(levelHeight);
        }
    }
    
    private void OnMouseDown() {
        
        if (!Player.death) {
            var screenPoint = (Input.mousePosition);
            screenPoint.z = 10.0f;

            vr1 = Camera.main.ScreenToWorldPoint(screenPoint);

            if (player.GetComponent<Rigidbody2D>().gravityScale != 0) {

                if (vr1.y > player.transform.position.y - 0.3f) {
                    GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.identity);

                    vr1 = levels.ShootingToEnemy(player.transform).position;

                    Vector2 vel = vr1 - player.transform.position;
                    bullet.GetComponent<Rigidbody2D>().velocity = vel.normalized * 20;
                    
                    bullet.GetComponent<AudioSource>().clip = audioClips[Random.Range(0, 2)];
                    bullet.GetComponent<AudioSource>().Play();
                    Destroy(bullet, 5);
                } 
            }          
        }  
        
    }

    private void OnMouseOver() {
        if (player.transform.position.y >= _camera.transform.position.y - 2f && player.GetComponent<Rigidbody2D>().velocity.y >= 0) {
            _camera.transform.position = new Vector3(0, player.transform.position.y + 2f, -10);
        }
        
    } 
}