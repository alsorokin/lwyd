using UnityEngine;
using System;

public enum Direction : sbyte
{
    None = 0, Down = 1, Bottom = 1, Right = 2, Up = 3, Top = 3, Left = 4
}

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 100;
    public Direction direction = Direction.Up;

    private bool isMoving = false;
    private Vector3 stopPosition;
    private Direction whereToNext = Direction.None;
    private Game game;

    // Use this for initialization
    void Start()
    {
        game = (Game)GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update() { }

    void FixedUpdate()
    {
        if (isMoving)
        {
            switch (direction)
            {
                case Direction.Down:
                    transform.position += new Vector3(0, -GetMovementScalar(movementSpeed), 0);
                    if (transform.position.y <= stopPosition.y)
                    {
                        StopMoving();
                    }
                    break;
                case Direction.Up:
                    transform.position += new Vector3(0, GetMovementScalar(movementSpeed), 0);
                    if (transform.position.y >= stopPosition.y)
                    {
                        StopMoving();
                    }
                    break;
                case Direction.Left:
                    transform.position += new Vector3(-GetMovementScalar(movementSpeed), 0, 0);
                    if (transform.position.x <= stopPosition.x)
                    {
                        StopMoving();
                    }
                    break;
                case Direction.Right:
                    transform.position += new Vector3(GetMovementScalar(movementSpeed), 0, 0);
                    if (transform.position.x >= stopPosition.x)
                    {
                        StopMoving();
                    }
                    break;
            }
        }
    }

    private void StopMoving()
    {
        // align to grid
        transform.position = stopPosition;
        // stop
        isMoving = false;
        // then start moving again, if user has already issued a new command
        if (direction != whereToNext)
        {
            direction = whereToNext;
            Go(direction);
        }
        whereToNext = Direction.None;
    }

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

    public void GoUp()
    {
        if (!game.CurrentLevel.CanIGo(new Vector2(transform.position.x, transform.position.y), Direction.Up))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Up;
            stopPosition = transform.position + new Vector3(0, 1, 0);
        }
        else
        {
            whereToNext = Direction.Up;
        }
    }

    public void GoDown()
    {
        if (!game.CurrentLevel.CanIGo(new Vector2(transform.position.x, transform.position.y), Direction.Down))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Down;
            stopPosition = transform.position + new Vector3(0, -1, 0);
        }
        else
        {
            whereToNext = Direction.Down;
        }
    }

    public void GoLeft()
    {
        if (!game.CurrentLevel.CanIGo(new Vector2(transform.position.x, transform.position.y), Direction.Left))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Left;
            stopPosition = transform.position + new Vector3(-1, 0, 0);
        }
        else
        {
            whereToNext = Direction.Left;
        }
    }

    public void GoRight()
    {
        if (!game.CurrentLevel.CanIGo(new Vector2(transform.position.x, transform.position.y), Direction.Right))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Right;
            stopPosition = transform.position + new Vector3(1, 0, 0);
        }
        else
        {
            whereToNext = Direction.Right;
        }
    }

    private float GetMovementScalar(float speed)
    {
        return speed * Time.deltaTime / 100;
    }

    void OnGUI() {}
}
