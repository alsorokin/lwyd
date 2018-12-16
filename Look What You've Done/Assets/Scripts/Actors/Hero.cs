using UnityEngine;
using System;

public class Hero : Fighter
{
    private const float inputThreshold = 0.001f;

    private float horizontalAxis;
    private float verticalAxis;

    protected override void Start()
    {
        base.Start();
    }
    void FixedUpdate()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (Math.Abs(horizontalAxis) > inputThreshold)
        {
            if (horizontalAxis > 0)
            {
                mc.GoRight();
            }
            else
            {
                mc.GoLeft();
            }
        }

        if (Math.Abs(verticalAxis) > inputThreshold)
        {
            if (verticalAxis > 0)
            {
                mc.GoUp();
            }
            else
            {
                mc.GoDown();
            }
        }
    }

    void OnGUI() {}

    public override GameObject Clone()
    {
        throw new Exception("Can't clone player... yet.");
    }
}
