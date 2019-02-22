using UnityEngine;
using System;
using Direction = MovementController.Direction;

public class Hero : Fighter
{
    public Command onMoveRight = new NullCommand();
    public Command onMoveLeft = new NullCommand();
    public Command onMoveUp = new NullCommand();
    public Command onMoveDown = new NullCommand();

    private const float inputThreshold = 0.001f;

    private float horizontalAxis;
    private float verticalAxis;

    protected override void Start()
    {
        base.Start();

        if (onMoveRight.GetType() == typeof(NullCommand))
        {
            onMoveRight = new MoveCommand(mc, Direction.Right);
        }

        if (onMoveLeft.GetType() == typeof(NullCommand))
        {
            onMoveLeft = new MoveCommand(mc, Direction.Left);
        }

        if (onMoveUp.GetType() == typeof(NullCommand))
        {
            onMoveUp = new MoveCommand(mc, Direction.Up);
        }

        if (onMoveDown.GetType() == typeof(NullCommand))
        {
            onMoveDown = new MoveCommand(mc, Direction.Down);
        }

        this.HasShadow = true;
    }
    void FixedUpdate()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (Math.Abs(horizontalAxis) > inputThreshold)
        {
            if (horizontalAxis > 0)
            {
                onMoveRight.Execute();
            }
            else
            {
                onMoveLeft.Execute();
            }
        }

        if (Math.Abs(verticalAxis) > inputThreshold)
        {
            if (verticalAxis > 0)
            {
                onMoveUp.Execute();
            }
            else
            {
                onMoveDown.Execute();
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(new Vector2(0f, 0f), new Vector2(300f, 30f)), transform.position.x.ToString() + ":" + transform.position.y.ToString());
    }

    public override GameObject Clone()
    {
        throw new Exception("Can't clone player... yet.");
    }
}
