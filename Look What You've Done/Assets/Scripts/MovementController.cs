using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction : sbyte
{
    None=0, Down=1, Bottom=1, Right=2, Up=3, Top=3, Left=4
}

public class MovementController : MonoBehaviour {
    public float movementSpeed = 100;
    public Direction direction = Direction.Up;

    private bool isMoving = false;
    private Vector3 startPosition;

    // Use this for initialization
    void Start () {}
    
    // Update is called once per frame
    void Update () {}

    void FixedUpdate()
    {
        if (isMoving)
        {
            switch (direction)
            {
                case Direction.Down:
                    transform.position -= new Vector3(0, GetMovementScalar(movementSpeed), 0);
                    break;
                case Direction.Up:
                    transform.position += new Vector3(0, GetMovementScalar(movementSpeed), 0);
                    break;
                case Direction.Left:
                    transform.position -= new Vector3(GetMovementScalar(movementSpeed), 0, 0);
                    break;
                case Direction.Right:
                    transform.position += new Vector3(GetMovementScalar(movementSpeed), 0, 0);
                    break;
            }
        }
    }

    public void GoUp() {
        if (!isMoving) {
            isMoving = true;
            direction = Direction.Up;
            startPosition = transform.position;
        }
    }

    public void GoDown() {
        if (!isMoving) {
            isMoving = true;
            direction = Direction.Down;
            startPosition = transform.position;
        }
    }

    public void GoLeft() {
        if (!isMoving) {
            isMoving = true;
            direction = Direction.Left;
            startPosition = transform.position;
        }
    }

    public void GoRight() {
        if (!isMoving) {
            isMoving = true;
            direction = Direction.Left;
            startPosition = transform.position;
        }
    }

    private float GetMovementScalar(float speed)
    {
        return speed * Time.deltaTime / 100;
    }
}
