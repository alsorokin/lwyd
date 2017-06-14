using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayermoveScript : MonoBehaviour
{

    private bool sentUp = false;
    private bool sentDown = false;
    private bool sentLeft = false;
    private bool sentRight = false;

    void FixedUpdate()
    {
        MovementController mc = GetComponent<MovementController>();
        if (!mc.isMoving)
        {
            sentUp = false;
            sentDown = false;
            sentLeft = false;
            sentRight = false;
        }

        if (Input.GetAxis("Horizontal") > 0.001)
        {
            if (!sentRight)
            {
                mc.GoRight();
                sentRight = true;
            }
        }
        else
        {
            sentRight = false;
        }
        if (Input.GetAxis("Horizontal") < -0.001)
        {
            if (!sentLeft)
            {
                mc.GoLeft();
                sentLeft = true;
            }

        }
        else
        {
            sentLeft = false;
        }
        if (Input.GetAxis("Vertical") > 0.001)
        {
            if (!sentUp)
            {
                mc.GoUp();
                sentUp = true;
            }
        }
        else
        {
            sentUp = false;
        }
        if (Input.GetAxis("Vertical") < -0.001)
        {
            if (!sentDown)
            {
                mc.GoDown();
                sentDown = true;
            }
        }
        else
        {
            sentDown = false;
        }
    }

    void OnGUI()
    {
        if (Application.isEditor)
        {
            GUI.Label(new Rect(new Vector2(10, 10), new Vector2(50, 20)), Input.GetAxis("Horizontal").ToString());
            GUI.Label(new Rect(new Vector2(10, 30), new Vector2(50, 20)), Input.GetAxis("Vertical").ToString());
        }
    }
}
