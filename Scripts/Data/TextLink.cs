using Godot;
using System;

public class TextLink : Node
{
    [Export]
    private int optionsValue;
    private HSlider slider;
    private string previous;
    private TextEdit textBox;
    public override void _Ready()
    {
        slider = GetChild<HSlider>(0);
        textBox = GetChild<TextEdit>(1);
        textBox.Text = GameManager.GetPlayerValue(optionsValue).ToString();
        previous = textBox.Text;
        slider.Value = GameManager.GetPlayerValue(optionsValue);
        textBox.Connect("text_changed", this, nameof(TextChanged));
        Godot.Collections.Array passing = new Godot.Collections.Array();
        passing.Add(optionsValue);
        slider.Connect("value_changed", GameManager.Instance, nameof(GameManager.ChangeValueOf), passing);
        slider.Connect("value_changed", this, nameof(ChangeValueOfSlider));
    }

    public void TextChanged()
    {
        float holder;
        if (textBox.Text == "")
            return;
        if (float.TryParse(textBox.Text, out holder))
        {
            slider.Value = holder;
        }
    }

    public void ChangeValueOfSlider(float value)
    {
        if (value == float.Parse(textBox.Text))
            return;
        textBox.Text = value.ToString();
        previous = textBox.Text;
    }
}
