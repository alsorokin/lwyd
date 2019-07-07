using UnityEngine;
using System;
using System.Globalization;
using Direction = MovementController.Direction;

public class Hero : Fighter
{
    public Command OnMoveRight = new NullCommand();
    public Command OnMoveLeft = new NullCommand();
    public Command OnMoveUp = new NullCommand();
    public Command OnMoveDown = new NullCommand();

    private const float InputThreshold = 0.001f;

    private float _horizontalAxis;
    private float _verticalAxis;

    public override GameObject Clone()
    {
        throw new Exception("Can't clone player... yet?");
    }

    protected override void Start()
    {
        base.Start();

        if (OnMoveRight.GetType() == typeof(NullCommand))
        {
            OnMoveRight = new MoveCommand(CurrentMovementController, Direction.Right);
        }

        if (OnMoveLeft.GetType() == typeof(NullCommand))
        {
            OnMoveLeft = new MoveCommand(CurrentMovementController, Direction.Left);
        }

        if (OnMoveUp.GetType() == typeof(NullCommand))
        {
            OnMoveUp = new MoveCommand(CurrentMovementController, Direction.Up);
        }

        if (OnMoveDown.GetType() == typeof(NullCommand))
        {
            OnMoveDown = new MoveCommand(CurrentMovementController, Direction.Down);
        }

        HasShadow = true;
    }

    void FixedUpdate()
    {
        _horizontalAxis = Input.GetAxis("Horizontal");
        _verticalAxis = Input.GetAxis("Vertical");

        if (Math.Abs(_horizontalAxis) > InputThreshold)
        {
            if (_horizontalAxis > 0)
            {
                OnMoveRight.Execute();
            }
            else
            {
                OnMoveLeft.Execute();
            }
        }

        if (Math.Abs(_verticalAxis) > InputThreshold)
        {
            if (_verticalAxis > 0)
            {
                OnMoveUp.Execute();
            }
            else
            {
                OnMoveDown.Execute();
            }
        }
    }

    void OnGUI() => GUI.Label(new Rect(
            new Vector2(0f, 0f),
            new Vector2(300f, 30f)),
            transform.position.x.ToString(CultureInfo.InvariantCulture) + ":" + transform.position.y.ToString(CultureInfo.InvariantCulture));
}
