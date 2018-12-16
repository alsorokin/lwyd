using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
    public enum Direction : sbyte
    {
        None = 0, Down = 1, Bottom = 1, Right = 2, Up = 3, Top = 3, Left = 4
    }

    protected Game game;
    public float movementSpeed = 333;

    protected virtual void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    }

    public abstract void GoUp();

    public abstract void GoDown();

    public abstract void GoLeft();

    public abstract void GoRight();

    public abstract void StopMoving();

    public void Go(Direction dir)
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
