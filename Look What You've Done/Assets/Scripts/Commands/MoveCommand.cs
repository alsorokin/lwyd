public class MoveCommand : Command {
    private MovementController movementController;
    private readonly MovementController.Direction direction;

    public MoveCommand (MovementController mc, MovementController.Direction dir)
    {
        movementController = mc;
        direction = dir;
    }

    override public void Execute()
    {
        if (!movementController.IsMoving(direction))
        {
            movementController.Go(direction);
        }
    }
}
