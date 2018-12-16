using UnityEngine;

public class GridMovementController : MovementController
{
    
    public static float collisionCheckFrequency = 0.1f;

    private Direction direction = Direction.Up;
    private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 stopPosition;
    private Direction whereToNext = Direction.None;

    private float collisionTimer = 0f;

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

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
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
                if (!game.CurrentLevel.CanIGo(gameObject.GetComponent<Actor>(), startPosition, direction))
                {
                    SwapDirection();
                }
            }
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
        if (direction != whereToNext)
        {
            direction = whereToNext;
            Go(direction);
        }
        whereToNext = Direction.None;
    }

    public override void GoUp()
    {
        if (!game.CurrentLevel.CanIGo(gameObject.GetComponent<Actor>(), Direction.Up))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Up;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(0, game.CurrentLevel.TileScale, 0);
        }
        else
        {
            whereToNext = direction == Direction.Up ? Direction.None : Direction.Up;
        }
    }

    public override void GoDown()
    {
        if (!game.CurrentLevel.CanIGo(gameObject.GetComponent<Actor>(), Direction.Down))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Down;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(0, -game.CurrentLevel.TileScale, 0);
        }
        else
        {
            whereToNext = direction == Direction.Down ? Direction.None : Direction.Down;
        }
    }

    public override void GoLeft()
    {
        if (!game.CurrentLevel.CanIGo(gameObject.GetComponent<Actor>(), Direction.Left))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Left;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(-game.CurrentLevel.TileScale, 0, 0);
        }
        else
        {
            whereToNext = direction == Direction.Left ? Direction.None : Direction.Left;
        }
    }

    public override void GoRight()
    {
        if (!game.CurrentLevel.CanIGo(gameObject.GetComponent<Actor>(), Direction.Right))
        {
            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            direction = Direction.Right;
            startPosition = transform.position;
            stopPosition = transform.position + new Vector3(game.CurrentLevel.TileScale, 0, 0);
        }
        else
        {
            whereToNext = direction == Direction.Right ? Direction.None : Direction.Right;
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
}
