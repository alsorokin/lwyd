using UnityEngine;

class FreeMovementController : MovementController
{
    private Vector2 _movement = Vector2.zero;

    public override bool IsMovingRight => _movement.x > 0;

    public override bool IsMovingLeft => _movement.x < 0;

    public override bool IsMovingUp => _movement.y > 0;

    public override bool IsMovingDown => _movement.y < 0;

    public override bool IsMoving => _movement.x > 0 || _movement.y > 0;

    public override void GoUp() => _movement = new Vector2(_movement.x, 1);

    public override void GoDown() => _movement = new Vector2(_movement.x, -1);

    public override void GoLeft() => _movement = new Vector2(-1, _movement.y);

    public override void GoRight() => _movement = new Vector2(1, _movement.y);

    public override void StopMoving() => _movement = Vector2.zero;

    public override void GoTopLeft() => _movement = new Vector2(-1, 1);

    public override void GoTopRight() => _movement = new Vector2(1, 1);

    public override void GoBottomLeft() => _movement = new Vector2(-1, -1);

    public override void GoBottomRight() => _movement = new Vector2(1, -1);

    private void FixedUpdate()
    {
        transform.position += new Vector3(GetMovementScalar() * _movement.x,
                                               GetMovementScalar() * _movement.y,
                                               0f);

        StopMoving();
    }
}
