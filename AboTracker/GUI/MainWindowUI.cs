using AboTracker.Logic;
using AboTracker.Model;
using Gtk;

namespace AboTracker.GUI;

public class MainWindowUi : Gtk.ApplicationWindow
{
    private readonly Box _mainContainer;
    
    public MainWindowUi(Application app)
    {
        AppSetup(app);
        
        _mainContainer = Box.New(Orientation.Vertical, 12);
        SetupContainerStructure();
        
        InputParser.InitializeArray();
        CreateElementsFromArray();
    }

    private void AppSetup(Application app)
    {
        // Create window and add Components:
        this.Application = app; 
        this.Title = "Simple Abo Tracker";
        this.SetDefaultSize(600, 400);
    }

    private void SetupContainerStructure()
    {
        _mainContainer.SetMarginTop(12);
        _mainContainer.SetMarginBottom(12);
        _mainContainer.SetMarginStart(12);
        _mainContainer.SetMarginEnd(12);
        this.SetChild(_mainContainer);
    }

    private void CreateElementsFromArray()
    {   
        foreach (Subscription s in InputParser.Subscriptions)
        {
            AddSubscriptionComponent(s);
        }
    }

    private void AddSubscriptionComponent(Subscription sub)
    {
        // Add a box:
        var box = Box.New(Gtk.Orientation.Vertical, 6);
        box.SetMarginTop(12);
        box.SetMarginBottom(12);
        box.SetMarginStart(12);
        box.SetMarginEnd(12);
            
        // Add a label and a button:
        var label = Label.New(sub.ToString()); // Use the sub data directly
        var button = Gtk.Button.NewWithLabel("Delete");
        box.Append(label);
        box.Append(button);
        
        // Add box to Window:
        _mainContainer.Append(box);
        
        // Make button clickable:
        button.OnClicked += (sender, e) => 
        {
            // TODO: Implement delete logic
            label.SetLabel("Abo was deleted!");
        };
    }
}