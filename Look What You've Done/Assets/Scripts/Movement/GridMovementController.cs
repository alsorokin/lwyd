using UnityEngine;

public class GridMovementController : MovementController
{
    private Direction _direction = Direction.Up;
    private bool _isMoving;
    private Vector3 _startPosition;
    private Vector3 _stopPosition;

    public override bool IsMovingUp => _isMoving && _direction == Direction.Up;

    public override bool IsMovingDown => _isMoving && _direction == Direction.Down;

    public override bool IsMovingLeft => _isMoving && _direction == Direction.Left;

    public override bool IsMovingRight => _isMoving && _direction == Direction.Right;

    public override bool IsMoving => _isMoving;

    public override void StopMoving()
    {
        Align();
        _isMoving = false;
    }

    public override void GoUp()
    {
        if (_isMoving) return;

        _isMoving = true;
        _direction = Direction.Up;
        _startPosition = transform.position;
        _stopPosition = transform.position + new Vector3(0, 1f, 0);
    }

    public override void GoDown()
    {
        if (!_isMoving) return;

        _isMoving = true;
        _direction = Direction.Down;
        _startPosition = transform.position;
        _stopPosition = transform.position + new Vector3(0, -1f, 0);
    }

    public override void GoLeft()
    {
        if (_isMoving) return;

        _isMoving = true;
        _direction = Direction.Left;
        _startPosition = transform.position;
        _stopPosition = transform.position + new Vector3(-1f, 0, 0);
    }

    public override void GoRight()
    {
        if (_isMoving) return;

        _isMoving = true;
        _direction = Direction.Right;
        _startPosition = transform.position;
        _stopPosition = transform.position + new Vector3(1f, 0, 0);
    }

    public override void GoTopRight()
    {
        if (_isMoving) return;

        _isMoving = true;
        _direction = Direction.TopRight;
        _startPosition = transform.position;
        _stopPosition = transform.position + new Vector3(1f, 1f, 0);
    }

    public override void GoTopLeft()
    {
        if (_isMoving) return;
    
        _isMoving = true;
        _direction = Direction.TopLeft;
        _startPosition = transform.position;
        _stopPosition = transform.position + new Vector3(-1f, 1f, 0);
    }

    public override void GoBottomLeft()
    {
        if (_isMoving) return;

        _isMoving = true;
        _direction = Direction.BottomLeft;
        _startPosition = transform.position;
        _stopPosition = transform.position + new Vector3(-1f, -1f, 0);
    }

    public override void GoBottomRight()
    {
        if (_isMoving) return;

        _isMoving = true;
        _direction = Direction.BottomRight;
        _startPosition = transform.position;
        _stopPosition = transform.position + new Vector3(1f, -1f, 0);
    }

    // align to grid
    private void Align()
    {
        float oldX = gameObject.transform.position.x;
        float newX = Game.CurrentLevel.TranslateGridToX(Game.CurrentLevel.TranslateXToGrid(oldX));

        float oldY = gameObject.transform.position.y;
        float newY = Game.CurrentLevel.TranslateGridToY(Game.CurrentLevel.TranslateYToGrid(oldY));

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    void OnGUI() {}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Swapping");
        SwapDirection();
    }
    
    // Update is called once per frame
    void Update() { }

    void FixedUpdate()
    {
        if (_isMoving)
        {
            switch (_direction)
            {
                case Direction.Down:
                    transform.position += new Vector3(0, -GetMovementScalar(), 0);
                    if (transform.position.y <= _stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.Up:
                    transform.position += new Vector3(0, GetMovementScalar(), 0);
                    if (transform.position.y >= _stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.Left:
                    transform.position += new Vector3(-GetMovementScalar(), 0, 0);
                    if (transform.position.x <= _stopPosition.x)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.Right:
                    transform.position += new Vector3(GetMovementScalar(), 0, 0);
                    if (transform.position.x >= _stopPosition.x)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.TopRight:
                    transform.position += new Vector3(GetMovementScalar(), GetMovementScalar(), 0);
                    if (transform.position.x >= _stopPosition.x || transform.position.y >= _stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.TopLeft:
                    transform.position += new Vector3(-GetMovementScalar(), GetMovementScalar(), 0);
                    if (transform.position.x <= _stopPosition.x || transform.position.y >= _stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.BottomRight:
                    transform.position += new Vector3(GetMovementScalar(), -GetMovementScalar(), 0);
                    if (transform.position.x >= _stopPosition.x || transform.position.y <= _stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
                case Direction.BottomLeft:
                    transform.position += new Vector3(-GetMovementScalar(), -GetMovementScalar(), 0);
                    if (transform.position.x <= _stopPosition.x || transform.position.y <= _stopPosition.y)
                    {
                        StopMoving();
                    }

                    break;
            }
        }
    }

    private void SwapDirection()
    {
        Vector3 temp = _startPosition;
        _startPosition = _stopPosition;
        _stopPosition = temp;
        switch (_direction)
        {
            case Direction.Up:
                _direction = Direction.Down;
                break;
            case Direction.Down:
                _direction = Direction.Up;
                break;
            case Direction.Left:
                _direction = Direction.Right;
                break;
            case Direction.Right:
                _direction = Direction.Left;
                break;
            case Direction.TopRight:
                _direction = Direction.BottomLeft;
                break;
            case Direction.TopLeft:
                _direction = Direction.BottomRight;
                break;
            case Direction.BottomLeft:
                _direction = Direction.TopRight;
                break;
            case Direction.BottomRight:
                _direction = Direction.TopLeft;
                break;
        }
    }
}
