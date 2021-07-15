using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour{
    [HideInInspector] public bool moving;

    Vector2 direction;
    Rigidbody2D rb;

    void Start(){
        direction = Vector2.right;
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = new Vector2(1, 0);
    }

    
    void Update(){
        if (moving) {
            if (Levels.level == 2) {
                if (transform.position.x > 2) {
                    transform.position = new Vector3(2, transform.position.y, transform.position.z);
                    direction = Vector2.left;
                    rb.velocity = new Vector2(-1, 0);
                }
                if (transform.position.x < -2) {
                    transform.position = new Vector3(-2, transform.position.y, transform.position.z);
                    rb.velocity = new Vector2(1, 0);
                    direction = Vector2.right;
                }
            }
        }
    }
}
