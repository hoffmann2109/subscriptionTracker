using AboTracker.Logic;
using Gtk;

namespace AboTracker.GUI;

public class MainWindowUi : Gtk.ApplicationWindow
{
    private readonly MainWindowLogic _logic;
    
    public MainWindowUi(Gtk.Application app)
    {
        _logic = new MainWindowLogic();
        // Create window and add Components:
        this.Application = app; 
        this.Title = "Simple Abo Tracker";
        this.SetDefaultSize(600, 400);
        AddComponents();
    }

    private void AddComponents()
    {
        // Add a box:
        var box = Gtk.Box.New(Gtk.Orientation.Vertical, 12);
        box.SetMarginTop(12);
        box.SetMarginBottom(12);
        box.SetMarginStart(12);
        box.SetMarginEnd(12);
            
        // Add a label and a button:
        var label = Label.New(_logic.CurrentMessage);
        var button = Gtk.Button.NewWithLabel("Click Me!");
        box.Append(label);
        box.Append(button);
        
        // Add box to Window:
        this.SetChild(box);
        
        // Make button clickable:
        button.OnClicked += (sender, e) => 
        {
            _logic.UpdateMessage();
            label.SetLabel(_logic.CurrentMessage);
        };
    }
}