using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayermoveScript : MonoBehaviour {

    void FixedUpdate()
    {
        MovementController mc = GetComponent<MovementController>();

        if (Input.GetAxis("Horizontal") > 0.1) {
            mc.GoRight();
        }
        if (Input.GetAxis("Horizontal") < -0.1) {
            mc.GoLeft();
        }
        if (Input.GetAxis("Vertical") > 0.1) {
            mc.GoUp();
        }
        if (Input.GetAxis("Vertical") < -0.1) {
            mc.GoDown();
        }
    } 

    void OnGUI()
    {
        if (Application.isEditor) {
            GUI.Label(new Rect(new Vector2(10, 10), new Vector2(50, 20)), Input.GetAxis("Horizontal").ToString());
            GUI.Label(new Rect(new Vector2(10, 30), new Vector2(50, 20)), Input.GetAxis("Vertical").ToString());
        }
    }
}
