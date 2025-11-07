using System.Globalization;
using AboTracker.Logic;
using AboTracker.Model;
using Gtk;
using Calendar = Gtk.Calendar;

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
    private Entry? _categoryEntry;
    private readonly string[] _periods = ["Daily", "Weekly", "Quarterly", "Monthly", "Yearly"];

    public void CreateAndShowAddDialog()
    {
        var dialog = WindowSetup();

        var mainBox = AddMainBox();
        dialog.SetChild(mainBox);

        AddFormFields(mainBox);
        
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
    
    internal static void AddCancelButton(Box buttonBox, Window dialog)
    {
        var cancelButton = Button.NewWithLabel("Cancel");
        buttonBox.Append(cancelButton);

        cancelButton.OnClicked += (sender, e) => { dialog.Close(); };
    }
    
    internal Subscription? ValidateAndExtractData()
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
            return null;
        }

        if (!decimal.TryParse(amountText, out var amount) || amount <= 0)
        {
            _errorLabel?.SetLabel("Please enter a valid amount (e.g., 9.99)");
            _errorLabel?.SetVisible(true);
            return null;
        }

        if (string.IsNullOrWhiteSpace(period))
        {
            _errorLabel?.SetLabel("Please select a payment period");
            _errorLabel?.SetVisible(true);
            return null;
        }
        
        // Date Calculation:
        var purchaseDate = _selectedPurchaseDate;
        var dateToday = DateTime.Now.Date;
        DateTime nextPaymentDate = purchaseDate;

        while (nextPaymentDate < dateToday)
        {
            switch (period)
            {
                case "Monthly":
                    nextPaymentDate = nextPaymentDate.AddMonths(1);
                    break;
                case "Yearly":
                    nextPaymentDate = nextPaymentDate.AddYears(1);
                    break;
                case "Quarterly":
                    nextPaymentDate = nextPaymentDate.AddMonths(3);
                    break;
                case "Weekly":
                    nextPaymentDate = nextPaymentDate.AddDays(7);
                    break;
                case "Daily":
                    nextPaymentDate = nextPaymentDate.AddDays(1);
                    break;
                default:
                    _errorLabel?.SetLabel("Invalid payment period selected.");
                    _errorLabel?.SetVisible(true);
                    return null;
            }
        }

        var purchaseDateString = purchaseDate.ToString("dd.MM.yyyy");
        var nextPaymentDateString = nextPaymentDate.ToString("dd.MM.yyyy");
        
        var category = _categoryEntry?.GetText().Trim();
        
        _errorLabel?.SetVisible(false);

        // Return a new subscription object:
        return new Subscription
        {
            Name = name,
            Amount = amount,
            PaymentPeriod = period,
            PurchaseDate = purchaseDateString,
            NextPaymentDate = nextPaymentDateString,
            Category = category
        };
    }

    private bool ValidateInputsAndCreateSubscription()
    {
        var newSubData = ValidateAndExtractData();
        if (newSubData == null)
        {
            return false; // Validation failed
        }
        
        // Validation was successful
        CreateAndAddNewSubscription(newSubData.Name, newSubData.Amount, newSubData.PaymentPeriod, 
            newSubData.PurchaseDate, newSubData.NextPaymentDate, newSubData.Category);
        
        return true;
    }

    private static void CreateAndAddNewSubscription(string name, decimal amount, string period, string purchaseDate, string nextPaymentDate, string category)
    {
        var newSubscription = new Subscription
        {
            Name = name,
            Amount = amount,
            PaymentPeriod = period,
            PurchaseDate = purchaseDate,
            NextPaymentDate = nextPaymentDate,
            Category = category
        };

        StorageManager.Subscriptions.Add(newSubscription);
        StorageManager.SaveListToJson();
    }

    internal void AddFormFields(Box mainBox)
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
        _periodEntry = DropDown.NewFromStrings(_periods);
        _periodEntry.SetSelected(0); // Default to "Monthly"
        mainBox.Append(_periodEntry);

        // Purchase Date:
        var purchaseDateLabel = Label.New("Purchase Date:");
        purchaseDateLabel.SetHalign(Align.Start);
        mainBox.Append(purchaseDateLabel);
        _purchaseDateCalendar = Calendar.New();
        
        _purchaseDateButton = MenuButton.New();
        _purchaseDateButton.SetLabel(_selectedPurchaseDate.ToString("dd.MM.yyyy"));
        mainBox.Append(_purchaseDateButton);

        var purchaseDatePopover = Popover.New();
        purchaseDatePopover.SetChild(_purchaseDateCalendar);
        
        if (_purchaseDateButton is not null)
            _purchaseDateButton.Popover = purchaseDatePopover;

        _purchaseDateCalendar.OnDaySelected += (sender, e) =>
        {
            var selected = _purchaseDateCalendar.GetDate();
            
            _selectedPurchaseDate = new System.DateTime(selected.GetYear(), selected.GetMonth(), selected.GetDayOfMonth());

            _purchaseDateButton?.SetLabel(_selectedPurchaseDate.ToString("dd.MM.yyyy"));
            purchaseDatePopover.Hide();
        };
        
        // Category:
        var categoryLabel = Label.New("Category:");
        categoryLabel.SetHalign(Align.Start);
        mainBox.Append(categoryLabel);
        _categoryEntry = Entry.New();
        _categoryEntry.SetPlaceholderText("e.g., Entertainment, News, Utility");
        mainBox.Append(_categoryEntry);
        
        // Error label:
        _errorLabel = Label.New("");
        _errorLabel.AddCssClass("error");
        _errorLabel.SetVisible(false);
        mainBox.Append(_errorLabel);
    }

    internal Window WindowSetup()
    {
        var dialog = Window.New();
        dialog.SetTransientFor(parentWindow);
        dialog.SetModal(true);
        dialog.SetTitle("Add Subscription");
        dialog.SetDefaultSize(400, 350); 

        return dialog;
    }

    internal Box AddMainBox()
    {
        var mainBox = Box.New(Orientation.Vertical, 12);
        mainBox.SetMarginTop(12);
        mainBox.SetMarginBottom(12);
        mainBox.SetMarginStart(12);
        mainBox.SetMarginEnd(12);

        return mainBox;
    }
    
    internal void SetFormFieldText(Subscription sub)
    {
        _nameEntry?.SetText(sub.Name);
        _amountEntry?.SetText(sub.Amount.ToString(CultureInfo.InvariantCulture));
        _categoryEntry?.SetText(sub.Category);
        
        // Set period:
        int periodIndex = Array.IndexOf(_periods, sub.PaymentPeriod);
        
        if (periodIndex == -1)
        {
            periodIndex = 0; 
        }
        _periodEntry?.SetSelected((uint)periodIndex);

        // Set Purchase Date:
        if (DateTime.TryParseExact(sub.PurchaseDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var purchaseDate))
        {
            _selectedPurchaseDate = purchaseDate;
            _purchaseDateButton?.SetLabel(_selectedPurchaseDate.ToString("dd.MM.yyyy"));
            
            var gDateTime = GLib.DateTime.NewLocal(purchaseDate.Year, purchaseDate.Month, purchaseDate.Day, 0, 0, 0);
            if (gDateTime != null) _purchaseDateCalendar?.SelectDay(gDateTime);
        }
    }
}