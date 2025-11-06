using AboTracker.Model;
using Gtk;

namespace AboTracker.UI;

public class RemoveDialogUi(Window parentWindow, Action onSubscriptionRemoved, Subscription sub)
{
    public bool RemoveField { get; private set; }

    public void CreateAndShowRemoveDialog()
    {
        var dialog = WindowSetup();
        
        var mainBox = AddMainBox();
        dialog.SetChild(mainBox);
        
        // Display name
        var label = Label.New(sub.Name);
        label.SetHalign(Align.Center);
        mainBox.Append(label);
        
        // Button box
        var buttonBox = Box.New(Orientation.Horizontal, 12);
        buttonBox.SetHalign(Align.Center);
        buttonBox.SetMarginTop(12);
        mainBox.Append(buttonBox);
        
        AddCancelButton(buttonBox, dialog);

        InsertRemoveButton(buttonBox, dialog);
        
        dialog.Show();
    }
    
    private void InsertRemoveButton(Box buttonBox, Window dialog)
    {
        var addButton = Button.NewWithLabel("Remove");
        addButton.AddCssClass("suggested-action");
        buttonBox.Append(addButton);

        addButton.OnClicked += (sender, e) =>
        {
            RemoveField = true;
            onSubscriptionRemoved?.Invoke();
            dialog.Close();
        };
    }
    
    private Window WindowSetup()
    {
        var dialog = Window.New();
        dialog.SetTransientFor(parentWindow);
        dialog.SetModal(true);
        dialog.SetTitle("Remove Subscription");
        dialog.SetDefaultSize(150, 75);

        RemoveField = false;

        return dialog;
    }
    
    private static Box AddMainBox()
    {
        var mainBox = Box.New(Orientation.Vertical, 12);
        mainBox.SetMarginTop(12);
        mainBox.SetMarginBottom(12);
        mainBox.SetMarginStart(12);
        mainBox.SetMarginEnd(12);

        return mainBox;
    }
    
    private void AddCancelButton(Box buttonBox, Window dialog)
    {
        var cancelButton = Button.NewWithLabel("Cancel");
        buttonBox.Append(cancelButton);

        cancelButton.OnClicked += (sender, e) =>
        {
            RemoveField = false;
            dialog.Close();
        };
    }
    
}