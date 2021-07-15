using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour{  

    [HideInInspector]public bool moving;

    Vector2 direction;
    
    private void Start() {
        direction = Vector2.right;

        
    }

    void Update(){
        if (moving) {
            if(transform.position.x > 2) {
                transform.position = new Vector3(2, transform.position.y, transform.position.z);
                direction = Vector2.left;
            }
            if (transform.position.x < -2) {
                transform.position = new Vector3(-2, transform.position.y, transform.position.z);
                direction = Vector2.right;
            }

            transform.Translate(direction * Time.deltaTime);
        }
    }
}
