using AboTracker.Logic;
using AboTracker.Model;
using Gtk;

namespace AboTracker.UI;

public class EditDialogUi(Window parentWindow, Action onSubscriptionAdded)
{
    private AddDialogUi? _addDialogUi;
    
    public void CreateAndShowEditDialog(Subscription sub)
    {
        _addDialogUi = new AddDialogUi(parentWindow, onSubscriptionAdded);
        var dialog = _addDialogUi.WindowSetup();

        var mainBox = _addDialogUi.AddMainBox();
        dialog.SetChild(mainBox);

        _addDialogUi.AddFormFields(mainBox);
        
        var buttonBox = Box.New(Orientation.Horizontal, 6);
        buttonBox.SetHalign(Align.End);
        buttonBox.SetMarginTop(12);
        mainBox.Append(buttonBox);

        AddDialogUi.AddCancelButton(buttonBox, dialog);

        InsertEditButton(buttonBox, dialog, sub);

        _addDialogUi.SetFormFieldText(sub);
        
        dialog.Show();
    }
    
    private void InsertEditButton(Box buttonBox, Window dialog, Subscription sub)
    {
        var editButton = Button.NewWithLabel("Edit Subscription");
        editButton.AddCssClass("suggested-action");
        buttonBox.Append(editButton);

        editButton.OnClicked += (sender, e) =>
        {
            // Validate the inputs from the form:
            var updatedData = _addDialogUi?.ValidateAndExtractData();

            // If validation fails: error label
            if (updatedData == null)
            {
                return;
            }
            
            sub.Name = updatedData.Name;
            sub.Amount = updatedData.Amount;
            sub.PaymentPeriod = updatedData.PaymentPeriod;
            sub.PurchaseDate = updatedData.PurchaseDate;
            sub.NextPaymentDate = updatedData.NextPaymentDate;
            sub.Category = updatedData.Category;

            // Save the changes
            StorageManager.SaveListToJson();
            
            onSubscriptionAdded?.Invoke();
            
            dialog.Close();
        };
    }
}