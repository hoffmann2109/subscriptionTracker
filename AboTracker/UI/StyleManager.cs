using Gdk;
using Gtk;

namespace AboTracker.UI;

public static class StyleManager
{
    public static void LoadGlobalCss(Display display)
    {
        var cssProvider = CssProvider.New();
        string allAppCss = """
                           /* --- Button Styles --- */
                           .add-button-custom {
                               background-image: none;
                               background-color: #80B4B3;
                               color: #313744;
                               border-radius: 5px;
                           }

                           .add-button-custom:hover {
                               background-color: #6DA4A3;
                           }

                           /* --- Category Indicator Styles --- */

                           /* Default style for all indicators */
                           .category-indicator {
                               background-color: #888; /* A default gray */
                               border-radius: 2px;
                           }

                           /* Your specific category colors */
                           .category-entertainment {
                               background-color: #E50914; /* Red */
                           }

                           .category-news {
                               background-color: #0078F2; /* Blue */
                           }

                           .category-utility {
                               background-color: #1DB954; /* Green  */
                           }

                           .category-sports {
                               background-color: #F5C518; /* Yellow */
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