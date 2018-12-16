using UnityEngine;

class FreeMovementController : MovementController
{
    public static float collisionCheckFrequency = 0.1f;

    private Vector2 movement = Vector2.zero;

    private void FixedUpdate()
    {
        transform.position += new Vector3(GetMovementScalar() * movement.x,
                                          GetMovementScalar() * movement.y, 0);
        StopMoving();
    }

    public override void GoUp()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x, 
                                       actor.transform.position.y - (this.game.CurrentLevel.TileScale / 2), 
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
                                       actor.transform.position.y + (this.game.CurrentLevel.TileScale / 2),
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
        var fromPosition = new Vector3(actor.transform.position.x + (this.game.CurrentLevel.TileScale / 2),
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
        var fromPosition = new Vector3(actor.transform.position.x - (this.game.CurrentLevel.TileScale / 2),
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
