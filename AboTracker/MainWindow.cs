using Gtk;

namespace AboTracker;

public class MainWindow : Gtk.ApplicationWindow
{
    public MainWindow(Gtk.Application app)
    {
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
        var label = Label.New("Hi, I'm a simple Abo Tracker");
        var button = Gtk.Button.NewWithLabel("Click Me!");
        box.Append(label);
        box.Append(button);
        
        // Add box to Window:
        this.SetChild(box);
        
        // Make button clickable:
        button.OnClicked += (sender, e) => 
        {
            label.SetLabel("You clicked the button!");
        };
    }
}