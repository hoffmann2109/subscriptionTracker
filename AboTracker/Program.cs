using Gtk;

namespace AboTracker;

internal static class AboTracker
{
    private static int Main(string[] args)
    {
        var app = Gtk.Application.New("org.abo.tracker", Gio.ApplicationFlags.DefaultFlags);
        
        app.OnActivate += (sender, e) => {
            var window = new ApplicationWindow();
            window.Application = app; 
            window.Title = "Simple Abo Tracker";
            window.SetDefaultSize(600, 400);
            
            var label = Label.New("Hi, I'm a simple Abo Tracker");
            window.SetChild(label);
            
            window.Show();
        };
        
        return app.Run(args.Length, args);
    }
}