using Godot;
using System;

public class InputHandler : Node
{
    [Signal]
    public delegate void MouseMoved(Vector2 inputting);
    private bool[] inputs = new bool[20];
    public static InputHandler Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _Process(float delta)
    {
        inputs[(int)Keys.moveForward] = Input.IsActionPressed("MoveForward");
        inputs[(int)Keys.moveBack] = Input.IsActionPressed("MoveBack");
        inputs[(int)Keys.moveLeft] = Input.IsActionPressed("MoveLeft");
        inputs[(int)Keys.moveRight] = Input.IsActionPressed("MoveRight");
        inputs[(int)Keys.jump] = Input.IsActionJustPressed("Jump");
        inputs[(int)Keys.crouch] = Input.IsActionJustPressed("Crouch");
        inputs[(int)Keys.sprint] = Input.IsActionPressed("Sprint");
        inputs[(int)Keys.hitting] = Input.IsActionJustPressed("Hit");
        inputs[(int)Keys.throwing] = Input.IsActionJustPressed("Throwing");
        inputs[(int)Keys.gliding] = Input.IsActionPressed("Jump");
        inputs[(int)Keys.strafe] = Input.IsActionJustPressed("Strafe");
        inputs[(int)Keys.throwing] = Input.IsActionJustPressed("Throwing");
        inputs[(int)Keys.hitting] = Input.IsActionJustPressed("Hit");

    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            EmitSignal(nameof(MouseMoved), motion.Relative);
        }
    }

    public bool GetInput(Keys key)
    {
        return inputs[(int)key];
    }

    public void ConnectToMouseMovement(Godot.Object target, string name)
    {
        Connect(nameof(InputHandler.MouseMoved), target, name);
    }
}

public enum Keys
{
    moveForward,
    moveBack,
    moveLeft,
    moveRight,
    jump,
    crouch,
    sprint,
    throwing,
    hitting,
    gliding,
    strafe
}