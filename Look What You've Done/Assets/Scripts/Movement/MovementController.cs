﻿using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
    public enum Direction : sbyte
    {
        None = 0,
        Down = 1, Bottom = 1,
        Right = 2,
        Up = 3, Top = 3,
        Left = 4,
        TopLeft = 5,
        TopRight = 6,
        BottomRight = 7,
        BottomLeft = 8
    }

    protected Game game;
    protected Actor actor;
    public float movementSpeed = 333;

    public abstract bool IsMovingRight { get; }
    public abstract bool IsMovingLeft { get; }
    public abstract bool IsMovingUp { get; }
    public abstract bool IsMovingDown { get; }

    public bool IsMoving(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return IsMovingUp;
            case Direction.Down:
                return IsMovingDown;
            case Direction.Left:
                return IsMovingLeft;
            case Direction.Right:
                return IsMovingRight;
            case Direction.BottomLeft:
                return IsMovingDown && IsMovingLeft;
            case Direction.BottomRight:
                return IsMovingDown && IsMovingRight;
            case Direction.TopLeft:
                return IsMovingUp && IsMovingLeft;
            case Direction.TopRight:
                return IsMovingUp && IsMovingRight;
            default:
                return false;
        }
    }

    protected virtual void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        actor = gameObject.GetComponent<Actor>();
    }

    public abstract void GoUp();

    public abstract void GoDown();

    public abstract void GoLeft();

    public abstract void GoRight();

    public abstract void StopMoving();


    public virtual void Go(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                GoUp();
                break;
            case Direction.Down:
                GoDown();
                break;
            case Direction.Left:
                GoLeft();
                break;
            case Direction.Right:
                GoRight();
                break;
        }
    }

    protected float GetMovementScalar()
    {
        return this.movementSpeed * this.game.CurrentLevel.TileScale * Time.deltaTime / 100;
    }
}
