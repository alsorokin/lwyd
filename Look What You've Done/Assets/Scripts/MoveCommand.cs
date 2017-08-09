using UnityEngine;

public class MoveCommand : Command {
    private MovementController movementController;
    private Direction direction;

    public MoveCommand (MovementController mc, Direction dir)
    {
        movementController = mc;
        direction = dir;
    }

    override public void Execute()
    {
        movementController.Go(direction);
    }
}