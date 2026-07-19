using Gdk;
using Gtk;

namespace AboTracker.UI;

public static class StyleManager
{
    public static void LoadGlobalCss(Display display)
    {
        // 1. Force GTK to prefer the Dark Theme variant globally
        var settings = Gtk.Settings.GetDefault();
        if (settings != null)
        {
            // Try setting the property directly. If your wrapper version uses 
            // the string variant, use: settings.SetProperty("gtk-application-prefer-dark-theme", new GLib.Value(true));
            settings.GtkApplicationPreferDarkTheme = true;
        }

        var cssProvider = CssProvider.New();
        
        // 2. Catppuccin Macchiato Color Palette Injection
        string allAppCss = """
                           /* --- Base Application Background & Typography --- */
                           window {
                               background-color: #24273a; /* Base */
                               color: #cad3f5;            /* Text */
                           }

                           label {
                               color: #cad3f5;            /* Text */
                           }

                           /* --- Input Fields (Search Bar, etc.) --- */
                           entry {
                               background-color: #1e2030; /* Mantle */
                               color: #cad3f5;            /* Text */
                               border: 1px solid #494d64; /* Surface1 */
                               border-radius: 5px;
                               padding: 6px;
                           }
                           
                           entry:focus {
                               border-color: #8aadf4;     /* Blue accent on focus */
                           }

                           /* --- Button Styles (Subtle Variant) --- */
                           button.add-button-custom {
                               background-color: #363a4f; /* Surface0 (Matches dropdown) */
                               border: 1px solid #494d64; /* Surface1 */
                               border-radius: 5px;
                               font-weight: bold;
                           }
                           
                           button.add-button-custom label {
                               color: #8bd5ca; /* Teal Text */
                           }
                           
                           button.add-button-custom:hover {
                               background-color: #494d64; /* Surface1 */
                               border-color: #8bd5ca;     /* Teal border on hover */
                           }

                           /* --- Category Indicator Styles --- */
                           .category-indicator {
                               background-color: #6e738d; /* Overlay0 (Default gray) */
                               border-radius: 2px;
                           }

                           .category-entertainment {
                               background-color: #ed8796; /* Red */
                           }

                           .category-news {
                               background-color: #8aadf4; /* Blue */
                           }

                           .category-utility {
                               background-color: #a6da95; /* Green */
                           }

                           .category-sports {
                               background-color: #eed49f; /* Yellow */
                           }

                           /* --- Subscription Card Entries --- */
                           .subscription-entry {
                               background-color: #363a4f; /* Surface0 */
                               border: 1px solid #494d64; /* Surface1 */
                               border-radius: 6px;
                               padding: 10px;
                           }
                           """;
        
        cssProvider.LoadFromData(allAppCss, -1);
        
        StyleContext.AddProviderForDisplay(
            display, 
            cssProvider, 
            800 // Style priority
        );
    }
}