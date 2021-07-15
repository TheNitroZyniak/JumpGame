using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class Player : MonoBehaviour {
    public bool isContact, changeHorVelocity, flyToRight;
    public bool bubble;
    public PhysicsMaterial2D pM2D;
  
    public Collider2D platformCollider;
    private Rigidbody2D rigitBody2D;
    private SpriteRenderer _renderer;
    float camWidth, camHalfWidth;
    public Camera _camera;
    public Image forground;

    public static bool collisionMade, death;

    public static int jumpingType, endScore;
    public Levels levels;

    public AudioClip[] clips;

    void Start() {
        endScore = 0;
        done = false;
        jumpingType = 0;
        death = false;
        pM2D.bounciness = 0;
        GetComponent<Collider2D>().sharedMaterial = pM2D;
        rigitBody2D = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float camHalfHeight = Camera.main.orthographicSize;
        camHalfWidth = screenAspect * camHalfHeight;
    }

    float accX;

    private void Update() {
        if (jumpingType == 1) {
            accX = Input.acceleration.x * 12;
            if (Input.GetKey(KeyCode.D)) accX = 2;
            if (Input.GetKey(KeyCode.A)) accX = -2;
            //if (!changeHorVelocity) 
                rigitBody2D.velocity = new Vector2(accX, rigitBody2D.velocity.y);
        }
        if (transform.position.x > 2.75f) transform.position = new Vector3(-2.75f, transform.position.y, -1);
        if (transform.position.x < -2.75f) transform.position = new Vector3(2.75f, transform.position.y, -1);

        if (rigitBody2D.velocity.y > 0) platformCollider.isTrigger = true;
        else platformCollider.isTrigger = false;

        if (transform.position.y < _camera.transform.position.y - 8) {
            //if (!done && Advertisement.IsReady(AdManager.rewardedVideoAd)) {
            if (!done) {               
                death = true;
                transform.position = new Vector3(0, _camera.transform.position.y - 8, transform.position.z);
                rigitBody2D.gravityScale = 0;
                rigitBody2D.velocity = new Vector2(0, 0);
                levels.RewardVideo();
                done = true;
                
            } 
            else {
                SetPrefs((int)(_camera.transform.position.y * 10));
                endScore = (int)(_camera.transform.position.y * 10);
                SceneManager.LoadScene("Menu");
            }
        }

    }
    public static bool done = false;



    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "CentralPlatform" || (collision.gameObject.tag == "JumpPlatform")) {
            if (!death) {
                ContactPoint2D contact = collision.contacts[0];
                isContact = true;
                changeHorVelocity = true;
                //rigitBody2D.angularVelocity = -contact.normal.x * 1000;
                //rigitBody2D.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);

                float angle = Vector2.Angle(contact.normal, new Vector2(5, 0));

                StartCoroutine(Push(Quaternion.Euler(0, 0, angle - 90), contact));

                GetComponent<Collider2D>().isTrigger = true;

                //transform.rotation = Quaternion.Euler(0, 0, angle - 90);
                //if (Input.acceleration.x > 0) flyToRight = true;
                //else flyToRight = false;

                collisionMade = true;

                rigitBody2D.angularVelocity = 0;

                if (collision.gameObject.tag == "JumpPlatform") jumpingType = 1;
            }
        } 
        else {
            death = true;
            SetPrefs((int)(_camera.transform.position.y * 10));
            StartCoroutine(Death());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Hole") {
            StartCoroutine(Pull(collision.gameObject.transform));
        }
    }


    private void OnTriggerExit2D(Collider2D collision) {
        if (!death) {
            if (collision.gameObject.name == "Bullet(Clone)") collision.gameObject.GetComponent<Collider2D>().isTrigger = false;

            if (collision.gameObject.name == "CentralPlatform" || collision.gameObject.tag == "JumpPlatform") {
                GetComponent<Collider2D>().isTrigger = false;
            }
        }
    }


    IEnumerator Death() {
        float a = 0.5f;
        GetComponent<Collider2D>().isTrigger = true;
        rigitBody2D.velocity = new Vector2(0, 6);
        rigitBody2D.angularVelocity = 120;
        forground.color = new Color(0, 0, 0, 0.5f);
        while (a > 0) {
            forground.color = new Color(0,0,0, a);
            a -= 0.01f;
            yield return new WaitForFixedUpdate();
        }
        forground.color = new Color(0, 0, 0, 0);
        
    }

    IEnumerator Push(Quaternion endRotation, ContactPoint2D contact) {
        float a = 0;
        while (a <= 1) {
            rigitBody2D.velocity = new Vector2(0, 0);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), endRotation, a);
            a += 0.25f;
            yield return new WaitForFixedUpdate();
        }

        GetComponent<AudioSource>().clip = clips[Random.Range(0,2)];
        GetComponent<AudioSource>().Play();
        rigitBody2D.velocity = new Vector2(contact.normal.x * 9, 7);
        if (rigitBody2D.velocity.x >= 0) _renderer.flipX = false;
        else _renderer.flipX = true;
        StartCoroutine(Fly(endRotation));

    }


    IEnumerator Fly(Quaternion startRotation) {
        float a = 0;
        changeHorVelocity = true;
        while (a <= 1 && !death) {
            accX = Input.acceleration.x * 12;
            Vector2 endVel = new Vector2(accX, rigitBody2D.velocity.y);

            //rigitBody2D.velocity = Vector2.Lerp(rigitBody2D.velocity, endVel, a);
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(0, 0, 0), a);

            a += 0.025f;
            yield return new WaitForFixedUpdate();
        }
        changeHorVelocity = false;
    }

    IEnumerator Pull(Transform hole) {
        float a = 0;
        rigitBody2D.velocity = new Vector2(0, 0);
        rigitBody2D.gravityScale = 0;
        
        while(a <= 1) {
            transform.position = Vector3.Lerp(transform.position, hole.position, a);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0,0,1), a);
            _renderer.color = new Color(1,1,1, 1-a);

            a += 0.04f;
            yield return new WaitForFixedUpdate();
        }

        transform.position = new Vector3(0, _camera.transform.position.y - 8, transform.position.z);
        _renderer.color = new Color(1, 1, 1, 0);
        death = true;


        //if(!done && Advertisement.IsReady(AdManager.rewardedVideoAd)) levels.RewardVideo();
        if (!done) levels.RewardVideo();
        else {
            SetPrefs((int)(_camera.transform.position.y * 10));
            endScore = (int)(_camera.transform.position.y * 10);
            SceneManager.LoadScene("Menu");      
        }
        done = true;
    }  


    void SetPrefs(int newResult) {
        int[] bestResults = new int[5];
        bestResults[0] = PlayerPrefs.GetInt("First");
        bestResults[1] = PlayerPrefs.GetInt("Second");
        bestResults[2] = PlayerPrefs.GetInt("Third");
        bestResults[3] = PlayerPrefs.GetInt("Fourth");
        bestResults[4] = PlayerPrefs.GetInt("Fifth");

        for(int i = 0; i < bestResults.Length; i++) {
            if(newResult > bestResults[i]) {
                for(int j = 0; j < 4-i; j++) {
                    bestResults[4-j] = bestResults[4-(j + 1)];
                }
                bestResults[i] = newResult;
                break;
            }
        }

        PlayerPrefs.SetInt("First", bestResults[0]);
        PlayerPrefs.SetInt("Second", bestResults[1]);
        PlayerPrefs.SetInt("Third", bestResults[2]);
        PlayerPrefs.SetInt("Fourth", bestResults[3]);
        PlayerPrefs.SetInt("Fifth", bestResults[4]);
    }

    public void Cancel() {
        SetPrefs((int)(_camera.transform.position.y * 10));
        endScore = (int)(_camera.transform.position.y * 10);
        SceneManager.LoadScene("Menu");
    }
}