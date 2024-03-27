using UnityEngine.UI;
using EventSystem;

public class CalculatorButton : IEventCell
{
    public Button button;
    public string Index { get; set; }
    public string Text { get => button.GetComponentInChildren<Text>().text; }

    public CalculatorButton(string name, Button button)
    {
        Index = name;
        this.button = button;
    }
}
