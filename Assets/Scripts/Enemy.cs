using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{
    public Vector3 startPosition;
    private Rigidbody2D _rigidbody2D;
    bool isDead;
    public Sprite[] sprites;


    private void Start() {
        //startPosition = transform.position;
        //startPosition = new Vector3(-1.5f, -10, transform.position.z);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = new Vector2(1,0);

        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, 1)];
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Bullet") {
            transform.position = new Vector3(10, 10, transform.position.z);
            collision.gameObject.transform.position = new Vector3(8, 8, transform.position.z);
            isDead = true;
            StartCoroutine(SetActive(collision.gameObject));
            GetComponent<AudioSource>().Play();
        }
    }

    private void Update() {
        if (!isDead) {
            if (transform.position.x > startPosition.x + 0.2f) {
                transform.position = new Vector3(startPosition.x + 0.2f, transform.position.y, transform.position.z);
                _rigidbody2D.velocity = new Vector2(-1, 0);
            }
            if (transform.position.x < startPosition.x - 0.2f) {
                transform.position = new Vector3(startPosition.x - 0.2f, transform.position.y, transform.position.z);
                _rigidbody2D.velocity = new Vector2(1, 0);
            } 
        }
    }

    IEnumerator SetActive(GameObject collisionGO) {
        yield return new WaitForSeconds(1);
        collisionGO.SetActive(false);
        gameObject.SetActive(false);
        isDead = false;
    }

}