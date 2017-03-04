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

    private bool nowMoving = true;
    

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (nowMoving)
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

    private float GetMovementScalar(float speed)
    {
        return speed * Time.deltaTime / 100;
    }
}
