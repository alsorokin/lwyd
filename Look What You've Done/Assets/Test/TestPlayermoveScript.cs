using UnityEngine;

public class TestPlayermoveScript : MonoBehaviour
{
    // TODO: create a NullCommand
    public Command onMoveRight = null;
    public Command onMoveLeft = null;
    public Command onMoveUp = null;
    public Command onMoveDown = null;

    // Use this for initialization
    void Start() {
        MovementController mc = GetComponent<MovementController>();
        if (onMoveRight == null)
        {
            onMoveRight = new MoveCommand(mc, Direction.Right);
        }
        if (onMoveLeft == null)
        {
            onMoveLeft = new MoveCommand(mc, Direction.Left);
        }
        if (onMoveUp == null)
        {
            onMoveUp = new MoveCommand(mc, Direction.Up);
        }
        if (onMoveDown == null)
        {
            onMoveDown = new MoveCommand(mc, Direction.Down);
        }
    }
    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") > 0.001)
        {
            onMoveRight.Execute();
        }
        if (Input.GetAxis("Horizontal") < -0.001)
        {
            onMoveLeft.Execute();
        }
        if (Input.GetAxis("Vertical") > 0.001)
        {
            onMoveUp.Execute();
        }
        if (Input.GetAxis("Vertical") < -0.001)
        {
            onMoveDown.Execute();
        }
    }

    void OnGUI() {}
}
