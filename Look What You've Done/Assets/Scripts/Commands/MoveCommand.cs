public class MoveCommand : Command
{
    private MovementController _movementController;
    private readonly MovementController.Direction _direction;

    public MoveCommand(MovementController mc, MovementController.Direction dir)
    {
        _movementController = mc;
        _direction = dir;
    }

    override public void Execute()
    {
        if (!_movementController.IsMovingInDirection(_direction))
        {
            _movementController.Go(_direction);
        }
    }
}
