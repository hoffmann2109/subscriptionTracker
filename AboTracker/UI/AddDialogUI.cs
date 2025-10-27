using AboTracker.Logic;
using AboTracker.Model;
using Gtk;

namespace AboTracker.UI;

public class AddDialogUi(Window parentWindow, Action onSubscriptionAdded)
{
    private Entry? _nameEntry;
    private Entry? _amountEntry;
    private Entry? _periodEntry;
    private Entry? _purchaseDateEntry;
    private Entry? _nextPaymentEntry;
    private Label? _errorLabel;
    
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
            ValidateInputsAndCreateSubscription();
        
            // Reload the list
            onSubscriptionAdded?.Invoke();
            
            dialog.Close();
        };
    }

    private static void AddCancelButton(Box buttonBox, Window dialog)
    {
        // Cancel button
        var cancelButton = Button.NewWithLabel("Cancel");
        buttonBox.Append(cancelButton);
    
        cancelButton.OnClicked += (sender, e) => 
        {
            dialog.Close();
        };
    }

    private bool ValidateInputsAndCreateSubscription()
    {
        var name = _nameEntry?.GetText();
        var amountText = _amountEntry?.GetText();
        var period = _periodEntry?.GetText();
        var purchaseDate = _purchaseDateEntry?.GetText();
        var nextPaymentDate = _nextPaymentEntry?.GetText();
        
        // Validate inputs
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
            _errorLabel?.SetLabel("Please enter a payment period");
            _errorLabel?.SetVisible(true);
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(purchaseDate))
        {
            _errorLabel?.SetLabel("Please enter a purchase date");
            _errorLabel?.SetVisible(true);
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(nextPaymentDate))
        {
            _errorLabel?.SetLabel("Please enter a next payment date");
            _errorLabel?.SetVisible(true);
            return false;
        }
            
        if (!DateTime.TryParse(purchaseDate, out _))
        {
            _errorLabel?.SetLabel("Purchase date must be in valid format (YYYY-MM-DD)");
            _errorLabel?.SetVisible(true);
            return false;
        }
        
        if (!DateTime.TryParse(nextPaymentDate, out _))
        {
            _errorLabel?.SetLabel("Next payment date must be in valid format (YYYY-MM-DD)");
            _errorLabel?.SetVisible(true);
            return false;
        }
        
        CreateAndAddNewSubscription(name, amount,  period, purchaseDate, nextPaymentDate);

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
        _periodEntry = Entry.New();
        _periodEntry.SetPlaceholderText("e.g., Monthly, Yearly");
        mainBox.Append(_periodEntry);
    
        // Purchase Date:
        var purchaseDateLabel = Label.New("Purchase Date (YYYY-MM-DD):");
        purchaseDateLabel.SetHalign(Align.Start);
        mainBox.Append(purchaseDateLabel);
        _purchaseDateEntry = Entry.New();
        _purchaseDateEntry.SetPlaceholderText("e.g., 2024-01-15");
        mainBox.Append(_purchaseDateEntry);
    
        // Next Payment Date:
        var nextPaymentLabel = Label.New("Next Payment Date (YYYY-MM-DD):");
        nextPaymentLabel.SetHalign(Align.Start);
        mainBox.Append(nextPaymentLabel);
        _nextPaymentEntry = Entry.New();
        _nextPaymentEntry.SetPlaceholderText("e.g., 2025-09-21");
        mainBox.Append(_nextPaymentEntry);
    
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