using UnityEngine;

class FreeMovementController : MovementController
{
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
    }

    public override void GoUp()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x, 
                                       actor.transform.position.y - ((this.game.CurrentLevel.TileScale / 2) - 0.001f), 
                                       0);

        movement = new Vector2(movement.x, 1);
    }

    public override void GoDown()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x,
                                       actor.transform.position.y + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       0);

        movement = new Vector2(movement.x, -1);
    }

    public override void GoLeft()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y,
                                       0);

        movement = new Vector2(-1, movement.y);
    }

    public override void GoRight()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x - ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y,
                                       0);

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

        movement = new Vector2(-1, 1);
    }

    public override void GoTopRight()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x - ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y - ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       0);

        movement = new Vector2(1, 1);
    }

    public override void GoBottomLeft()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       0);

        movement = new Vector2(-1, -1);
    }

    public override void GoBottomRight()
    {
        var actor = gameObject.GetComponent<Actor>();
        var fromPosition = new Vector3(actor.transform.position.x - ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       actor.transform.position.y + ((this.game.CurrentLevel.TileScale / 2) - 0.001f),
                                       0);

        movement = new Vector2(1, -1);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("onEnter");
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log("onStay");
    }
}
