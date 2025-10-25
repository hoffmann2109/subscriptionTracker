namespace AboTracker.Logic;
public class MainWindowLogic
{
    public string CurrentMessage { get; private set; } = "Hi, I'm a simple Abo Tracker";
    
    public void UpdateMessage()
    {
        CurrentMessage = "You clicked the button!";
    }
}