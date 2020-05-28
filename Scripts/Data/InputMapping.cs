using Godot;

///<summary>This is used to label buttons with what button is used for a certain action. it should be replaced with something that can change input as well</summary>
public class InputMapping : Node
{
    [Export]
    private string inputName = "Jump";
    public override void _Ready()
    {
        GetChild<Button>(0).Text = ((InputEvent)InputMap.GetActionList(inputName)[0]).AsText();
    }
}
