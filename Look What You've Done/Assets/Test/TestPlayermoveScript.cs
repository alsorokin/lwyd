using UnityEngine;
using System;

public class TestPlayermoveScript : MonoBehaviour
{
    public Command onMoveRight = new NullCommand();
    public Command onMoveLeft = new NullCommand();
    public Command onMoveUp = new NullCommand();
    public Command onMoveDown = new NullCommand();

    private const float inputThreshold = 0.001f;

    private float horizontalAxis;
    private float verticalAxis;

    // Use this for initialization
    void Start()
    {
        MovementController mc = GetComponent<MovementController>();
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
    }
    void FixedUpdate()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (Math.Abs(horizontalAxis) < inputThreshold && Math.Abs(verticalAxis) < inputThreshold)
        {
            return;
        }
        if (Math.Abs(horizontalAxis) > Math.Abs(verticalAxis))
        {
            if (horizontalAxis < 0)
            {
                onMoveLeft.Execute();
            } else
            {
                onMoveRight.Execute();
            }
        } else
        {
            if (verticalAxis < 0)
            {
                onMoveDown.Execute();
            } else
            {
                onMoveUp.Execute();
            }
        }
    }

    void OnGUI() {}
}
