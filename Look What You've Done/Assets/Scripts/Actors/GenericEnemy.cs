using System.Collections.Generic;
using UnityEngine;
using Direction = MovementController.Direction;

class GenericEnemy : Actor
{
    public Command onMoveRight = new NullCommand();
    public Command onMoveLeft = new NullCommand();
    public Command onMoveUp = new NullCommand();
    public Command onMoveDown = new NullCommand();

    public float moveTimeThreshold = 1f;

    private float timeElapsed;
    private Direction currentDirection = Direction.None;
    private bool isThinking = false;

    protected override void Start()
    {
        base.Start();

        if (onMoveRight.GetType() == typeof(NullCommand))
        {
            onMoveRight = new MoveCommand(mc, Direction.Right);
        }

        if (onMoveLeft.GetType() == typeof(NullCommand))
        {
            onMoveLeft = new MoveCommand(mc, Direction.Left);
        }

        if (onMoveUp.GetType() == typeof(NullCommand))
        {
            onMoveUp = new MoveCommand(mc, Direction.Up);
        }

        if (onMoveDown.GetType() == typeof(NullCommand))
        {
            onMoveDown = new MoveCommand(mc, Direction.Down);
        }
    }

    void FixedUpdate()
    {
        if (!Alive || mc == null)
        {
            return;
        }

        timeElapsed += Time.deltaTime;

        // go random direction every second
        if (timeElapsed >= moveTimeThreshold)
        {
            if (!Cloneable)
            {
                // --all you zombies--
                // wait... not cloneable equals cloned?
                Suffer(1);
            }

            timeElapsed = 0;

            // stay awhile and listen...
            isThinking = !isThinking;

            List<Direction> dirs = new List<Direction>();
            if (myLevel.CanIGo(this, Direction.Up))
            {
                dirs.Add(Direction.Up);
            }

            if (myLevel.CanIGo(this, Direction.Down))
            {
                dirs.Add(Direction.Down);
            }

            if (myLevel.CanIGo(this, Direction.Left))
            {
                dirs.Add(Direction.Left);
            }

            if (myLevel.CanIGo(this, Direction.Right))
            {
                dirs.Add(Direction.Right);
            }

            if (myLevel.CanIGo(this, Direction.TopLeft))
            {
                dirs.Add(Direction.TopLeft);
            }

            if (myLevel.CanIGo(this, Direction.TopRight))
            {
                dirs.Add(Direction.TopRight);
            }

            if (myLevel.CanIGo(this, Direction.BottomRight))
            {
                dirs.Add(Direction.BottomRight);
            }

            if (myLevel.CanIGo(this, Direction.BottomLeft))
            {
                dirs.Add(Direction.BottomLeft);
            }

            if (dirs.Count == 0)
            {
                // I'm suffocating!
                Suffer(5);
                return;
            }

            var rnd = Random.Range(0, dirs.Count);
            currentDirection = dirs[rnd];
        }

        if (isThinking) { return; }

        // We have to call Move() every frame
        switch (currentDirection)
        {
            case Direction.Up:
                onMoveUp.Execute();
                break;
            case Direction.Down:
                onMoveDown.Execute();
                break;
            case Direction.Left:
                onMoveLeft.Execute();
                break;
            case Direction.Right:
                onMoveRight.Execute();
                break;
            case Direction.TopLeft:
                mc.GoTopLeft();
                break;
            case Direction.TopRight:
                mc.GoTopRight();
                break;
            case Direction.BottomLeft:
                mc.GoBottomLeft();
                break;
            case Direction.BottomRight:
                mc.GoBottomRight();
                break;
        }
    }

    public override GameObject Clone()
    {
        if (!Cloneable)
        {
            return null;
        }

        // configuring GameObject
        GameObject result = GameObject.Instantiate(this.gameObject);
        result.GetComponent<SpriteRenderer>().enabled = true;

        // configuring Actor
        GenericEnemy ge = result.GetComponent<GenericEnemy>();
        ge.SetLevel(myLevel);
        myLevel.AddActor(ge);
        ge.SetMaxHealth(MaxHealth);
        if (Alive)
        {
            ge.Health = Health;
        }
        else
        {
            ge.Health = MaxHealth;
        }

        ge.Cloneable = false;
        ge.moveTimeThreshold = Random.Range(0.5f, 1.5f);
        ge.enabled = true;

        //configuring MovementController
        var geMc = result.GetComponent<MovementController>();
        geMc.movementSpeed = Random.Range(100, 300);

        return result;
    }
}
