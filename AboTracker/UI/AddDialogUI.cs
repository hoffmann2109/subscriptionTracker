using AboTracker.Logic;
using AboTracker.Model;
using Gtk;

namespace AboTracker.UI;

public class AddDialogUi(Window parentWindow, Action onSubscriptionAdded)
{
    private Entry nameEntry;
    private Entry amountEntry;
    private Entry periodEntry;
    private Entry purchaseDateEntry;
    private Entry nextPaymentEntry;
    private Label errorLabel;
    
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
    
        // Add button
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
        
        dialog.Show();
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
        var name = nameEntry.GetText();
        var amountText = amountEntry.GetText();
        var period = periodEntry.GetText();
        var purchaseDate = purchaseDateEntry.GetText();
        var nextPaymentDate = nextPaymentEntry.GetText();
        
        // Validate inputs
        if (string.IsNullOrWhiteSpace(name))
        {
            errorLabel.SetLabel("Please enter a subscription name");
            errorLabel.SetVisible(true);
            return false;
        }
        
        if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
        {
            errorLabel.SetLabel("Please enter a valid amount (e.g., 9.99)");
            errorLabel.SetVisible(true);
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(period))
        {
            errorLabel.SetLabel("Please enter a payment period");
            errorLabel.SetVisible(true);
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(purchaseDate))
        {
            errorLabel.SetLabel("Please enter a purchase date");
            errorLabel.SetVisible(true);
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(nextPaymentDate))
        {
            errorLabel.SetLabel("Please enter a next payment date");
            errorLabel.SetVisible(true);
            return false;
        }
            
        if (!DateTime.TryParse(purchaseDate, out _))
        {
            errorLabel.SetLabel("Purchase date must be in valid format (YYYY-MM-DD)");
            errorLabel.SetVisible(true);
            return false;
        }
        
        if (!DateTime.TryParse(nextPaymentDate, out _))
        {
            errorLabel.SetLabel("Next payment date must be in valid format (YYYY-MM-DD)");
            errorLabel.SetVisible(true);
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
        nameEntry = Entry.New();
        nameEntry.SetPlaceholderText("e.g., Netflix, Spotify");
        mainBox.Append(nameEntry);
    
        // Amount:
        var amountLabel = Label.New("Amount:");
        amountLabel.SetHalign(Align.Start);
        mainBox.Append(amountLabel);
        amountEntry = Entry.New();
        amountEntry.SetPlaceholderText("e.g., 9.99");
        mainBox.Append(amountEntry);
    
        // Payment Period:
        var periodLabel = Label.New("Payment Period:");
        periodLabel.SetHalign(Align.Start);
        mainBox.Append(periodLabel);
        periodEntry = Entry.New();
        periodEntry.SetPlaceholderText("e.g., Monthly, Yearly");
        mainBox.Append(periodEntry);
    
        // Purchase Date:
        var purchaseDateLabel = Label.New("Purchase Date (YYYY-MM-DD):");
        purchaseDateLabel.SetHalign(Align.Start);
        mainBox.Append(purchaseDateLabel);
        purchaseDateEntry = Entry.New();
        purchaseDateEntry.SetPlaceholderText("e.g., 2024-01-15");
        mainBox.Append(purchaseDateEntry);
    
        // Next Payment Date:
        var nextPaymentLabel = Label.New("Next Payment Date (YYYY-MM-DD):");
        nextPaymentLabel.SetHalign(Align.Start);
        mainBox.Append(nextPaymentLabel);
        nextPaymentEntry = Entry.New();
        nextPaymentEntry.SetPlaceholderText("e.g., 2025-09-21");
        mainBox.Append(nextPaymentEntry);
    
        // Error label:
        errorLabel = Label.New("");
        errorLabel.AddCssClass("error");
        errorLabel.SetVisible(false);
        mainBox.Append(errorLabel);
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