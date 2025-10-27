using AboTracker.Logic;
using AboTracker.Model;
using Gtk;

namespace AboTracker.UI;

public class MainWindowUi : ApplicationWindow
{
    // Boxes:
    private readonly Box _rootBox;
    private readonly Box _subscriptionListContainer;
    private readonly Box _navigationContainer;
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
        _navigationContainer = Box.New(Orientation.Vertical, 6);
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
        SetupNavigationBar();
        
        _rootBox.Append(Separator.New(Orientation.Horizontal));
        
        _subscriptionListContainer.SetMarginTop(12);
        _subscriptionListContainer.SetMarginBottom(12);
        _subscriptionListContainer.SetMarginStart(12);
        _subscriptionListContainer.SetMarginEnd(12);
        _rootBox.Append(_subscriptionListContainer);
        
        _rootBox.Append(Separator.New(Orientation.Horizontal));
        
        _calculationContainer.SetMarginTop(12);
        _calculationContainer.SetMarginBottom(12);
        _calculationContainer.SetMarginStart(12);
        _calculationContainer.SetMarginEnd(12);
        _calculationContainer.Append(Label.New("Calculations are done here"));
        _rootBox.Append(_calculationContainer);
    }

    private void SetupNavigationBar()
    {
        _navigationContainer.SetMarginTop(12);
        _navigationContainer.SetMarginBottom(12);
        _navigationContainer.SetMarginStart(12);
        _navigationContainer.SetMarginEnd(12);
        var label = Label.New("Navigation is done here");
        _navigationContainer.Append(label);
        _rootBox.Append(_navigationContainer);
        
        var button = Button.NewWithLabel("Add");
        _navigationContainer.Append(button);
        
        button.OnClicked += (sender, e) => 
        {
            // TODO: Implement add logic
            label.SetLabel("Abo was added!");
            CreateAndShowAddDialog();
        };
    }

    private void CreateAndShowAddDialog()
    {
        
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
        // TODO: sort by due date
        
        // Add a box:
        var box = Box.New(Orientation.Horizontal, 6);
        box.SetMarginTop(12);
        box.SetMarginBottom(12);
        box.SetMarginStart(12);
        box.SetMarginEnd(12);
            
        // Add a label and a button:
        var label = Label.New(sub.ToString());
        label.SetHexpand(true);
        label.SetHalign(Align.Start);
        box.Append(label);
        
        var button = Button.NewFromIconName("user-trash-symbolic");
        button.SetTooltipText("Delete subscription");
        button.SetHexpand(false);
        box.Append(button);
        
        // Add box to Window:
        _subscriptionListContainer.Append(box);
        
        // Make button clickable:
        button.OnClicked += (sender, e) => 
        {
            var success = StorageManager.RemoveSubscriptionByName(sub.Name);
            
            if (success)
            {
                StorageManager.SaveListToJson();
                ReloadSubscriptionList();
            }
        };
    }
    
    private void ReloadSubscriptionList()
    {
        while (_subscriptionListContainer.GetFirstChild() is { } child)
        {
            _subscriptionListContainer.Remove(child);
        }
        
        StorageManager.InitializeArray();
        
        CreateElementsFromArray(StorageManager.Subscriptions);
    }
}