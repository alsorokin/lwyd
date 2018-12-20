using UnityEngine;

class FreeMovementController : MovementController
{
    public float collisionCheckFrequency = 0.1f;
    private const float bumpSpeed = 0.01f;
    private const float bumpSpeedThreshold = 0.005f;

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

    public override bool IsMoving()
    {
        return movement.x > 0 || movement.y > 0;
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(GetMovementScalar() * movement.x,
                                          GetMovementScalar() * movement.y, 
                                          0f);

        StopMoving();

        var gridX = this.game.CurrentLevel.TranslateXToGrid(transform.position.x);
        var snappedX = this.game.CurrentLevel.TranslateGridToX(gridX);
        var distanceX = (transform.position.x - snappedX) / 10f;
        if (System.Math.Abs(distanceX) < bumpSpeedThreshold)
        {
            distanceX = 0f;
        }

        var gridY = this.game.CurrentLevel.TranslateYToGrid(transform.position.y);
        var snappedY = this.game.CurrentLevel.TranslateGridToY(gridY);
        var distanceY = (transform.position.y - snappedY) / 10f;
        if (System.Math.Abs(distanceY) < bumpSpeedThreshold)
        {
            distanceY = 0f;
        }

        // If we're stacked, try to break free
        if (!this.game.CurrentLevel.CanIGo(this.actor, Direction.None))
        {
            var bumpDistanceX = 0f;
            var bumpDistanceY = 0f;

            // We need to choose the side we're about to be bumped to
            // First, check three tiles on the right and on the left
            if (distanceX >= 0f)
            {
                if (this.game.CurrentLevel.CanIGo(this.actor, Direction.Right))
                {
                    bumpDistanceX = bumpSpeed;
                }
                else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.TopRight))
                {
                    bumpDistanceX = bumpSpeed;
                    bumpDistanceY = bumpSpeed;
                }
                else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.BottomRight))
                {
                    bumpDistanceX = bumpSpeed;
                    bumpDistanceY = -bumpSpeed;
                }
            }
            else // if (distanceX < 0f)
            {
                if (this.game.CurrentLevel.CanIGo(this.actor, Direction.Left))
                {
                    bumpDistanceX = -bumpSpeed;
                }
                else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.BottomLeft))
                {
                    bumpDistanceX = -bumpSpeed;
                    bumpDistanceY = -bumpSpeed;
                }
                else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.TopLeft))
                {
                    bumpDistanceX = -bumpSpeed;
                    bumpDistanceY = bumpSpeed;
                }
            }

            // Then, check two remaining tiles (top and bottom)
            if (distanceY >= 0f)
            {
                if (this.game.CurrentLevel.CanIGo(this.actor, Direction.Up))
                {
                    bumpDistanceY = bumpSpeed;
                }
            }
            else // if (distanceY < 0f)
            {
                if (this.game.CurrentLevel.CanIGo(this.actor, Direction.Down))
                {
                    bumpDistanceY = -bumpSpeed;
                }
            }

            transform.position -= new Vector3(bumpDistanceX, bumpDistanceY, 0f);
        }
        else
        {
            // We're not stacked, so let's check if we're colliding with something around us

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
            else if (distanceX < 0f)
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
    }

    private void BumpSomewhere()
    {
        if (this.game.CurrentLevel.CanIGo(this.actor, Direction.TopLeft))
        {
            transform.position += new Vector3(-bumpSpeed, bumpSpeed, 0f);
        }
        else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.Top))
        {
            transform.position += new Vector3(0f, bumpSpeed, 0f);
        }
        else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.TopRight))
        {
            transform.position += new Vector3(bumpSpeed, bumpSpeed, 0f);
        }
        else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.Right))
        {
            transform.position += new Vector3(bumpSpeed, 0f, 0f);
        }
        else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.BottomRight))
        {
            transform.position += new Vector3(bumpSpeed, -bumpSpeed, 0f);
        }
        else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.Down))
        {
            transform.position += new Vector3(0f, -bumpSpeed, 0f);
        }
        else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.BottomLeft))
        {
            transform.position += new Vector3(-bumpSpeed, -bumpSpeed, 0f);
        }
        else if (this.game.CurrentLevel.CanIGo(this.actor, Direction.Left))
        {
            transform.position += new Vector3(-bumpSpeed, 0f, 0f);
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

    public override void GoTopLeft()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y - ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       0);
        if (!game.CurrentLevel.CanIGo(actor, fromPosition, Direction.TopLeft))
        {
            return;
        }

        movement = new Vector2(-1, 1);
    }

    public override void GoTopRight()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x - ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y - ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       0);
        if (!game.CurrentLevel.CanIGo(actor, fromPosition, Direction.TopRight))
        {
            return;
        }

        movement = new Vector2(1, 1);
    }

    public override void GoBottomLeft()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       0);
        if (!game.CurrentLevel.CanIGo(actor, fromPosition, Direction.BottomLeft))
        {
            return;
        }

        movement = new Vector2(-1, -1);
    }

    public override void GoBottomRight()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x - ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       0);
        if (!game.CurrentLevel.CanIGo(actor, fromPosition, Direction.BottomRight))
        {
            return;
        }

        movement = new Vector2(1, -1);
    }
}
