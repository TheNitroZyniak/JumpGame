using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    float kickStrength = 0.75f;
    public bool isContact, death, changeHorVelocity, flyToRight;
    public bool bubble;
    public PhysicsMaterial2D pM2D;
  

    public Collider2D[] platformColliders;

    private Rigidbody2D rigitBody2D;

    private SpriteRenderer _renderer;

    void Start() {
        pM2D.bounciness = 0;
        GetComponent<Collider2D>().sharedMaterial = pM2D;
        rigitBody2D = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }


    float accX;

    private void Update() {
        accX = Input.acceleration.x * 12;
        if (!changeHorVelocity) rigitBody2D.velocity = new Vector2(accX, rigitBody2D.velocity.y);

        if (transform.position.x > 3.15f) transform.position = new Vector3(-3.12f, transform.position.y, -1);
        if (transform.position.x < -3.15f) transform.position = new Vector3(3.12f, transform.position.y, -1);

        if (rigitBody2D.velocity.y > 0) for (int i = 0; i < 3; i++) platformColliders[i].isTrigger = true;
        else for (int i = 0; i < 3; i++) platformColliders[i].isTrigger = false;

        //if (rigitBody2D.velocity.x > 0) _renderer.flipX = false;
        //else _renderer.flipX = true;

    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "CentralPlatform" || collision.gameObject.name == "LeftPlatform" || collision.gameObject.name == "RightPlatform") {
            if (!death) {
                if (collision.gameObject.name == "CentralPlatform") kickStrength = 1.2f;
                else kickStrength = 0.4f;
                ContactPoint2D contact = collision.contacts[0];
                isContact = true;
                changeHorVelocity = true;
                //rigitBody2D.angularVelocity = -contact.normal.x * 1000;
                rigitBody2D.AddForce(contact.normal * kickStrength, ForceMode2D.Impulse);

                float angle = Vector2.Angle(contact.normal, new Vector2(5, 0));

                transform.rotation = Quaternion.Euler(0, 0, angle - 90);

                if (Input.acceleration.x > 0) flyToRight = true;
                else flyToRight = false;

                //GetComponent<AudioSource>().Play();
                rigitBody2D.angularVelocity = 0;
                StartCoroutine(Fly(Quaternion.Euler(0, 0, angle - 90)));
                // GetComponent<Rigidbody2D>().velocity = new Vector2(contact.normal.x * 7, contact.normal.y * 7);
            }
        } else {
            //death = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.name == "Bullet(Clone)") collision.gameObject.GetComponent<Collider2D>().isTrigger = false;
    }


    IEnumerator Fly(Quaternion startRotation) {
        float a = 0;
        changeHorVelocity = true;
        while (a <= 1) {
            accX = Input.acceleration.x * 12;
            Vector2 endVel = new Vector2(accX, rigitBody2D.velocity.y);

            rigitBody2D.velocity = Vector2.Lerp(rigitBody2D.velocity, endVel, a);
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(0, 0, 0), a);
            //print(rigitBody2D.velocity + " " + endVel);

            a += 0.02f;
            yield return new WaitForFixedUpdate();
        }
        changeHorVelocity = false;
    }
}