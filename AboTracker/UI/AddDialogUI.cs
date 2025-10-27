using AboTracker.Logic;
using AboTracker.Model;
using Gtk;

namespace AboTracker.UI;

public class AddDialogUi(Window parentWindow, Action onSubscriptionAdded)
{
    private Entry? _nameEntry;
    private Entry? _amountEntry;
    private DropDown? _periodEntry;
    private MenuButton? _purchaseDateButton;
    private Calendar? _purchaseDateCalendar;
    private Label? _errorLabel;

    private DateTime _selectedPurchaseDate = DateTime.Today;

    public void CreateAndShowAddDialog()
    {
        var dialog = WindowSetup();

        var mainBox = AddMainBox();
        dialog.SetChild(mainBox);

        AddFormFields(mainBox);

        // Button box
        var buttonBox = Box.New(Orientation.Horizontal, 6);
        buttonBox.SetHalign(Align.End);
        buttonBox.SetMarginTop(12);
        mainBox.Append(buttonBox);

        AddCancelButton(buttonBox, dialog);

        InsertAddButton(buttonBox, dialog);

        dialog.Show();
    }

    private void InsertAddButton(Box buttonBox, Window dialog)
    {
        var addButton = Button.NewWithLabel("Add Subscription");
        addButton.AddCssClass("suggested-action");
        buttonBox.Append(addButton);

        addButton.OnClicked += (sender, e) =>
        {
            // Only close and reload if validation is successful
            if (ValidateInputsAndCreateSubscription())
            {
                // Reload the list
                onSubscriptionAdded?.Invoke();
                dialog.Close();
            }
        };
    }

    private static void AddCancelButton(Box buttonBox, Window dialog)
    {
        var cancelButton = Button.NewWithLabel("Cancel");
        buttonBox.Append(cancelButton);

        cancelButton.OnClicked += (sender, e) => { dialog.Close(); };
    }

    private bool ValidateInputsAndCreateSubscription()
    {
        var name = _nameEntry?.GetText().Trim();
        var amountText = _amountEntry?.GetText();
        
        string? period = null;
        if (_periodEntry is not null && _periodEntry.Model is StringList stringList)
        {
            period = stringList.GetString(_periodEntry.Selected);
        }

        // Validation:
        if (string.IsNullOrWhiteSpace(name))
        {
            _errorLabel?.SetLabel("Please enter a subscription name");
            _errorLabel?.SetVisible(true);
            return false;
        }

        if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
        {
            _errorLabel?.SetLabel("Please enter a valid amount (e.g., 9.99)");
            _errorLabel?.SetVisible(true);
            return false;
        }

        if (string.IsNullOrWhiteSpace(period))
        {
            _errorLabel?.SetLabel("Please select a payment period");
            _errorLabel?.SetVisible(true);
            return false;
        }
        
        // Date Calculation:
        DateTime purchaseDate = _selectedPurchaseDate;
        DateTime nextPaymentDate;

        switch (period)
        {
            case "Monthly":
                nextPaymentDate = purchaseDate.AddMonths(1);
                break;
            case "Yearly":
                nextPaymentDate = purchaseDate.AddYears(1);
                break;
            case "Weekly":
                nextPaymentDate = purchaseDate.AddDays(7);
                break;
            case "Daily":
                nextPaymentDate = purchaseDate.AddDays(1);
                break;
            default:
                _errorLabel?.SetLabel("Invalid payment period selected.");
                _errorLabel?.SetVisible(true);
                return false;
        }
        
        var purchaseDateString = purchaseDate.ToString("yyyy-MM-dd");
        var nextPaymentDateString = nextPaymentDate.ToString("yyyy-MM-dd");

        CreateAndAddNewSubscription(name, amount, period, purchaseDateString, nextPaymentDateString);
        
        _errorLabel?.SetVisible(false);
        return true;
    }

    private static void CreateAndAddNewSubscription(string name, decimal amount, string period, string purchaseDate, string nextPaymentDate)
    {
        var newSubscription = new Subscription
        {
            Name = name,
            Amount = amount,
            PaymentPeriod = period,
            PurchaseDate = purchaseDate,
            NextPaymentDate = nextPaymentDate
        };

        StorageManager.Subscriptions.Add(newSubscription);
        StorageManager.SaveListToJson();
    }

    private void AddFormFields(Box mainBox)
    {
        // Name:
        var nameLabel = Label.New("Subscription Name:");
        nameLabel.SetHalign(Align.Start);
        mainBox.Append(nameLabel);
        _nameEntry = Entry.New();
        _nameEntry.SetPlaceholderText("e.g., Netflix, Spotify");
        mainBox.Append(_nameEntry);

        // Amount:
        var amountLabel = Label.New("Amount:");
        amountLabel.SetHalign(Align.Start);
        mainBox.Append(amountLabel);
        _amountEntry = Entry.New();
        _amountEntry.SetPlaceholderText("e.g., 9.99");
        mainBox.Append(_amountEntry);

        // Payment Period:
        var periodLabel = Label.New("Payment Period:");
        periodLabel.SetHalign(Align.Start);
        mainBox.Append(periodLabel);
        _periodEntry = DropDown.NewFromStrings(["Daily", "Weekly", "Monthly", "Yearly"]);
        _periodEntry.SetSelected(0); // Default to "Monthly"
        mainBox.Append(_periodEntry);

        // Purchase Date:
        var purchaseDateLabel = Label.New("Purchase Date:");
        purchaseDateLabel.SetHalign(Align.Start);
        mainBox.Append(purchaseDateLabel);

        _purchaseDateCalendar = Calendar.New();
        
        _purchaseDateButton = MenuButton.New();
        _purchaseDateButton.SetLabel(_selectedPurchaseDate.ToString("yyyy-MM-dd"));
        mainBox.Append(_purchaseDateButton);

        var purchaseDatePopover = Popover.New();
        purchaseDatePopover.SetChild(_purchaseDateCalendar);
        
        if (_purchaseDateButton is not null)
            _purchaseDateButton.Popover = purchaseDatePopover;

        _purchaseDateCalendar.OnDaySelected += (sender, e) =>
        {
            GLib.DateTime selected = _purchaseDateCalendar.GetDate();
            
            _selectedPurchaseDate = new System.DateTime(selected.GetYear(), selected.GetMonth(), selected.GetDayOfMonth());

            _purchaseDateButton?.SetLabel(_selectedPurchaseDate.ToString("yyyy-MM-dd"));
            purchaseDatePopover.Hide();
        };
        
        // Error label:
        _errorLabel = Label.New("");
        _errorLabel.AddCssClass("error");
        _errorLabel.SetVisible(false);
        mainBox.Append(_errorLabel);
    }

    private Window WindowSetup()
    {
        var dialog = Window.New();
        dialog.SetTransientFor(parentWindow);
        dialog.SetModal(true);
        dialog.SetTitle("Add Subscription");
        dialog.SetDefaultSize(400, 350); 

        return dialog;
    }

    private Box AddMainBox()
    {
        var mainBox = Box.New(Orientation.Vertical, 12);
        mainBox.SetMarginTop(12);
        mainBox.SetMarginBottom(12);
        mainBox.SetMarginStart(12);
        mainBox.SetMarginEnd(12);

        return mainBox;
    }
}