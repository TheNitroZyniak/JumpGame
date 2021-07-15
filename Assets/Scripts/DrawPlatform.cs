using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPlatform : MonoBehaviour{
    private Vector2 startPoint, endPoint;

    public Transform platform;
    public Transform[] sides;

    public Camera _camera;
    float cam = 0;
    float x0 = 0, y0 = 0;
    Vector2 vr1;

    bool allowDraw = false;
    public GameObject player;
    Rigidbody2D rb;

    private void Start() {
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && Player.jumpingType == 0) {
            Player.collisionMade = false;
            startPoint = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            if (startPoint.y < player.transform.position.y - 0.3f) {
                var screenPoint = (Input.mousePosition);
                screenPoint.z = 10.0f;

                vr1 = Camera.main.ScreenToWorldPoint(screenPoint);
                x0 = vr1.x;
                y0 = vr1.y;
                cam = _camera.transform.position.y;
                allowDraw = true;
            } 
            else {
                allowDraw = false;
            }
            if (startPoint.y > player.transform.position.y + 0.3f) {
                //allowDraw = false;
            }

        }

        if (Input.GetMouseButton(0) && allowDraw && !Player.collisionMade && Player.jumpingType == 0) {
            float y01 = _camera.transform.position.y - cam;

            var screenPoint = (Input.mousePosition);
            screenPoint.z = 10.0f;
            Vector3 vr = Camera.main.ScreenToWorldPoint(screenPoint);
            float x1 = vr.x;
            float y1 = vr.y;

            if (vr.y > player.transform.position.y - 0.3f) {
                Player.collisionMade = true;
                return;
            }

            sides[0].transform.position = new Vector3(x0, y0 + y01, -1);
            sides[1].transform.position = new Vector3(x1, y1, -1);

            platform.transform.position = new Vector3((x0 + x1) / 2f, (sides[1].transform.position.y + sides[0].transform.position.y) / 2f, -1);
            platform.transform.localScale = new Vector2(Mathf.Sqrt((x1 - x0) * (x1 - x0) + ((sides[1].transform.position.y - sides[0].transform.position.y)) * ((sides[1].transform.position.y - sides[0].transform.position.y))), 0.2f);

            float angle;
            if (x1 - x0 != 0) angle = Mathf.Atan((sides[1].transform.position.y - sides[0].transform.position.y) / (x1 - x0)) * Mathf.Rad2Deg;
            else angle = 90;

            platform.transform.localEulerAngles = new Vector3(0, 0, angle);
            if (x0 < x1) {
                sides[0].transform.localEulerAngles = new Vector3(0, 0, angle);
                sides[1].transform.localEulerAngles = new Vector3(0, 0, angle);
            } else {
                sides[0].transform.localEulerAngles = new Vector3(0, 0, 180 + angle);
                sides[1].transform.localEulerAngles = new Vector3(0, 0, 180 + angle);
            }

        }
    }
}
