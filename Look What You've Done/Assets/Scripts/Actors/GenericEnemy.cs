using System.Collections.Generic;
using UnityEngine;
using Direction = MovementController.Direction;

class GenericEnemy : Actor
{
    public Command OnMoveRight = new NullCommand();
    public Command OnMoveLeft = new NullCommand();
    public Command OnMoveUp = new NullCommand();
    public Command OnMoveDown = new NullCommand();

    public float MoveTimeThreshold = 1f;

    private float _timeElapsed;
    private Direction _direction = Direction.None;
    private bool _isThinking;

    public override GameObject Clone()
    {
        if (!Cloneable) return null;

        // configuring GameObject
        GameObject result = Instantiate(gameObject);
        result.GetComponent<SpriteRenderer>().enabled = true;
        var rb = result.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;

        // configuring Actor
        GenericEnemy ge = result.GetComponent<GenericEnemy>();
        ge.SetLevel(CurrentLevel);
        CurrentLevel.AddActor(ge);
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
        ge.MoveTimeThreshold = Random.Range(0.5f, 1.5f);
        ge.enabled = true;

        //configuring MovementController
        var geMc = result.GetComponent<MovementController>();
        geMc.MovementSpeed = Random.Range(100, 300);

        return result;
    }

    protected override void Start()
    {
        base.Start();

        if (OnMoveRight.GetType() == typeof(NullCommand))
        {
            OnMoveRight = new MoveCommand(CurrentMovementController, Direction.Right);
        }

        if (OnMoveLeft.GetType() == typeof(NullCommand))
        {
            OnMoveLeft = new MoveCommand(CurrentMovementController, Direction.Left);
        }

        if (OnMoveUp.GetType() == typeof(NullCommand))
        {
            OnMoveUp = new MoveCommand(CurrentMovementController, Direction.Up);
        }

        if (OnMoveDown.GetType() == typeof(NullCommand))
        {
            OnMoveDown = new MoveCommand(CurrentMovementController, Direction.Down);
        }
    }

    void FixedUpdate()
    {
        if (!Alive || CurrentMovementController == null)
        {
            return;
        }

        _timeElapsed += Time.deltaTime;

        // go random direction every second
        if (_timeElapsed >= MoveTimeThreshold)
        {
            if (!Cloneable)
            {
                // --all you zombies--
                // wait... not cloneable equals cloned?
                Suffer(1);
            }

            _timeElapsed = 0;

            // stay awhile and listen...
            _isThinking = !_isThinking;

            List<Direction> dirs = new List<Direction>();
            dirs.Add(Direction.Up);
            dirs.Add(Direction.Down);
            dirs.Add(Direction.Left);
            dirs.Add(Direction.Right);
            dirs.Add(Direction.TopLeft);
            dirs.Add(Direction.TopRight);
            dirs.Add(Direction.BottomRight);
            dirs.Add(Direction.BottomLeft);

            var rnd = Random.Range(0, dirs.Count);
            _direction = dirs[rnd];
        }

        if (_isThinking) { return; }

        // We have to call Move() every frame
        switch (_direction)
        {
            case Direction.Up:
                OnMoveUp.Execute();
                break;
            case Direction.Down:
                OnMoveDown.Execute();
                break;
            case Direction.Left:
                OnMoveLeft.Execute();
                break;
            case Direction.Right:
                OnMoveRight.Execute();
                break;
            case Direction.TopLeft:
                CurrentMovementController.GoTopLeft();
                break;
            case Direction.TopRight:
                CurrentMovementController.GoTopRight();
                break;
            case Direction.BottomLeft:
                CurrentMovementController.GoBottomLeft();
                break;
            case Direction.BottomRight:
                CurrentMovementController.GoBottomRight();
                break;
        }
    }
}
