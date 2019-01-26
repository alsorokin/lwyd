using UnityEngine;

public class GridMovementController : MovementController
{
    private Direction direction = Direction.Up;
    private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 stopPosition;
    private Direction whereToNext = Direction.None;

    public override bool IsMovingUp
    {
        get
        {
            return this.isMoving && this.direction == Direction.Up;
        }
    }

    public override bool IsMovingDown
    {
        get
        {
            return this.isMoving && this.direction == Direction.Down;
        }
    }

    public override bool IsMovingLeft
    {
        get
        {
            return this.isMoving && this.direction == Direction.Left;
        }
    }

    public override bool IsMovingRight
    {
        get
        {
            return this.isMoving && this.direction == Direction.Right;
        }
    }

    public override bool IsMoving()
    {
        return isMoving;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
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
                    transform.position += new Vector3(0, -GetMovementScalar(), 0);
                    if (transform.position.y <= stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.Up:
                    transform.position += new Vector3(0, GetMovementScalar(), 0);
                    if (transform.position.y >= stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.Left:
                    transform.position += new Vector3(-GetMovementScalar(), 0, 0);
                    if (transform.position.x <= stopPosition.x)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.Right:
                    transform.position += new Vector3(GetMovementScalar(), 0, 0);
                    if (transform.position.x >= stopPosition.x)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.TopRight:
                    transform.position += new Vector3(GetMovementScalar(), GetMovementScalar(), 0);
                    if (transform.position.x >= stopPosition.x || transform.position.y >= stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.TopLeft:
                    transform.position += new Vector3(-GetMovementScalar(), GetMovementScalar(), 0);
                    if (transform.position.x <= stopPosition.x || transform.position.y >= stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.BottomRight:
                    transform.position += new Vector3(GetMovementScalar(), -GetMovementScalar(), 0);
                    if (transform.position.x >= stopPosition.x || transform.position.y <= stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.BottomLeft:
                    transform.position += new Vector3(-GetMovementScalar(), -GetMovementScalar(), 0);
                    if (transform.position.x <= stopPosition.x || transform.position.y <= stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
            }
        }
    }

    public override void StopMoving()
    {
        // align to grid
        Align();
        // stop
        isMoving = false;

        // then start moving again, if user has already issued a new command
        // commented out for the time being. whereToNext is not working good with generic enemy script right now
        // and we don't use grid movement for the player anymore, so looks like we don't need it
        //if (direction != whereToNext)
        //{
        //    direction = whereToNext;
        //    Go(direction);
        //}

        whereToNext = Direction.None;
    }

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
            case Direction.TopRight:
                direction = Direction.BottomLeft;
                break;
            case Direction.TopLeft:
                direction = Direction.BottomRight;
                break;
            case Direction.BottomLeft:
                direction = Direction.TopRight;
                break;
            case Direction.BottomRight:
                direction = Direction.TopLeft;
                break;
        }
    }

    public override void GoUp()
    {
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Up;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(0, 1f, 0);
        }
        else
        {
            whereToNext = direction == Direction.Up ? Direction.None : Direction.Up;
        }
    }

    public override void GoDown()
    {
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Down;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(0, -1f, 0);
        }
        else
        {
            whereToNext = direction == Direction.Down ? Direction.None : Direction.Down;
        }
    }

    public override void GoLeft()
    {
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Left;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(-1f, 0, 0);
        }
        else
        {
            whereToNext = direction == Direction.Left ? Direction.None : Direction.Left;
        }
    }

    public override void GoRight()
    {
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Right;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(1f, 0, 0);
        }
        else
        {
            whereToNext = direction == Direction.Right ? Direction.None : Direction.Right;
        }
    }

    public override void GoTopRight()
    {
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.TopRight;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(1f, 1f, 0);
        }
        else
        {
            whereToNext = direction == Direction.TopRight ? Direction.None : Direction.TopRight;
        }
    }

    public override void GoTopLeft()
    {
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.TopLeft;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(-1f, 1f, 0);
        }
        else
        {
            whereToNext = direction == Direction.TopLeft ? Direction.None : Direction.TopLeft;
        }
    }

    public override void GoBottomLeft()
    {
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.BottomLeft;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(-1f, -1f, 0);
        }
        else
        {
            whereToNext = direction == Direction.BottomLeft ? Direction.None : Direction.BottomLeft;
        }
    }

    public override void GoBottomRight()
    {
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.BottomRight;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(1f, -1f, 0);
        }
        else
        {
            whereToNext = direction == Direction.BottomRight ? Direction.None : Direction.BottomRight;
        }
    }

    // align to grid
    private void Align()
    {
        float oldX = gameObject.transform.position.x;
        float newX = game.CurrentLevel.TranslateGridToX(game.CurrentLevel.TranslateXToGrid(oldX));

        float oldY = gameObject.transform.position.y;
        float newY = game.CurrentLevel.TranslateGridToY(game.CurrentLevel.TranslateYToGrid(oldY));

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    void OnGUI() {}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Swapping");
        SwapDirection();
    }
}
