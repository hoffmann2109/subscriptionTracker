using Gtk;
using System.Text;

namespace AboTracker.UI;

public class HelpDialogUi(Window parentWindow)
{
    public void CreateAndShowHelpDialog()
    {
        var helpText = new StringBuilder();
        helpText.AppendLine("This app helps you track your subscriptions." +
                            " It was originally designed to monitor non-essential subscriptions (like streaming services) rather than fixed living costs (like rent), but it can be used for any recurring payment." +
                            " It calculates your total monthly cost and shows the next payment date for each item.");
        helpText.AppendLine();
        helpText.AppendLine("<b>Adding a New Subscription</b>");
        helpText.AppendLine("When you click 'Add', you'll need to provide:");
        helpText.AppendLine("  •  <b>Name:</b> A name");
        helpText.AppendLine("  •  <b>Amount:</b> The cost for one period (e.g., 15.99).");
        helpText.AppendLine("  •  <b>Payment Period:</b> How often you pay (Weekly, Monthly, Quarterly, or Yearly).");
        helpText.AppendLine("  •  <b>Purchase Date:</b> The date your subscription started. This is used to calculate all future payments.");

        var dialog = new MessageDialog
        {
            TransientFor = parentWindow, 
            Modal = true,
            DestroyWithParent = true,
            MessageType = MessageType.Info,
            Text = "About Simple Abo Tracker",
            SecondaryText = helpText.ToString(),
            SecondaryUseMarkup = true
        };
        
        dialog.AddButton("OK", (int)ResponseType.Ok);

        dialog.OnResponse += (sender, e) =>
        {
            dialog.Destroy();
        };

        dialog.Show();
    }
}