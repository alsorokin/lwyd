public class GridMoveCommand : Command {
    private GridMovementController movementController;
    private MovementController.Direction direction;

    public GridMoveCommand (GridMovementController mc, MovementController.Direction dir)
    {
        movementController = mc;
        direction = dir;
    }

    override public void Execute()
    {
        movementController.Go(direction);
    }
}
