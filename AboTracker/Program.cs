using AboTracker.GUI;
using AboTracker.Logic;
using Gtk;

namespace AboTracker;

internal static class AboTracker
{
    private static int Main(string[] args)
    {
        var app = Gtk.Application.New("org.abo.tracker", Gio.ApplicationFlags.DefaultFlags);
        InputParser.InitializeArray();
        InputParser.PrintSubscriptions();
        app.OnActivate += (sender, e) =>
        {
            
            var window = new MainWindowUi(app);
            window.Show();
        };
        
        return app.Run(args.Length, args);
    }
}