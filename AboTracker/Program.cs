using Gtk;

namespace AboTracker;

internal static class AboTracker
{
    private static int Main(string[] args)
    {
        // Create a new GTK application
        var app = Gtk.Application.New("org.abo.tracker", Gio.ApplicationFlags.DefaultFlags);

        // Connect the "activate" signal to our startup method
        app.OnActivate += (sender, e) => {
            // Create a new window (no longer takes 'app' in constructor)
            var window = new ApplicationWindow();
            // Set the application property instead
            window.Application = app; 
            
            window.Title = "Simple Abo Tracker";
            window.SetDefaultSize(600, 400);

            // Create a label with your text
            var label = Label.New("Hi, I'm a simple Abo Tracker");
            
            // Add the label to the window
            window.SetChild(label);

            // Show the window
            window.Show();
        };

        // Run the application (Run method needs argc and argv)
        return app.Run(args.Length, args);
    }
}