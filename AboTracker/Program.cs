using Gtk;

namespace AboTracker;

internal static class AboTracker
{
    private static int Main(string[] args)
    {
        var app = Gtk.Application.New("org.abo.tracker", Gio.ApplicationFlags.DefaultFlags);
        
        app.OnActivate += (sender, e) =>
        {
            var window = new MainWindow(app);
            window.Show();
        };
        
        return app.Run(args.Length, args);
    }
}