using AboTracker.Logic;
using AboTracker.Model;
using GLib;
using Gtk;
using Application = Gtk.Application;

namespace AboTracker.UI;

public class MainWindowUi : ApplicationWindow
{
    // Boxes:
    private readonly Box _rootBox;
    private readonly Box _subscriptionListContainer;
    private readonly Box _navigationContainer;
    private readonly Box _calculationContainer;
    private readonly Label _monthlyCostLabel = Label.New("?");
    private readonly ComboBoxText _sortComboBox;
    
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
        _navigationContainer = Box.New(Orientation.Horizontal, 12);
        _sortComboBox = ComboBoxText.New();
        
        var enumerable = subscriptions as Subscription[] ?? subscriptions.ToArray();
        
        CalculateUtility.ComputeNextPaymentDate(enumerable);
        SetupContainerStructure(enumerable);
        CreateElementsFromArray(enumerable);
    }

    private void AppUiSetup(Application app)
    {
        this.Application = app; 
        this.Title = "Simple Abo Tracker";
        this.SetDefaultSize(550, 650);
        StyleManager.LoadGlobalCss(this.GetDisplay());
    }

    // Set layout and add all boxes to main box
    private void SetupContainerStructure(IEnumerable<Subscription> subscriptions)
    {
        SetupNavigationBar();
        
        _rootBox.Append(Separator.New(Orientation.Horizontal));
        
        var subscriptionScroller = ScrolledWindow.New();
        subscriptionScroller.SetPolicy(PolicyType.Never, PolicyType.Automatic);
        
        subscriptionScroller.SetVexpand(true);
        subscriptionScroller.SetMarginTop(12);
        subscriptionScroller.SetMarginBottom(12);
        subscriptionScroller.SetMarginStart(12);
        subscriptionScroller.SetMarginEnd(12);
        subscriptionScroller.SetChild(_subscriptionListContainer);
        
        _rootBox.Append(subscriptionScroller);
        _rootBox.Append(Separator.New(Orientation.Horizontal));
        
        _calculationContainer.SetMarginTop(12);
        _calculationContainer.SetMarginBottom(12);
        _calculationContainer.SetMarginStart(12);
        _calculationContainer.SetMarginEnd(12);
        
        var initialSum = Math.Round(CalculateUtility.CalculateTotalMonthlySum(subscriptions), 2);
        var initialCostText = "Average Monthly Sum: " + "€" + initialSum;
        _monthlyCostLabel.SetMarkup($"<b><big>{Markup.EscapeText(initialCostText)}</big></b>");
        _monthlyCostLabel.SetUseMarkup(true);
        
        _calculationContainer.Append(_monthlyCostLabel);
        _rootBox.Append(_calculationContainer);
    }
    
    private void SetupNavigationBar()
    {
        _navigationContainer.SetMarginTop(12);
        _navigationContainer.SetMarginBottom(12);
        _navigationContainer.SetMarginStart(12);
        _navigationContainer.SetMarginEnd(12);
        _rootBox.Append(_navigationContainer);
        
        CreateSortingBox();
        CreateAndStyleAddButton();

        var infoButton = Button.NewFromIconName("dialog-information-symbolic");
        _navigationContainer.Append(infoButton);

        infoButton.OnClicked += (sender, e) =>
        {
            var helpDialog = new HelpDialogUi(this);
            helpDialog.CreateAndShowHelpDialog();
        };
    }

    private void CreateAndStyleAddButton()
    {
        var addButton = Button.NewWithLabel("Add");
        addButton.AddCssClass("add-button-custom");
        
        _navigationContainer.Append(addButton);
        
        addButton.OnClicked += (sender, e) => 
        {
            var addDialog = new AddDialogUi(this, ReloadSubscriptionList);
            addDialog.CreateAndShowAddDialog();
        };
    }

    private void CreateSortingBox()
    {
        // Sort ComboBox:
        _sortComboBox.AppendText("Sort by... (Default: Input order)");
        _sortComboBox.AppendText("Name (A-Z)");
        _sortComboBox.AppendText("Amount (High-Low)");
        _sortComboBox.AppendText("Amount (Low-High)");
        _sortComboBox.AppendText("Next Payment (Soonest)");
        _sortComboBox.AppendText("Purchase Date (Newest)");
        _sortComboBox.AppendText("Payment Period");
        _sortComboBox.AppendText("Category (A-Z)");
        _sortComboBox.SetHexpand(true);
        _sortComboBox.SetActive(0);
        _sortComboBox.OnChanged += OnSortChanged;
        _navigationContainer.Append(_sortComboBox);
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
        var box = Box.New(Orientation.Horizontal, 6);
        box.SetMarginTop(12);
        box.SetMarginBottom(12);
        box.SetMarginStart(12);
        box.SetMarginEnd(12);
        
        var categoryIndicator = Box.New(Orientation.Vertical, 0);
        categoryIndicator.SetVexpand(true);
        categoryIndicator.SetValign(Align.Fill);
        categoryIndicator.SetSizeRequest(5, -1);
        categoryIndicator.SetMarginEnd(6);

        // Sanitize category name for use as a CSS class
        // (e.g., "Video Streaming" -> "category-video-streaming")
        var categoryCssClass = "category-" + System.Text.RegularExpressions.Regex.Replace(
            sub.Category.ToLower(), 
            @"[^a-z0-9]+", 
            "-"
        ).Trim('-');
        
        categoryIndicator.AddCssClass("category-indicator");
        categoryIndicator.AddCssClass(categoryCssClass);
        
        box.Append(categoryIndicator);
        
        // Add a label and a button:
        var textBox = Box.New(Orientation.Vertical, 2);
        textBox.SetHexpand(true);
        textBox.SetHalign(Align.Start);
        
        var nameLabel = Label.New(null);
        string nameMarkup = $"<b>{Markup.EscapeText(sub.Name + " (" + GeneralUtility.ToUpperFirst(sub.Category) + ")")}</b>";
        nameLabel.SetMarkup(nameMarkup);
        nameLabel.SetUseMarkup(true);
        nameLabel.SetHalign(Align.Start);
        
        string detailsText = $"€{CalculateUtility.CalculateMonthlySum(sub)} per month, Paid: ({sub.PaymentPeriod}) - Due: {sub.NextPaymentDate}";
        var detailsLabel = Label.New(detailsText);
        detailsLabel.SetHalign(Align.Start);
        
        textBox.Append(nameLabel);
        textBox.Append(detailsLabel);
        
        box.Append(textBox);
        
        var deleteButton = Button.NewFromIconName("user-trash-symbolic");
        deleteButton.SetTooltipText("Delete subscription");
        deleteButton.SetHexpand(false);
        box.Append(deleteButton);
        
        var editButton = Button.NewFromIconName("edit-symbolic");
        editButton.SetTooltipText("Edit subscription");
        editButton.SetHexpand(false);
        box.Append(editButton);
        
        // Add box to Window:
        _subscriptionListContainer.Append(box);
        
        // Make button clickable:
        deleteButton.OnClicked += (sender, e) => 
        {
            Action removeAndReloadAction = () =>
            {
                var success = StorageManager.RemoveSubscriptionByName(sub.Name);
        
                if (success)
                {
                    StorageManager.SaveListToJson();
                    ReloadSubscriptionList();
                }
            };
            
            var removeDialog = new RemoveDialogUi(this, removeAndReloadAction, sub);
            removeDialog.CreateAndShowRemoveDialog();
        };
        
        editButton.OnClicked +=  (sender, e) =>
        {
            var editDialog = new EditDialogUi(this, ReloadSubscriptionList);
            editDialog.CreateAndShowEditDialog(sub);
        };
    }
    
    private void OnSortChanged(object? sender, EventArgs e)
    {
        var activeSort = _sortComboBox.GetActiveText();
        
        if (_sortComboBox.GetActive() == 0)
        {
            while (_subscriptionListContainer.GetFirstChild() is { } child)
            {
                _subscriptionListContainer.Remove(child);
            }
            CreateElementsFromArray(StorageManager.Subscriptions);
            return;
        }

        var listToSort = StorageManager.Subscriptions;
        var sortedList = CalculateUtility.SortList(listToSort, activeSort);
        
        while (_subscriptionListContainer.GetFirstChild() is { } child)
        {
            _subscriptionListContainer.Remove(child);
        }
        
        CreateElementsFromArray(sortedList);
    }

    private void ReloadSubscriptionList()
    {
        while (_subscriptionListContainer.GetFirstChild() is { } child)
        {
            _subscriptionListContainer.Remove(child);
        }
        
        StorageManager.InitializeArray();
        
        CreateElementsFromArray(StorageManager.Subscriptions);
        
        var newSum = Math.Round(CalculateUtility.CalculateTotalMonthlySum(StorageManager.Subscriptions), 2);
        var newCostText = "Average Monthly Sum: " + "€" + newSum;
        _monthlyCostLabel.SetMarkup($"<b><big>{Markup.EscapeText(newCostText)}</big></b>");
        
        _sortComboBox.SetActive(0);
    }
}