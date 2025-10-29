# AboTracker (Subscription Tracker)

AboTracker is a simple, cross-platform desktop application built with .NET and Gtk\# to help you track your subscriptions. It provides a clear overview of all your recurring payments, calculates your total monthly cost, and shows you when each subscription is due for its next payment.

-----

## Features

  * **List Subscriptions:** View all your added subscriptions in a clean, scrollable list.
  * **Add Subscriptions:** Easily add new subscriptions with a name, amount, payment period (Daily, Weekly, Quarterly, Monthly, or Yearly), and the initial purchase date.
  * **Delete Subscriptions:** Remove subscriptions you no longer need with a single click.
  * **Monthly Cost Calculation:** See a live-updated total of your equivalent monthly spending, calculated from all your subscriptions regardless of their payment schedules.
  * **Next Payment Date:** The app automatically calculates and displays the next upcoming payment date for every subscription based on its purchase date and period.
  * **Sorting:** Organize your subscription list by various criteria, including:
      * Name (A-Z)
      * Amount (High-Low or Low-High)
      * Next Payment (Soonest)
      * Purchase Date (Newest)
      * Payment Period
  * **Data Persistence:** Your subscription list is saved locally in a `aboList.json` file, so your data is preserved between sessions.

-----

## How It Works

The application loads subscription data from a `aboList.json` file on startup. When you add or remove a subscription, this JSON file is automatically updated.

The core logic handles two main calculations:

1.  **Monthly Sum:** It converts the cost of each subscription (whether daily, weekly, quarterly, or yearly) into its equivalent yearly cost and then divides by 12 to provide a single, consistent monthly total.
2.  **Next Payment Date:** It iteratively adds the payment period (e.g., 1 month, 7 days, 1 year) to the original purchase date until it finds the next date that is in the future.

-----

## Installation & Running from Source

You can build and run this project from the source code.

### Prerequisites

1.  **.NET 9.0 SDK (or newer):** You must have the .NET 9.0 SDK installed.
2.  **GTK 4 Runtime:** This is a Gtk\# application, so you need the GTK 4 runtime libraries installed on your system.
      * **Linux:** Usually available through your package manager (e.g., `sudo apt install libgtk-4-1`).
      * **Windows/macOS:** You may need to install the GTK 4 runtime separately.

### Build and Run

1.  Clone this repository to your local machine.

2.  Open a terminal or command prompt.

3.  Navigate to the root folder of the project (the one containing `AboTracker.sln`).

4.  Run the following .NET CLI commands:

    ```sh
    # Restore the required packages
    dotnet restore

    # Build and run the application
    dotnet run --project AboTracker/AboTracker.csproj
    ```

-----

## Technology Stack

  * **.NET 9.0**
  * **Gtk\# 4 (via `GirCore.Gtk-4.0`)** for the graphical user interface
  * **C\#**

-----

## License

This project is licensed under the **MIT License**.
