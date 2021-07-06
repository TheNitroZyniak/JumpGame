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

    public Image image;
    //public Text text, rightSideText, tapToPlay, scoreAfterGame;

    float x0, y0, x1, y1, cam, deathTime, alpha;
    bool isPlatformMoving, gameActive;
    int i = 0, levelHeight, bestScoreInt;

    //Levels levels;
    //private FirebaseApp app;

    //private GameObject[] enemies = new GameObject[5];

    public Sprite[] forms;

    public Transform[] background;
    int last = 1;
    Vector3 vr1, vr2;




    private void Update() {
        //ForBall();

        //levels.LevelUpdate(5, mainHero, figures);

        if (player.transform.position.y >= _camera.transform.position.y - 2.8f && player.GetComponent<Rigidbody2D>().velocity.y >= 0 && !isPlatformMoving) {
            _camera.transform.position = new Vector3(0, player.transform.position.y + 2.8f, -10);

        }

        GetComponent<Collider2D>().transform.position = new Vector3(0, _camera.transform.position.y, -2);
        if (player.GetComponent<Player>().isContact) {
            isPlatformMoving = false;
            player.GetComponent<Player>().isContact = false;
        }

        //text.text = ((int)(_camera.transform.position.y * 10)).ToString();

        if (_camera.transform.position.y > background[last].position.y) {
            if (last == 0) last = 1;
            else last = 0;

            background[last].position = new Vector3(0, background[last].position.y + 20.8f, background[last].position.z);
        }
    }



    private void OnMouseDown() {

        if (!player.GetComponent<Player>().death) {
            var screenPoint = (Input.mousePosition);
            screenPoint.z = 10.0f;

            vr1 = Camera.main.ScreenToWorldPoint(screenPoint);
            x0 = vr1.x;
            y0 = vr1.y;
            cam = _camera.transform.position.y;
            if (player.GetComponent<Rigidbody2D>().gravityScale != 0) {

                if (y0 < player.transform.position.y - 0.3f) {
                    isPlatformMoving = true;
                    cam = _camera.transform.position.y;
                } else {
                    GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.identity);
                    Vector2 vel = vr1 - player.transform.position;

                    bullet.GetComponent<Rigidbody2D>().velocity = vel.normalized * 20;
                    Destroy(bullet, 5);
                }
            }
            
        }

    }




    float y01 = 0;


    private void OnMouseOver() {
        if (player.transform.position.y >= _camera.transform.position.y - 2.8f && player.GetComponent<Rigidbody2D>().velocity.y >= 0) {
            _camera.transform.position = new Vector3(0, player.transform.position.y + 2.8f, -10);
        }
        if (isPlatformMoving) {
            y01 = _camera.transform.position.y - cam;

            var screenPoint = (Input.mousePosition);
            screenPoint.z = 10.0f;
            Vector3 vr = Camera.main.ScreenToWorldPoint(screenPoint);
            x1 = vr.x;
            y1 = vr.y;

            platformSides[0].transform.position = new Vector3(x0, y0 + y01, -1);
            platformSides[1].transform.position = new Vector3(x1, y1, -1);

            platform.transform.position = new Vector3((x0 + x1) / 2f, (platformSides[1].transform.position.y + platformSides[0].transform.position.y) / 2f, -1);
            platform.transform.localScale = new Vector2(Mathf.Sqrt((x1 - x0) * (x1 - x0) + ((platformSides[1].transform.position.y - platformSides[0].transform.position.y)) * ((platformSides[1].transform.position.y - platformSides[0].transform.position.y))), 0.2f);

            float angle;
            if (x1 - x0 != 0) angle = Mathf.Atan((platformSides[1].transform.position.y - platformSides[0].transform.position.y) / (x1 - x0)) * Mathf.Rad2Deg;
            else angle = 90;

            platform.transform.localEulerAngles = new Vector3(0, 0, angle);
            if (x0 < x1) {
                platformSides[0].transform.localEulerAngles = new Vector3(0, 0, angle);
                platformSides[1].transform.localEulerAngles = new Vector3(0, 0, angle);
            } else {
                platformSides[0].transform.localEulerAngles = new Vector3(0, 0, 180 + angle);
                platformSides[1].transform.localEulerAngles = new Vector3(0, 0, 180 + angle);
            }

            if (y1 > player.transform.position.y) isPlatformMoving = false;
        }
    }
    private void OnMouseUp() {

        vr1 = new Vector3(vr1.x, vr1.y + (_camera.transform.position.y - cam));

        var screenPoint = (Input.mousePosition);
        screenPoint.z = 10.0f;
        vr2 = Camera.main.ScreenToWorldPoint(screenPoint);
        float dist = Vector3.Distance(vr1, vr2);

        if (dist < 0.1f) {
            

            //platform.transform.position = platformSides[0].transform.position = platformSides[1].transform.position = new Vector3(10, 10, platform.transform.position.z);
        }

        isPlatformMoving = false;
    }
}
