using AboTracker.Logic;
using AboTracker.Model;
using Gtk;

namespace AboTracker.GUI;

public class MainWindowUi : Gtk.ApplicationWindow
{
    // Boxes:
    private readonly Box _rootBox;
    private readonly Box _subscriptionListContainer;
    private readonly Box _calculationContainer;
    
    // Main Window:
    public MainWindowUi(Application app, IEnumerable<Subscription> subscriptions)
    {
        AppUiSetup(app);
        
        // Main Box:
        _rootBox = Box.New(Orientation.Vertical, 6);
        this.SetChild(_rootBox);
        
        // Further Boxes:
        _subscriptionListContainer = Box.New(Orientation.Vertical, 12);
        _calculationContainer = Box.New(Orientation.Vertical, 6);
        SetupContainerStructure();
        CreateElementsFromArray(subscriptions);
    }

    private void AppUiSetup(Application app)
    {
        this.Application = app; 
        this.Title = "Simple Abo Tracker";
        this.SetDefaultSize(600, 400);
    }

    // Set layout and add all boxes to main box
    private void SetupContainerStructure()
    {
        _subscriptionListContainer.SetMarginTop(12);
        _subscriptionListContainer.SetMarginBottom(12);
        _subscriptionListContainer.SetMarginStart(12);
        _subscriptionListContainer.SetMarginEnd(12);
        _rootBox.Append(_subscriptionListContainer);
        
        var separator = Separator.New(Gtk.Orientation.Horizontal);
        _rootBox.Append(separator);
        
        _calculationContainer.SetMarginTop(12);
        _calculationContainer.SetMarginBottom(12);
        _calculationContainer.SetMarginStart(12);
        _calculationContainer.SetMarginEnd(12);
        var label = Label.New("Calculations are done here");
        _calculationContainer.Append(label);
        _rootBox.Append(_calculationContainer);
    }

    // Create Boxes from Subscription Elements:
    private void CreateElementsFromArray(IEnumerable<Subscription> subscriptions)
    {   
        foreach (Subscription s in subscriptions)
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
        _subscriptionListContainer.Append(box);
        
        // Make button clickable:
        button.OnClicked += (sender, e) => 
        {
            // TODO: Implement delete logic
            label.SetLabel("Abo was deleted!");
        };
    }
}