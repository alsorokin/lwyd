using UnityEngine;

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

    protected Game Game;
    protected Actor Actor;
    public float MovementSpeed = 333;

    public abstract bool IsMovingRight { get; }
    public abstract bool IsMovingLeft { get; }
    public abstract bool IsMovingUp { get; }
    public abstract bool IsMovingDown { get; }

    public bool IsMovingInDirection(Direction dir)
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
        Game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        Actor = gameObject.GetComponent<Actor>();
    }

    public abstract void GoUp();

    public abstract void GoDown();

    public abstract void GoLeft();

    public abstract void GoRight();

    public abstract void GoTopLeft();

    public abstract void GoTopRight();

    public abstract void GoBottomLeft();

    public abstract void GoBottomRight();

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
            case Direction.TopLeft:
                GoUp();
                GoLeft();
                break;
            case Direction.TopRight:
                GoUp();
                GoRight();
                break;
            case Direction.BottomLeft:
                GoLeft();
                GoDown();
                break;
            case Direction.BottomRight:
                GoRight();
                GoDown();
                break;
        }
    }

    public abstract bool IsMoving { get; }

    protected float GetMovementScalar()
    {
        return MovementSpeed * Time.deltaTime / 100;
    }
}
