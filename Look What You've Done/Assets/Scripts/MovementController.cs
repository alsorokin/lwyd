using UnityEngine;
using System;

public enum Direction : sbyte
{
    None = 0, Down = 1, Bottom = 1, Right = 2, Up = 3, Top = 3, Left = 4
}

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 333;
    public static float collisionCheckFrequency = 0.1f;

    private Direction direction = Direction.Up;
    private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 stopPosition;
    private Direction whereToNext = Direction.None;
    private Game game;
    private float collisionTimer = 0f;

    // Use this for initialization
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update() { }

    private void SwapDirection()
    {
        Vector3 temp = startPosition;
        startPosition = stopPosition;
        stopPosition = temp;
        switch (direction)
        {
            case Direction.Up:
                direction = Direction.Down;
                break;
            case Direction.Down:
                direction = Direction.Up;
                break;
            case Direction.Left:
                direction = Direction.Right;
                break;
            case Direction.Right:
                direction = Direction.Left;
                break;
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            collisionTimer += Time.deltaTime;
            if (collisionTimer >= collisionCheckFrequency)
            {
                collisionTimer = 0f;
                if (!game.currentLevel.CanIGo(gameObject.GetComponent<Actor>(), startPosition, direction))
                {
                    SwapDirection();
                }
            }
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
        Align();
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
        if (!game.currentLevel.CanIGo(gameObject.GetComponent<Actor>(), Direction.Up))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Up;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(0, 1, 0);
        }
        else
        {
            whereToNext = direction == Direction.Up ? Direction.None : Direction.Up;
        }
    }

    public void GoDown()
    {
        if (!game.currentLevel.CanIGo(gameObject.GetComponent<Actor>(), Direction.Down))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Down;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(0, -1, 0);
        }
        else
        {
            whereToNext = direction == Direction.Down ? Direction.None : Direction.Down;
        }
    }

    public void GoLeft()
    {
        if (!game.currentLevel.CanIGo(gameObject.GetComponent<Actor>(), Direction.Left))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Left;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(-1, 0, 0);
        }
        else
        {
            whereToNext = direction == Direction.Left ? Direction.None : Direction.Left;
        }
    }

    public void GoRight()
    {
        if (!game.currentLevel.CanIGo(gameObject.GetComponent<Actor>(), Direction.Right))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Right;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(1, 0, 0);
        }
        else
        {
            whereToNext = direction == Direction.Right ? Direction.None : Direction.Right;
        }
    }

    private float GetMovementScalar(float speed)
    {
        return speed * Time.deltaTime / 100;
    }

    // align to grid
    private void Align()
    {
        float oldX = gameObject.transform.position.x;
        float newX = game.currentLevel.TranslateGridToX(game.currentLevel.TranslateXToGrid(oldX));

        float oldY = gameObject.transform.position.y;
        float newY = game.currentLevel.TranslateGridToY(game.currentLevel.TranslateYToGrid(oldY));

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    void OnGUI() {}
}
