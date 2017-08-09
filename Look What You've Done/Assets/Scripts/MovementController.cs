using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction : sbyte
{
    None = 0, Down = 1, Bottom = 1, Right = 2, Up = 3, Top = 3, Left = 4
}

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 100;
    public Direction direction = Direction.Up;
    public bool isMoving = false;

    private Vector3 stopPosition;
    private Direction whereToNext = Direction.None;

    // Use this for initialization
    void Start() { }

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
        // only actually stop if no further commands were issued
        isMoving = whereToNext != Direction.None;
        // then start moving again, if user has already issued a new command
        direction = whereToNext;
        whereToNext = Direction.None;
    }

    public void GoUp()
    {
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

    void OnGUI()
    {
        
    }
}
