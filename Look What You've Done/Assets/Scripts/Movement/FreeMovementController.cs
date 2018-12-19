using UnityEngine;

class FreeMovementController : MovementController
{
    public static float collisionCheckFrequency = 0.1f;

    private Vector2 movement = Vector2.zero;

    public override bool IsMovingRight
    {
        get
        {
            return movement.x > 0;
        }
    }

    public override bool IsMovingLeft
    {
        get
        {
            return movement.x < 0;
        }
    }

    public override bool IsMovingUp
    {
        get
        {
            return movement.y > 0;
        }
    }

    public override bool IsMovingDown
    {
        get
        {
            return movement.y < 0;
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(GetMovementScalar() * movement.x,
                                          GetMovementScalar() * movement.y, 
                                          0f);

        StopMoving();

        // Detect collisions here
        var threshold = 0.005f;

        var gridX = this.game.CurrentLevel.TranslateXToGrid(transform.position.x);
        var snappedX = this.game.CurrentLevel.TranslateGridToX(gridX);
        var distanceX = (transform.position.x - snappedX) / 10f;
        if (System.Math.Abs(distanceX) < threshold)
        {
            distanceX = 0f;
        }

        var gridY = this.game.CurrentLevel.TranslateYToGrid(transform.position.y);
        var snappedY = this.game.CurrentLevel.TranslateGridToY(gridY);
        var distanceY = (transform.position.y - snappedY) / 10f;
        if (System.Math.Abs(distanceY) < threshold)
        {
            distanceY = 0f;
        }

        // check if something is blocking us by x
        var shouldSnapX = false;
        if (distanceX > 0f)
        {
            shouldSnapX = !this.game.CurrentLevel.CanIGo(this.actor, Direction.Right);
            if (!shouldSnapX && distanceY > 0f)
            {
                shouldSnapX = !this.game.CurrentLevel.CanIGo(this.actor, Direction.TopRight);
            }
            else if (!shouldSnapX && distanceY < 0f)
            {
                shouldSnapX = !this.game.CurrentLevel.CanIGo(this.actor, Direction.BottomRight);
            }
        }
        else if(distanceX < 0f)
        {
            shouldSnapX = !this.game.CurrentLevel.CanIGo(this.actor, Direction.Left);
            if (!shouldSnapX && distanceY > 0f)
            {
                shouldSnapX = !this.game.CurrentLevel.CanIGo(this.actor, Direction.TopLeft);
            }
            else if (!shouldSnapX && distanceY < 0f)
            {
                shouldSnapX = !this.game.CurrentLevel.CanIGo(this.actor, Direction.BottomLeft);
            }
        }

        if (shouldSnapX)
        {
            transform.position -= new Vector3(distanceX, 0f, 0f);
        }

        // check if something is blocking us by y
        var shouldSnapY = false;
        if (distanceY > 0f)
        {
            shouldSnapY = !this.game.CurrentLevel.CanIGo(this.actor, Direction.Top);
            if (!shouldSnapY && distanceX > 0f)
            {
                shouldSnapY = !this.game.CurrentLevel.CanIGo(this.actor, Direction.TopRight);
            }
            else if (!shouldSnapY && distanceX < 0f)
            {
                shouldSnapY = !this.game.CurrentLevel.CanIGo(this.actor, Direction.TopLeft);
            }
        }
        else if (distanceY < 0f)
        {
            shouldSnapY = !this.game.CurrentLevel.CanIGo(this.actor, Direction.Bottom);
            if (!shouldSnapY && distanceX > 0f)
            {
                shouldSnapY = !this.game.CurrentLevel.CanIGo(this.actor, Direction.BottomRight);
            }
            else if (!shouldSnapY && distanceX < 0f)
            {
                shouldSnapY = !this.game.CurrentLevel.CanIGo(this.actor, Direction.BottomLeft);
            }
        }

        if (shouldSnapY)
        {
            transform.position -= new Vector3(0f, distanceY, 0f);
        }
    }

    public override void GoUp()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x, 
                                       actor.transform.position.y - ((this.game.CurrentLevel.TileScale / 2) - 0.001f), 
                                       0);
        if (!game.CurrentLevel.CanIGo(actor, fromPosition, Direction.Up))
        {
            return;
        }

        movement = new Vector2(movement.x, 1);
    }

    public override void GoDown()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x,
                                       actor.transform.position.y + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       0);
        if (!game.CurrentLevel.CanIGo(actor, fromPosition, Direction.Down))
        {
            return;
        }

        movement = new Vector2(movement.x, -1);
    }

    public override void GoLeft()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y,
                                       0);
        if (!game.CurrentLevel.CanIGo(actor, fromPosition, Direction.Left))
        {
            return;
        }

        movement = new Vector2(-1, movement.y);
    }

    public override void GoRight()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x - ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y,
                                       0);
        if (!game.CurrentLevel.CanIGo(actor, fromPosition, Direction.Right))
        {
            return;
        }

        movement = new Vector2(1, movement.y);
    }

    public override void StopMoving()
    {
        movement = Vector2.zero;
    }
}
