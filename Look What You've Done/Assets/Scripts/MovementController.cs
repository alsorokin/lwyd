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
    private Vector3 stopPosition;

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
                    transform.position += new Vector3(0, -GetMovementScalar(movementSpeed), 0);
                    if (transform.position.y <= stopPosition.y)  {
                        isMoving = false;
                        transform.position = stopPosition;
                    }
                    break;
                case Direction.Up:
                    transform.position += new Vector3(0, GetMovementScalar(movementSpeed), 0);
                    if (transform.position.y >= stopPosition.y)  {
                        isMoving = false;
                        transform.position = stopPosition;
                    }
                    break;
                case Direction.Left:
                    transform.position += new Vector3(-GetMovementScalar(movementSpeed), 0, 0);
                    if (transform.position.x <= stopPosition.x)  {
                        isMoving = false;
                        transform.position = stopPosition;
                    }
                    break;
                case Direction.Right:
                    transform.position += new Vector3(GetMovementScalar(movementSpeed), 0, 0);
                    if (transform.position.x >= stopPosition.x)  {
                        isMoving = false;
                        transform.position = stopPosition;
                    }
                    break;
            }
        }
    }

    public void GoUp() {
        if (!isMoving) {
            isMoving = true;
            direction = Direction.Up;
            stopPosition = transform.position + new Vector3(0, 1, 0);
        }
    }

    public void GoDown() {
        if (!isMoving) {
            isMoving = true;
            direction = Direction.Down;
            stopPosition = transform.position + new Vector3(0, -1, 0);
        }
    }

    public void GoLeft() {
        if (!isMoving) {
            isMoving = true;
            direction = Direction.Left;
            stopPosition = transform.position + new Vector3(-1, 0, 0);
        }
    }

    public void GoRight() {
        if (!isMoving) {
            isMoving = true;
            direction = Direction.Right;
            stopPosition = transform.position + new Vector3(1, 0, 0);
        }
    }

    private float GetMovementScalar(float speed)
    {
        return speed * Time.deltaTime / 100;
    }

    void OnGUI()
    {
        if (Application.isEditor) {
            GUI.Label(new Rect(new Vector2(10, 50), new Vector2(550, 20)), "isMoving: " + isMoving.ToString());
            GUI.Label(new Rect(new Vector2(10, 70), new Vector2(550, 20)), "stopPosition" + stopPosition.ToString());
        }
    }
}
